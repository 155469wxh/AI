function [y,ygabptest,net]=myfun(x)

%��ȡ
global inputnumber hiddenumber outputnumber input_train output_train input_test P1 P2 T1 T2 inputps outputps indexM N;
global net0;
% S=[hiddenumber*inputnumber,outputnumber*hiddenumber,hiddenumber*1,outputnumber*1];
%Ȩֵ��ʼ��
index011=indexM(1,1);
index012=indexM(1,2);
index021=indexM(2,1);
index022=indexM(2,2);
index031=indexM(3,1);
index032=indexM(3,2);
index041=indexM(4,1);
index042=indexM(4,2);

line1=x(index011:index012);
line2=x(index021:index022);
line3=x(index031:index032);
line4=x(index041:index042);

mat1=reshape(line1,hiddenumber,inputnumber);
mat2=reshape(line2,outputnumber,hiddenumber);
mat3=reshape(line3,hiddenumber,1);
mat4=reshape(line4,outputnumber,1);

%����Ȩֵ��ֵ
net=net0;
net.iw{1,1}=mat1;
net.lw{2,1}=mat2;
net.b{1}=mat3;
net.b{2}=mat4;
% net.trainParam.showWindow = false; % ����ʾѵ������
% net.trainParam.showCommandLine = false;% �����в���ʾ���
% [net,tr]=train(net,input_train,output_train);%����ѵ��
ygabptest=sim(net,input_test);
ygabptest=mapminmax('reverse',ygabptest,outputps);%Ԥ�����ݷ���һ��

% y=sum((ygabptest-T2).^2);
y=sum((abs(ygabptest-T2)./T2).^2);

