function [x0,ybptest]=myfun2()
global inputnumber hiddenumber outputnumber input_train output_train input_test P1 P2 T1 T2 inputps outputps indexM N;
global net0;
%�½�bp������
net0=newff(input_train,output_train,hiddenumber);
net0.trainParam.epochs=100;%ѵ������
net0.trainParam.lr=0.1;%ѧϰ��
net0.trainParam.goal=0.00001;
net0.trainParam.showWindow = false; % ����ʾѵ������
net0.trainParam.showCommandLine = false;% �����в���ʾ���
net0.trainFcn='traingd';

[net0,tr]=train(net0,input_train,output_train);
mse01=tr.perf;%ѵ�����
epochs=tr.epoch;% ѵ������
% mse01

figure;
semilogy(epochs,mse01);
xlabel('ѵ������','fontname','����'); 
ylabel('ѵ�����','fontname','����'); 
title('BP������ѵ���������','fontname','����'); 


ybptrain=sim(net0,input_train);
ybptrain=mapminmax('reverse',ybptrain,outputps);%Ԥ�����ݷ���һ��

ybptest=sim(net0,input_test);
ybptest=mapminmax('reverse',ybptest,outputps);%Ԥ�����ݷ���һ��

%�ܽڵ�������
% ��ֵ
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
