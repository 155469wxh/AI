%% 遗传算法 优化BP
clc;close all;clear all;%清除变量
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
% index200=1:snumber;%顺序样本
index200=randperm(snumber);%随机样本
numberTest=50;%用于测试的样本个数

indextrain=index200(1:end-numberTest);
indextest=index200(end-numberTest+1:end);

% 定义训练集
P1=Inputdata(:,indextrain);
T1=Outputdata(:,indextrain);
% 定义测试集
P2=Inputdata(:,indextest);
T2=Outputdata(:,indextest);
%设置训练集和测试集

%% (2)训练数据归一化
[input_train,inputps]=mapminmax(P1);
[output_train,outputps]=mapminmax(T1);
%测试数据归一化
input_test=mapminmax('apply',P2,inputps);
output_test=mapminmax('apply',T2,outputps);


[k11,k12]=size(input_train);%计算维数
inputnumber=k11;%输入维数
hiddenumber=8;%隐含神经元数
outputnumber=size(output_train,1);%输出维数;%输出维数
S=[hiddenumber*inputnumber,outputnumber*hiddenumber,hiddenumber*1,outputnumber*1];
indexM=S2indexMfun(S);
N=sum(S);% 编码长度

tic;
[x0,ybptest]=myfun2();
bptime=toc;


% 遗传算法参数
popsize=50;%遗传算法种群数
maxgen=100;%遗传算法迭代次数
PM=0.1;%变异概率
PC=0.8;%交叉概率

lb=-3*ones(1,N);
ub=3*ones(1,N);

%% 遗传算法主程序
%性能跟踪
tic;
tracematga=zeros(maxgen,2);
gen=0;
Chrom=genChrome(popsize,N,lb,ub,x0);% 建立种群
Value = decodingFun(Chrom,popsize);% 解码染色体
[vmin,indexmin]=min(Value);
bestValue=vmin;% 记录函数的最优值
bestChrom=Chrom(indexmin,:);% 记录函数的最优染色体

%% 遗传算法优化
%进度条
wait_hand = waitbar(0,'running...', 'tag', 'TMWWaitbar');
while gen<maxgen
    %% 遗传算子
    FitnV=ranking(Value);% 分配适应度值
    Chrom=select('rws',Chrom,FitnV,1);% 选择
    Chrom=mutationGA(Chrom,popsize,PM,N,lb,ub);% 种群变异,单点变异
    Chrom=crossGA(Chrom,popsize,PC,N);% 种群交叉,2点交叉
    Value = decodingFun(Chrom,popsize);% 解码染色体
    
    %% 计算最优
    [vmin,indexmin]=min(Value);
    gen=gen+1;
    
    %% 记录最优
    if bestValue>vmin
        bestValue=vmin;% 记录函数的最优值
        bestChrom=Chrom(indexmin,:);% 记录最优染色体
    end
    tracematga(gen,1)=bestValue;% 保留最优
    tracematga(gen,2)=mean(Value);
    
    waitbar(gen/maxgen,wait_hand);% 每循环一次更新一次进步条
end
delete(wait_hand);% 执行完后删除该进度条
gabptime=toc;% 运行时间

% 显示结果
disp('遗传算法优化得到的最优目标函数');
bestValue
% disp('遗传算法优化得到的最优染色体');
% bestChrom


figure;
plot(tracematga(:,1),'r-','linewidth',1);
legend({'种群最优值'},'fontname','宋体');
xlabel('迭代次数','fontname','宋体');
ylabel('目标函数','fontname','宋体');
title('遗传算法优化BP神经网络收敛曲线','fontname','宋体');


%% 以优化得到的权值阀值寻bp神经网络
x=bestChrom;
[y,y_ga_bp_test,net]=myfun(x);


ygabptrain=sim(net,input_train);
ygabptrain=mapminmax('reverse',ygabptrain,outputps);%预测数据反归一化
[vmin,ygabptrain]=max(ygabptrain);

y_ga_bp_test=sim(net,input_test);
y_ga_bp_test=mapminmax('reverse',y_ga_bp_test,outputps);%预测数据反归一化


% 输出测试结果的绘图
figure;
plot(T2,'bo-');
hold on;
plot(y_ga_bp_test,'r*-');
legend({'实际值','GA-BP预测值'},'fontname','宋体');
xlabel('测试样本号','fontname','宋体');
ylabel('目标值','fontname','宋体');
title('GA-BP神经网络测试集预测的结果','fontname','宋体');

figure;
plot(y_ga_bp_test-T2,'r*-');
legend({'绝对误差'},'fontname','宋体');
xlabel('测试样本号','fontname','宋体');
ylabel('绝对误差','fontname','宋体');
title('GABP神经网络测试集预测的绝对误差','fontname','宋体');

figure;
plot((y_ga_bp_test-T2)./T2*100,'r*-');
legend({'相对误差'},'fontname','宋体');
xlabel('测试样本号','fontname','宋体');
ylabel('相对误差(%)','fontname','宋体');
title('GABP神经网络测试集预测的相对误差','fontname','宋体');


%% 普通BP
% 输出测试结果的绘图
figure;
plot(T2,'bo-');
hold on;
plot(ybptest,'r*-');
legend({'实际值','BP预测值'},'fontname','宋体');
xlabel('测试样本号','fontname','宋体');
ylabel('目标值','fontname','宋体');
title('BP神经网络测试集预测的结果','fontname','宋体');

figure;
plot(ybptest-T2,'r*-');
legend({'绝对误差'},'fontname','宋体');
xlabel('测试样本号','fontname','宋体');
ylabel('绝对误差','fontname','宋体');
title('BP神经网络测试集预测的绝对误差','fontname','宋体');

figure;
plot((ybptest-T2)./T2*100,'r*-');
legend({'相对误差'},'fontname','宋体');
xlabel('测试样本号','fontname','宋体');
ylabel('误差(%)','fontname','宋体');
title('BP神经网络测试集预测的相对误差','fontname','宋体');

% 时间
disp('GA-BP运行时间(s)');
gabptime

disp('GA-BP预测结果的各项指标');
y=T2;
y1=y_ga_bp_test;
[R2_GABP,MSE_GABP,RMSE_GABP,MAPE_GABP,MAD_GABP]=predictorsfun(y,y1);


disp('bp运行时间(s)');
bptime
disp('BP预测结果的各项指标');
y=T2;
y1=ybptest;

[R2_BP,MSE_BP,RMSE_BP,MAPE_BP,MAD_BP]=predictorsfun(y,y1);

outcell={'算法','R2','MSE','RMSE','MAPE','MAD'};
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
legend({'实际值','BP预测值','GA-BP预测值'},'fontname','宋体');
xlabel('测试样本号','fontname','宋体');
ylabel('目标值','fontname','宋体');
title('','fontname','宋体');

figure;
hold on;
plot((ybptest-T2)./T2*100,'gs-');
plot((y_ga_bp_test-T2)./T2*100,'r*-');
legend({'BP','GA-BP'},'fontname','宋体');
xlabel('测试样本号','fontname','宋体');
ylabel('误差(%)','fontname','宋体');
title('','fontname','宋体');


% 输出结果
outcell={'期望输出','BP实际输出','绝对误差','误差(%)'};
E1=ybptest-T2;
E2=(ybptest-T2)./(T2)*100;
outcell2=num2cell([T2',ybptest',E1',E2']);
outcell=[outcell;outcell2];
xlswrite('输出_BP预测结果.xlsx',outcell);

outcell={'期望输出','GABP实际输出','绝对误差','误差(%)'};
E1=y_ga_bp_test-T2;
E2=(y_ga_bp_test-T2)./(T2)*100;
outcell2=num2cell([T2',y_ga_bp_test',E1',E2']);
outcell=[outcell;outcell2];
xlswrite('输出_GABP预测结果.xlsx',outcell);




rmpath(genpath('mytoolbox'));
