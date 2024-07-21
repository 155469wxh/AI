%% �Ŵ��㷨 �Ż�BP
clc;close all;clear all;%�������
rand('seed',1000);
randn('seed',1000);
format long g;
addpath(genpath('mytoolbox'));

global inputnumber hiddenumber outputnumber input_train output_train input_test P1 P2 T1 T2 inputps outputps indexM N;

X=rand(200,3);
Y=sum(X,2);

Inputdata=X';
Outputdata=Y';

snumber=size(Outputdata,2);
% index200=1:snumber;%˳������
index200=randperm(snumber);%�������
numberTest=50;%���ڲ��Ե���������

indextrain=index200(1:end-numberTest);
indextest=index200(end-numberTest+1:end);

% ����ѵ����
P1=Inputdata(:,indextrain);
T1=Outputdata(:,indextrain);
% ������Լ�
P2=Inputdata(:,indextest);
T2=Outputdata(:,indextest);
%����ѵ�����Ͳ��Լ�

%% (2)ѵ�����ݹ�һ��
[input_train,inputps]=mapminmax(P1);
[output_train,outputps]=mapminmax(T1);
%�������ݹ�һ��
input_test=mapminmax('apply',P2,inputps);
output_test=mapminmax('apply',T2,outputps);


[k11,k12]=size(input_train);%����ά��
inputnumber=k11;%����ά��
hiddenumber=8;%������Ԫ��
outputnumber=size(output_train,1);%���ά��;%���ά��
S=[hiddenumber*inputnumber,outputnumber*hiddenumber,hiddenumber*1,outputnumber*1];
indexM=S2indexMfun(S);
N=sum(S);% ���볤��

tic;
[x0,ybptest]=myfun2();
bptime=toc;


% �Ŵ��㷨����
popsize=50;%�Ŵ��㷨��Ⱥ��
maxgen=100;%�Ŵ��㷨��������
PM=0.1;%�������
PC=0.8;%�������

lb=-3*ones(1,N);
ub=3*ones(1,N);

%% �Ŵ��㷨������
%���ܸ���
tic;
tracematga=zeros(maxgen,2);
gen=0;
Chrom=genChrome(popsize,N,lb,ub,x0);% ������Ⱥ
Value = decodingFun(Chrom,popsize);% ����Ⱦɫ��
[vmin,indexmin]=min(Value);
bestValue=vmin;% ��¼����������ֵ
bestChrom=Chrom(indexmin,:);% ��¼����������Ⱦɫ��

%% �Ŵ��㷨�Ż�
%������
wait_hand = waitbar(0,'running...', 'tag', 'TMWWaitbar');
while gen<maxgen
    %% �Ŵ�����
    FitnV=ranking(Value);% ������Ӧ��ֵ
    Chrom=select('rws',Chrom,FitnV,1);% ѡ��
    Chrom=mutationGA(Chrom,popsize,PM,N,lb,ub);% ��Ⱥ����,�������
    Chrom=crossGA(Chrom,popsize,PC,N);% ��Ⱥ����,2�㽻��
    Value = decodingFun(Chrom,popsize);% ����Ⱦɫ��
    
    %% ��������
    [vmin,indexmin]=min(Value);
    gen=gen+1;
    
    %% ��¼����
    if bestValue>vmin
        bestValue=vmin;% ��¼����������ֵ
        bestChrom=Chrom(indexmin,:);% ��¼����Ⱦɫ��
    end
    tracematga(gen,1)=bestValue;% ��������
    tracematga(gen,2)=mean(Value);
    
    waitbar(gen/maxgen,wait_hand);% ÿѭ��һ�θ���һ�ν�����
end
delete(wait_hand);% ִ�����ɾ���ý�����
gabptime=toc;% ����ʱ��

% ��ʾ���
disp('�Ŵ��㷨�Ż��õ�������Ŀ�꺯��');
bestValue
% disp('�Ŵ��㷨�Ż��õ�������Ⱦɫ��');
% bestChrom


figure;
plot(tracematga(:,1),'r-','linewidth',1);
legend({'��Ⱥ����ֵ'},'fontname','����');
xlabel('��������','fontname','����');
ylabel('Ŀ�꺯��','fontname','����');
title('�Ŵ��㷨�Ż�BP��������������','fontname','����');


%% ���Ż��õ���Ȩֵ��ֵѰbp������
x=bestChrom;
[y,y_ga_bp_test,net]=myfun(x);


ygabptrain=sim(net,input_train);
ygabptrain=mapminmax('reverse',ygabptrain,outputps);%Ԥ�����ݷ���һ��
[vmin,ygabptrain]=max(ygabptrain);

y_ga_bp_test=sim(net,input_test);
y_ga_bp_test=mapminmax('reverse',y_ga_bp_test,outputps);%Ԥ�����ݷ���һ��


% ������Խ���Ļ�ͼ
figure;
plot(T2,'bo-');
hold on;
plot(y_ga_bp_test,'r*-');
legend({'ʵ��ֵ','GA-BPԤ��ֵ'},'fontname','����');
xlabel('����������','fontname','����');
ylabel('Ŀ��ֵ','fontname','����');
title('GA-BP��������Լ�Ԥ��Ľ��','fontname','����');

figure;
plot(y_ga_bp_test-T2,'r*-');
legend({'�������'},'fontname','����');
xlabel('����������','fontname','����');
ylabel('�������','fontname','����');
title('GABP��������Լ�Ԥ��ľ������','fontname','����');

figure;
plot((y_ga_bp_test-T2)./T2*100,'r*-');
legend({'������'},'fontname','����');
xlabel('����������','fontname','����');
ylabel('������(%)','fontname','����');
title('GABP��������Լ�Ԥ���������','fontname','����');


%% ��ͨBP
% ������Խ���Ļ�ͼ
figure;
plot(T2,'bo-');
hold on;
plot(ybptest,'r*-');
legend({'ʵ��ֵ','BPԤ��ֵ'},'fontname','����');
xlabel('����������','fontname','����');
ylabel('Ŀ��ֵ','fontname','����');
title('BP��������Լ�Ԥ��Ľ��','fontname','����');

figure;
plot(ybptest-T2,'r*-');
legend({'�������'},'fontname','����');
xlabel('����������','fontname','����');
ylabel('�������','fontname','����');
title('BP��������Լ�Ԥ��ľ������','fontname','����');

figure;
plot((ybptest-T2)./T2*100,'r*-');
legend({'������'},'fontname','����');
xlabel('����������','fontname','����');
ylabel('���(%)','fontname','����');
title('BP��������Լ�Ԥ���������','fontname','����');

% ʱ��
disp('GA-BP����ʱ��(s)');
gabptime

disp('GA-BPԤ�����ĸ���ָ��');
y=T2;
y1=y_ga_bp_test;
[R2_GABP,MSE_GABP,RMSE_GABP,MAPE_GABP,MAD_GABP]=predictorsfun(y,y1);


disp('bp����ʱ��(s)');
bptime
disp('BPԤ�����ĸ���ָ��');
y=T2;
y1=ybptest;

[R2_BP,MSE_BP,RMSE_BP,MAPE_BP,MAD_BP]=predictorsfun(y,y1);

outcell={'�㷨','R2','MSE','RMSE','MAPE','MAD'};
outcell201={'GA-BP';'BP'};
outmat=[R2_GABP,MSE_GABP,RMSE_GABP,MAPE_GABP,MAD_GABP;
    R2_BP,MSE_BP,RMSE_BP,MAPE_BP,MAD_BP];

outcell=[outcell;
    outcell201,num2cell(outmat)]



figure;
plot(T2,'bo-');
hold on;
plot(ybptest,'gs-');
plot(y_ga_bp_test,'r*-');
legend({'ʵ��ֵ','BPԤ��ֵ','GA-BPԤ��ֵ'},'fontname','����');
xlabel('����������','fontname','����');
ylabel('Ŀ��ֵ','fontname','����');
title('','fontname','����');

figure;
hold on;
plot((ybptest-T2)./T2*100,'gs-');
plot((y_ga_bp_test-T2)./T2*100,'r*-');
legend({'BP','GA-BP'},'fontname','����');
xlabel('����������','fontname','����');
ylabel('���(%)','fontname','����');
title('','fontname','����');


% ������
outcell={'�������','BPʵ�����','�������','���(%)'};
E1=ybptest-T2;
E2=(ybptest-T2)./(T2)*100;
outcell2=num2cell([T2',ybptest',E1',E2']);
outcell=[outcell;outcell2];
xlswrite('���_BPԤ����.xlsx',outcell);

outcell={'�������','GABPʵ�����','�������','���(%)'};
E1=y_ga_bp_test-T2;
E2=(y_ga_bp_test-T2)./(T2)*100;
outcell2=num2cell([T2',y_ga_bp_test',E1',E2']);
outcell=[outcell;outcell2];
xlswrite('���_GABPԤ����.xlsx',outcell);




rmpath(genpath('mytoolbox'));
