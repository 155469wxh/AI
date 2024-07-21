function Value = decodingFun(Chrom,popsize)
%½âÂëÈ¾É«Ìå
Value=zeros(popsize,1);
for i=1:popsize
    x=Chrom(i,:);
    y=myfun(x);
    Value(i,1)=y;
end
