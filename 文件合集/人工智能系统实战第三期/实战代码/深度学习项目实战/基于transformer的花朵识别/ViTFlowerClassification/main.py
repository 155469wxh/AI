import os
import time

from train import train
from predict import predict

if __name__ == '__main__':

    args = {
        # 部分参数需要斟酌调整大小！
        'num_classes': 5,  # 手动设置分成几类。该项目为花朵分类，所以设置为5类
        'label_name': ["daisy", "dandelion", "roses", "sunflowers", "tulips"],  # 手动设置标签名称
        'epochs': 100,  # 设置训练的轮数
        'batch_size': 128,  # 设置每批读入的图片数量
        'lr': 1e-3,  # 设置学习率
        'lrf': 1e-2, # 设置学习率优化策略的参数
        'train_dir': './flower/train',  # 设置训练集路径
        'val_dir': './flower/val',  # 设置测试集路径
        'summary_dir': './summary',  # 设置训练结果与日志的存储路径
        'use_weights': True,  #
        'gpu_list': '0',  # 对于多GPU训练，设置GPU列表
        'model_type': 'vit_base_patch16_224',  # 选择一个模型进行训练

    }

    # 此处代码根据需要，二选一运行即可
    # 如果使用预训练模型，设置使用预训练的模型的名称和位置
    args['weights_name'] = str(
        '/pretrained_model/' + args['model_type'] + '_' + time.strftime("%Y-%m-%d", time.localtime()) + '.pth')
    # 如果从头训练，则置为空字符串
    args['weights_name'] = ''


    # 给系统设置多gpu并行训练的gpu列表
    os.environ["CUDA_VISIBLE_DEVICES"] = args['gpu_list']

    # print('------------------训练阶段------------------')
    # # 进行训练
    # train(args=args)

    print('------------------测试阶段------------------')
    args['predicted_image'] = "./flower/val/daisy/173350276_02817aa8d5.jpg"
    args['saved_pth'] = "{}/weights/epoch=74_val_acc=0.5220.pth".format(args['summary_dir'])
    #选择一张图片进行测试
    predict(args=args)
