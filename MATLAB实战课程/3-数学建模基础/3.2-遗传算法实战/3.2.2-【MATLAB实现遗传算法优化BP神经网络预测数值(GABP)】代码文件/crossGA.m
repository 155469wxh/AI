function Chrom=crossGA(Chrom,popsize,PC,N)
% ��Ⱥ����,2��佻�� 
PNumber=N; 
index1=randperm(popsize); 
long1=fix(popsize/2);
set1=index1(1:long1);
set2=index1(long1+1:long1*2);
for i=1:long1
    a=rand;
    if a<PC%С�ڱ����ʿ�ʼ����
        index1=set1(i);
        index2=set2(i);
        R1=Chrom(index1,:);
        R2=Chrom(index2,:);
        r1=unidrnd(PNumber);%
        r2=unidrnd(PNumber);%
        if r1>r2%ȷ��r2>=r1
            t1=r1;
            r1=r2;
            r2=t1;
        end
        S1=R1(r1:r2);%����λ�õĽڵ�
        S2=R2(r1:r2);%����λ�õĽڵ�
        R1(r1:r2)=S2;
        R2(r1:r2)=S1;
        %����Ⱦɫ��
        Chrom(index1,:)=R1;
        Chrom(index2,:)=R2;
    end
end
