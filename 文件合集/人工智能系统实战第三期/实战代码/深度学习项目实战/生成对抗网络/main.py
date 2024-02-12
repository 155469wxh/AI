from DCGAN import build_dc_classifier, build_dc_generator
from utils import train_data, deprocess_img, show_images
import torch
from torch import nn
from torch.autograd import Variable

import torchvision.transforms as tfs
from torch.utils.data import DataLoader, sampler
from torchvision.datasets import MNIST

import numpy as np

import matplotlib.pyplot as plt
import matplotlib.gridspec as gridspec

if __name__ == '__main__':

    bce_loss = nn.BCEWithLogitsLoss()


    def discriminator_loss(logits_real, logits_fake):  # 判别器的 loss
        size = logits_real.shape[0]
        true_labels = Variable(torch.ones(size, 1)).float().cuda()
        false_labels = Variable(torch.zeros(size, 1)).float().cuda()
        loss = bce_loss(logits_real, true_labels) + bce_loss(logits_fake, false_labels)
        return loss

    # 使用 adam 来进行训练，学习率是 3e-4, beta1 是 0.5, beta2 是 0.999
    def get_optimizer(net):
        optimizer = torch.optim.Adam(net.parameters(), lr=3e-4, betas=(0.5, 0.999))
        return optimizer


    def generator_loss(logits_fake):  # 生成器的 loss
        size = logits_fake.shape[0]
        true_labels = Variable(torch.ones(size, 1)).float().cuda()
        loss = bce_loss(logits_fake, true_labels)
        return loss

    def train_dc_gan(D_net, G_net, D_optimizer, G_optimizer, discriminator_loss, generator_loss, show_every=250,
                     noise_size=96, num_epochs=4000):
        iter_count = 0
        for epoch in range(num_epochs):
            for x, _ in train_data:
                bs = x.shape[0]
                # 判别网络
                real_data = Variable(x).cuda()  # 真实数据
                logits_real = D_net(real_data)  # 判别网络得分

                sample_noise = (torch.rand(bs, noise_size) - 0.5) / 0.5  # -1 ~ 1 的均匀分布
                g_fake_seed = Variable(sample_noise).cuda()
                fake_images = G_net(g_fake_seed)  # 生成的假的数据
                logits_fake = D_net(fake_images)  # 判别网络得分

                d_total_error = discriminator_loss(logits_real, logits_fake)  # 判别器的 loss
                D_optimizer.zero_grad()
                d_total_error.backward()
                D_optimizer.step()  # 优化判别网络

                # 生成网络
                g_fake_seed = Variable(sample_noise).cuda()
                fake_images = G_net(g_fake_seed)  # 生成的假的数据

                gen_logits_fake = D_net(fake_images)
                g_error = generator_loss(gen_logits_fake)  # 生成网络的 loss
                G_optimizer.zero_grad()
                g_error.backward()
                G_optimizer.step()  # 优化生成网络

                if (iter_count % show_every == 0):
                    print('Iter: {}, D: {:.4}, G:{:.4}'.format(iter_count, d_total_error.item(), g_error.item()))
                    imgs_numpy = deprocess_img(fake_images.data.cpu().numpy())
                    show_images(imgs_numpy[0:16])
                    plt.show()
                    print()
                iter_count += 1


    D_DC = build_dc_classifier().cuda()
    G_DC = build_dc_generator().cuda()

    D_DC_optim = get_optimizer(D_DC)
    G_DC_optim = get_optimizer(G_DC)

    train_dc_gan(D_DC, G_DC, D_DC_optim, G_DC_optim, discriminator_loss, generator_loss, num_epochs=4000)
