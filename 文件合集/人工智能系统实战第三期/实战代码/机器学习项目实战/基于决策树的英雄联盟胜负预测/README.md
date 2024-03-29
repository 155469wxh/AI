# 基于决策树算法的英雄联盟游戏胜负预测

**项目数据集链接：** [数据集下载链接](https://pan.baidu.com/s/1AgHi10abQqI689pMeRGxbQ?pwd=DTAI)

**数据集说明：** 本数据集来自Kaggle，包含了9879场钻一到大师段位的单双排对局，对局双方几乎是同一水平。每条数据是前10分钟的对局情况，每支队伍有19个特征，红蓝双方共38个特征。这些特征包括英雄击杀、死亡，金钱、经验、等级情况等等。一局游戏一般会持续30至40分钟，但是实际前10分钟的局面很大程度上影响了之后胜负的走向。

## 基本任务（80）：
1. 合理的进行特征工程处理。
2. 划分训练集和测试集。
3. 使用决策树算法完成游戏胜负的预测。
4. 对比不同特征组合对模型效果的影响。
5. 提交代码和实验报告。

## 扩展任务（20）：
1. 尝试自行实现决策树算法细节。
2. 决策树算法的调参。

## Tips：

- 可以舍去无用特征以及构建差值特征。