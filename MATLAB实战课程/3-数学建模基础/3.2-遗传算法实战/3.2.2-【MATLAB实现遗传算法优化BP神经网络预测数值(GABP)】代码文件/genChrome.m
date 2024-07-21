function Chrom=genChrome(popsize,N,lb,ub,x0)
% 建立种群 
Chrom=zeros(popsize,N);
for i=1:popsize
    % Chrom(i,:)=x0.*(1+(-0.1+0.2*rand(size(x0))));
    Chrom(i,:)=x0;
end
