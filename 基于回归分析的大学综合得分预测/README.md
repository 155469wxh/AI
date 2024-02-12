# 基于回归分析的大学综合得分预测

**项目数据集链接：** [数据集下载链接](https://pan.baidu.com/s/1AgHi10abQqI689pMeRGxbQ?pwd=DTAI)

项目中将使用Kaggle的数据集，利用线性回归模型，依据大学各项排名的指标来预测其综合得分。可以使用sk-learn等第三方库，不要求自己实现线性回归。

## 基础任务（80分）：

1. 观察和可视化数据，揭示数据的特性。
2. 训练集和测试集应按照7:3的比例随机划分，采用RMSE（均方根误差）作为模型的评估标准，计算并获取测试集上的线性回归模型的RMSE值。
3. 对线性回归模型中的系数进行分析。
4. 尝试使用其他类型的回归模型，并比较其效果。

## 进阶任务（20分）：

1. 尝试将地区的离散特征融入到线性回归模型中，然后比较并分析结果。
2. 利用R2指标和VIF指标进行模型评价和特征筛选，尝试是否可以增加模型精度。

## Tips：

- 需要查看数据，对数据做简单的清洗
- 选取的重要特征为：'quality_of_faculty', 'publications', 'citations', 'alumni_employment', 'influence', 'quality_of_education', 'broad_impact', 'patents'
- 标签为：'score'