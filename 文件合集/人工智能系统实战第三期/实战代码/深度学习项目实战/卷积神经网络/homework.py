import torch.nn as nn
import torch 

class AlexNet(nn.Module):
    def __init__(self, num_classes=1000 ):
        super(AlexNet, self).__init__()
        self.features = nn.Sequential(
            nn.Conv2d(3, 96, kernel_size=11, stride=4, padding=2),  # input[3, 224, 224]  output[96, 55, 55]
            nn.ReLU(inplace=True),
            nn.MaxPool2d(kernel_size=3, stride=2),                  # output[96, 27, 27]

            """
            补全代码
            """
        )  
        self.classifier = nn.Sequential(
            nn.Dropout(p=0.5),
            nn.Linear(256 * 6 * 6, 4096),
            nn.ReLU(inplace=True),

            """
            补全代码
            """
        )
  
    def forward(self, x):
        x = self.features(x)
        x = torch.flatten(x, start_dim=1)
        x = self.classifier(x)
        return x
 
import os 
import sys 
import torch
import torch.nn as nn
from torchvision import transforms, datasets 
import torch.optim as optim 
from tqdm import tqdm  
 
def main():
    # 判断可用设备
    device = torch.device("cuda:0" if torch.cuda.is_available() else "cpu")
    print("using {} device.".format(device))

    # 注意改成自己的数据集路径
    data_path = './data'
    assert os.path.exists(data_path), "{} path does not exist.".format(data_path) 

    # 数据预处理与增强
    """ 
    ToTensor()能够把灰度范围从0-255变换到0-1之间的张量.
    transform.Normalize()则把0-1变换到(-1,1). 具体地说, 对每个通道而言, Normalize执行以下操作: image=(image-mean)/std
    其中mean和std分别通过(0.5,0.5,0.5)和(0.5,0.5,0.5)进行指定。原来的0-1最小值0则变成(0-0.5)/0.5=-1; 而最大值1则变成(1-0.5)/0.5=1. 
    也就是一个均值为0, 方差为1的正态分布. 这样的数据输入格式可以使神经网络更快收敛。
    """
    data_transform = {
        "train": transforms.Compose([transforms.Resize(224),
                                     transforms.CenterCrop(224),
                                     transforms.RandomHorizontalFlip(p=0.5), # 依概率p水平翻转
                                     transforms.ToTensor(),
                                     transforms.Normalize((0.5, 0.5, 0.5), (0.5, 0.5, 0.5))]),

        "val": transforms.Compose([transforms.Resize((224, 224)),  # val不需要任何数据增强
                                   transforms.ToTensor(),
                                   transforms.Normalize((0.5, 0.5, 0.5), (0.5, 0.5, 0.5))])}


    # 使用ImageFlolder加载数据集中的图像，并使用指定的预处理操作来处理图像， ImageFlolder会同时返回图像和对应的标签。 (image path, class_index) tuples
    train_dataset = datasets.ImageFolder(root=os.path.join(data_path, "train"), transform=data_transform["train"])
    validate_dataset = datasets.ImageFolder(root=os.path.join(data_path, "val"), transform=data_transform["val"])
    train_num = len(train_dataset)
    val_num = len(validate_dataset) 

    batch_size = 64 # batch_size大小，是超参，可调，如果模型跑不起来，尝试调小batch_size
 
    # 使用 DataLoader 将 ImageFloder 加载的数据集处理成批量（batch）加载模式
    train_loader = torch.utils.data.DataLoader(train_dataset, batch_size=batch_size, shuffle=True )
    validate_loader = torch.utils.data.DataLoader(validate_dataset, batch_size=4, shuffle=False ) # 注意，验证集不需要shuffle
    print("using {} images for training, {} images for validation.".format(train_num, val_num))
    
    # 实例化模型，并送进设备
    net = AlexNet(num_classes=5 )
    net.to(device)

    # 指定损失函数用于计算损失；指定优化器用于更新模型参数；指定训练迭代的轮数，训练权重的存储地址
    loss_function = nn.CrossEntropyLoss()  # MSELoss
    optimizer = optim.Adam(net.parameters(), lr=0.0002)
    epochs = 70
    save_path = os.path.abspath(os.path.join(os.getcwd(), './results/weights/alexnet')) 
    if not os.path.exists(save_path):    
        os.makedirs(save_path)
 
    for epoch in range(epochs):
        ############################################################## train ######################################################
        net.train() 
        acc_num = torch.zeros(1).to(device)    # 初始化，用于计算训练过程中预测正确的数量
        sample_num = 0                         # 初始化，用于记录当前迭代中，已经计算了多少个样本
        # tqdm是一个进度条显示器，可以在终端打印出现在的训练进度
        train_bar = tqdm(train_loader, file=sys.stdout, ncols=100)
        for data in train_bar :
            images, labels = data 
            sample_num += images.shape[0] #[64, 3, 224, 224]
            optimizer.zero_grad()
            """
            补全代码
            """

            # print statistics 
            train_acc = acc_num.item() / sample_num 
            # .desc是进度条tqdm中的成员变量，作用是描述信息
            train_bar.desc = "train epoch[{}/{}] loss:{:.3f}".format(epoch + 1,  epochs, loss)

        # validate
        net.eval()
        acc_num = 0.0  # accumulate accurate number per epoch
        with torch.no_grad(): 
            for val_data in validate_loader:
                val_images, val_labels = val_data
                outputs = net(val_images.to(device)) 
                predict_y = torch.max(outputs, dim=1)[1] 
                acc_num += torch.eq(predict_y, val_labels.to(device)).sum().item() 

        val_accurate = acc_num / val_num
        print('[epoch %d] train_loss: %.3f  train_acc: %.3f  val_accuracy: %.3f' %  (epoch + 1, loss, train_acc, val_accurate))   

        torch.save(net.state_dict(), os.path.join(save_path, "AlexNet.pth") )

        # 每次迭代后清空这些指标，重新计算 
        train_acc = 0.0
        val_accurate = 0.0

    print('Finished Training')

 
# if __name__ == '__main__':
#     main()
main()