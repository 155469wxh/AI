options = gaoptimset('PopulationSize', 20, ...     % 种群包含个体数目
                     'EliteCount', 10, ...          % 种群中精英个体数目
                     'CrossoverFraction', 0.75, ... % 交叉后代比率
                     'Generations', 2000, ...        % 迭代代数
                     'StallGenLimit', 1500, ...      % 停止代数
                     'TolFun', 1e-100, ...          % 适应度函数偏差
                     'PlotFcns', {@gaplotbestf,  @gaplotbestindiv, @gaplotstopping}); % 绘制最优个体适应度函数与最优个体
[X,fval,exitflag,output] = ga(@myfunc,2 ,[],[],[],[],[],[],[],options);