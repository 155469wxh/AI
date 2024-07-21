function NewChrIx = rws(FitnV,Nsel)
% бжел╤дя║тЯ
[Nind,ans] = size(FitnV);
cumfit  = cumsum(FitnV); 
trials = cumfit(Nind) .* rand(Nsel, 1);
Mf = cumfit(:, ones(1, Nsel));
Mt = trials(:, ones(1, Nind))';
[NewChrIx, ans] = find(Mt < Mf & ...
    [ zeros(1, Nsel); Mf(1:Nind-1, :) ] <= Mt);
