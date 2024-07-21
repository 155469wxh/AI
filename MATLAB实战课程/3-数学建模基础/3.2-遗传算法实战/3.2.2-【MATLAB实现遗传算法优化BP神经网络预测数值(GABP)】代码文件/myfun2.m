function [x0,ybptest]=myfun2()
global inputnumber hiddenumber outputnumber input_train output_train input_test P1 P2 T1 T2 inputps outputps indexM N;
global net0;
%新建bp神经网络
net0=newff(input_train,output_train,hiddenumber);
net0.trainParam.epochs=100;%训练次数
net0.trainParam.lr=0.1;%学习率
net0.trainParam.goal=0.00001;
net0.trainParam.showWindow = false; % 不显示训练窗口
net0.trainParam.showCommandLine = false;% 命令行不显示结果
net0.trainFcn='traingd';

[net0,tr]=train(net0,input_train,output_train);
mse01=tr.perf;%训练误差
epochs=tr.epoch;% 训练次数
% mse01

figure;
semilogy(epochs,mse01);
xlabel('训练次数','fontname','宋体'); 
ylabel('训练误差','fontname','宋体'); 
title('BP神经网络训练误差曲线','fontname','宋体'); 


ybptrain=sim(net0,input_train);
ybptrain=mapminmax('reverse',ybptrain,outputps);%预测数据反归一化

ybptest=sim(net0,input_test);
ybptest=mapminmax('reverse',ybptest,outputps);%预测数据反归一化

%总节点数计算
% 赋值
x0=zeros(1,N);
index011=indexM(1,1);
index012=indexM(1,2);
index021=indexM(2,1);
index022=indexM(2,2);
index031=indexM(3,1);
index032=indexM(3,2);
index041=indexM(4,1);
index042=indexM(4,2);

mat1=net0.iw{1,1};
mat2=net0.lw{2,1};
mat3=net0.b{1};
mat4=net0.b{2};

line1=reshape(mat1,1,hiddenumber*inputnumber);
line2=reshape(mat2,1,outputnumber*hiddenumber);
line3=reshape(mat3,1,1*hiddenumber);
line4=reshape(mat4,1,1*outputnumber);

x0(index011:index012)=line1;
x0(index021:index022)=line2;
x0(index031:index032)=line3;
x0(index041:index042)=line4;
