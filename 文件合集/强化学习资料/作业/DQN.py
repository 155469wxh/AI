import pandas as pd
import torch
import torch.nn as nn
import torch.optim as optim
import torch.nn.functional as F
import numpy as np
from matplotlib import pyplot as plt


class Environment:
    def __init__(self, data, initial_oi, max_steps):
        self.data = data
        self.initial_oi = initial_oi
        self.max_steps = max_steps
        self.reset()

    def reset(self):
        self.step = 0
        self.position = self.initial_oi
        self.profit = 0
        self.done = False

    def get_state(self):
        return np.array([
            self.data['open'][self.step],
            self.data['high'][self.step],
            self.data['low'][self.step],
            self.data['close'][self.step],
            self.data['volume'][self.step],
            self.data['open_oi'][self.step],
            self.data['close_oi'][self.step],
            self.position
        ])

    def take_action(self, action):
        if action == 0:  # Buy
            self.position += int(0.2 * self.position)
        elif action == 1:  # Sell
            self.position -= int(0.2 * self.position)

        self.profit += self.position * (self.data['close'][self.step + 1] - self.data['open'][self.step])

        self.step += 1
        if self.step >= self.max_steps:
            self.done = True

    def get_reward(self):
        return self.profit

    def is_done(self):
        return self.done


class DQN(nn.Module):
    def __init__(self, state_size, action_size):
        super(DQN, self).__init__()
        self.fc1 = nn.Linear(state_size, 64)
        self.fc2 = nn.Linear(64, 64)
        self.fc3 = nn.Linear(64, action_size)

    def forward(self, x):
        # 改进，尝试加入残差模块
        out1 = F.relu(self.fc1(x))
        out2 = F.relu(self.fc2(out1))
        out2 += out1
        x = self.fc3(out2)
        return x

class Agent:
    def __init__(self, state_size, action_size, learning_rate, gamma, epsilon):
        self.state_size = state_size
        self.action_size = action_size
        self.learning_rate = learning_rate
        self.gamma = gamma
        self.epsilon = epsilon
        self.device = torch.device("cuda" if torch.cuda.is_available() else "cpu")

        # Q-Network
        self.q_network = DQN(state_size, action_size).to(self.device)
        self.optimizer = optim.Adam(self.q_network.parameters(), lr=learning_rate)

    def get_action(self, state):
        state = torch.from_numpy(state).float().unsqueeze(0).to(self.device)
        with torch.no_grad():
            q_values = self.q_network(state)
        if np.random.rand() <= self.epsilon:
            action = np.random.choice(self.action_size)
        else:
            action = torch.argmax(q_values).item()
        return action

    def update_model(self, state, action, reward, next_state, done):   #   on-policy offpolicy  传入s,a,r,next_s
        state = torch.from_numpy(state).float().unsqueeze(0).to(self.device)
        next_state = torch.from_numpy(next_state).float().unsqueeze(0).to(self.device)
        action = torch.tensor(action).long().unsqueeze(0).to(self.device)
        reward = torch.tensor([reward], dtype=torch.float).unsqueeze(0).to(self.device)
        done = torch.tensor([done], dtype=torch.float).unsqueeze(0).to(self.device)

        q_values = self.q_network(state)
        next_q_values = self.q_network(next_state)

        q_value = q_values.gather(1, action.unsqueeze(1)).squeeze(1)
        next_q_value = next_q_values.max(1)[0]
        expected_q_value = reward + self.gamma * next_q_value * (1 - done)

        loss = F.smooth_l1_loss(q_value, expected_q_value.detach())

        self.optimizer.zero_grad()
        loss.backward()
        self.optimizer.step()


# 读取CSV数据
data = pd.read_csv('G:/AAADT/code/TCL/CZCE.TA401_kline_1m.csv')

# 设置超参数
state_size = 8
action_size = 3
learning_rate = 0.001
gamma = 0.99
epsilon = 0.1
initial_oi = data['open_oi'][0]
max_steps = len(data) - 1

# 创建Agent和Environment实例
agent = Agent(state_size, action_size, learning_rate, gamma, epsilon)
env = Environment(data, initial_oi, max_steps)

# 训练Agent
num_episodes = 100
show = []
for episode in range(num_episodes):
    env.reset()
    state = env.get_state()
    total_reward = 0

    while not env.is_done():
        action = agent.get_action(state)
        env.take_action(action)
        next_state = env.get_state()
        reward = env.get_reward()
        done = env.is_done()

        agent.update_model(state, action, reward, next_state, done)

        state = next_state
        total_reward += reward
    if episode % 10 == 0:
        show.append(total_reward)
        print(f"Episode: {episode + 1}, Total Reward: {total_reward}")

# 使用训练好的Agent进行交易
# env.reset()
# state = env.get_state()
# total_reward = 0
#
# while not env.is_done():
#     action = agent.get_action(state)
#     env.take_action(action)
#     next_state = env.get_state()
#     reward = env.get_reward()
#     done = env.is_done()
#
#     state = next_state
#     total_reward += reward
# print(f"Total Reward: {total_reward}")


plt.plot(show, '-', c='r', label='reward')
plt.show()

