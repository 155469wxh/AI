import torch
from torchvision import datasets, transforms

# 数据预处理：转换为Tensor并标准化
transform = transforms.Compose([
    transforms.ToTensor(),
    transforms.Normalize((0.5,), (0.5,))
])

# 下载并加载MNIST数据集
train_dataset = datasets.MNIST(root='./data', train=True, download=True, transform=transform)
train_loader = torch.utils.data.DataLoader(dataset=train_dataset, batch_size=64, shuffle=True)


import torch.nn as nn
import torch.nn.functional as F

class Autoencoder(nn.Module):
    def __init__(self):
        super(Autoencoder, self).__init__()
        # 编码器
        self.encoder = nn.Sequential(
            nn.Linear(28*28, 128),
            nn.ReLU(True),
            nn.Linear(128, 64),
            nn.ReLU(True),
            nn.Linear(64, 12),
            nn.ReLU(True),
            nn.Linear(12, 3)  # 压缩到3个特征以便于可视化
        )
        # 解码器
        self.decoder = nn.Sequential(
            nn.Linear(3, 12),
            nn.ReLU(True),
            nn.Linear(12, 64),
            nn.ReLU(True),
            nn.Linear(64, 128),
            nn.ReLU(True),
            nn.Linear(128, 28*28),
            nn.Tanh()
        )

    def forward(self, x):
        x = self.encoder(x)
        x = self.decoder(x)
        return x

# 创建模型和优化器
model = Autoencoder() 
criterion = nn.MSELoss()
optimizer = torch.optim.Adam(model.parameters(), lr=1e-3)

# 训练模型
num_epochs = 5
for epoch in range(num_epochs):
    """
    ##############################################
    ##############################################
    ##############################################
    ##############################################
    code
    ##############################################
    ##############################################
    ##############################################
    ##############################################
    """
    
    print(f'Epoch [{epoch+1}/{num_epochs}], Loss: {loss.item():.4f}')


def retrieve_images(query_image, train_loader, model, n=5):

    """
    实现代码逻辑，返回与查询图片最像的n张图片
    ##############################################
    ##############################################
    code
    ##############################################
    ##############################################
    ##############################################
    ##############################################
    """
    return [x[1] for x in distances[:n]]  # 返回最近的n张图片

import matplotlib.pyplot as plt
import numpy as np

def visualize_retrieval(query_image, retrieved_images):
    plt.figure(figsize=(10, 2))

    # 显示查询图片
    plt.subplot(1, len(retrieved_images) + 1, 1)
    plt.imshow(query_image.reshape(28, 28), cmap='gray')
    plt.title('Query Image')
    plt.axis('off')

    # 显示检索到的图片
    for i, img in enumerate(retrieved_images, 2):
        plt.subplot(1, len(retrieved_images) + 1, i)
        plt.imshow(img.reshape(28, 28), cmap='gray')
        plt.title(f'Retrieved {i-1}')
        plt.axis('off')

    plt.show()

test_dataset = datasets.MNIST(root='../data', train=False, download=True, transform=transform)
test_loader = torch.utils.data.DataLoader(dataset=test_dataset, batch_size=1, shuffle=True)
for img, _ in test_loader:
    query_image = img.view(img.size(0), -1) 
    break  # 只取第一张图片


# 假设 retrieve_images 和 model 已经定义
retrieved_images = retrieve_images(query_image, train_loader, model, n=5)
# 假设 visualize_retrieval 已经定义
visualize_retrieval(query_image.cpu().squeeze(), [img.squeeze() for img in retrieved_images])
