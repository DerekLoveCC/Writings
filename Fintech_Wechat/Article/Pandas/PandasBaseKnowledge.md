#Pandas基础知识
##简介
>Pandas是一个用Python语言编写的高性能，易于使用的数据分析库，它还能和其他库紧密结合使用，如Numpy(Pandas构建在Numpy之上因此Pandas中也有很多Numpy的功能)，SciPy(Pandas可以为它提供清理好的数据集)，Matplotlib（Pandas使用它进行绘图），Scikit-Learn（Pandas为此AI框架提供数据）， 因此它是我们在做数据分析时经常使用的一个Python库。其官方网址为：https://pandas.pydata.org/ GitHub地址是：https://github.com/pandas-dev/pandas 。

>在这篇文章中， 我们主要从加载数据，清理和分析数据，保存结果三个方面进行介绍。注：本文中使用的数据是[A股数据分析之数据收集](https://mp.weixin.qq.com/s/noDq-vlrGXrrJcJwqDimWw)一文中收集的股票列表数据。
###安装Pandas
>安装Pandas非常容易，使用以下命令中的一个即可：
```Shell
pip install pandas #通过pip安装
conda install pandas #通过anaconda安装，需要先安装anaconda
```
##加载数据
>Pandas有两个最基本的数据结构DataFrame和Series，其中DataFrame表示一个二维表，Series表示一列。我们主要用这两个对象来操作数据。Pandas直接支持通过以下几种方式加载数据：
1. Dictionary List
2. CSV
3. Excel
4. Json
5. Database
>下面通过例子一一说明：
```Python
import pandas as pd
#Create DataFrame from dictionary list
dataList = [
            {'StockCode':'000001', 'Symbol':'平安银行'},
            {'StockCode':'000002', 'Symbol':'万  科Ａ'},
            {'StockCode':'000004', 'Symbol':'国农科技'}
           ]

df = pd.DataFrame(dataList)
```
>首先声明了一个列表dataList，然后通过pd.DataFrame创建了一个对象：df，下面我们看看df的类型和其中存放的数据
```Python
type(df)
```
```Python
df
```
>代码运行结果如下：
![从dic list构造DataFrame]()
```Python
#Create DataFrame from csv file
csv_df = pd.read_csv('../data/shanghai_stocks.csv', sep='\t', encoding='gb2312')
csv_df
```
>上面的代码从shanghai_stocks.csv文件中读取股票列表到DataFrame对象csv_df中，其中sep参数指出了CSV文件的分隔符，而encoding参数指定了编码方式，运行结果如下图：
![从csv文件构造DataFrame]()
```Python
#Create DataFrame from excel file
excel_df = pd.read_excel('../data/shenzhen_stocks.xlsx', encoding='gb2312')
excel_df
```
>上面的代码从shenzhen_stocks.xlsx文件中读取股票列表到DataFrame对象excel_df中，encoding参数指名了编码方式，运行结果如下图：
![从excel文件构造DataFrame]()

```Python
#create DataFrame from json file
json_df = pd.read_json('../data/shanghai_stocks.json')
json_df
```
>上面的代码从shanghai_stocks.json文件中读取股票列表到DataFrame对象json_df中,运行结果如下图：
![从json文件构造DataFrame]()
```Python
#create DataFrame object from database
import pyodbc
connection_string = 'Driver={SQL Server};Server=LocalHost;Database=Mars;Trusted_Connection=yes;'
conn = pyodbc.connect(connection_string)
database_df = pd.read_sql('Select * FROM [Mars].[dbo].[AStock]', conn)
database_df
```
>上面的代码从SQL Server数据库读取股票列表到DataFrame对象database_df中,步骤如下：
1. 导入pyodbc
2. 使用连接串创建odbc连接对象conn
3. 使用sql语句和conn调用pd.read_sql函数，并读取数据到DataFrame对象database_df中，运行结果如下图：
![从数据库构造DataFrame]()
##清理和分析数据
>把数据加载到DataFrame之后，首先我们需要查看一下数据的整体情况，下面是一些常用的函数以及它们的说明
```Python
database_df.info()# 1.显示总共有多少行数据，以及行号的范围；2.显示总共有多少列，每一列的名字，类型，以及每一列有多少个非null的对象；4.内存使用情况
database_df.shape #显示表总共有多少行多少列
database_df.describe()#统计每一列的数据情况，如有多少非null值，多少唯一值，top值是什么，频率是多少
database_df.columns#查看列名
database_df.values#返回所有的值
database_df.value_counts()
```
>下图是运行结果：
![查看数据表总体情况]()


>查看数据：
```Python
database_df.head(10)#查看前10条数据，默认是前5条
database_df.tail(6)#查看末尾6条数据，默认是末尾5条
database_df['IPODate']#查看IPODate这一列的数据
database_df.loc[100]#查看第101行数据
database_df[database_df['Industry']=='J 金融业']#获取金融业的公司
database_df[database_df['AStockCode'].isin(['000002','000004'])]#选取股票代码是000002，000004的两只股票。
```
>其他操作：
```Python
database_df.rename({'IPODate':'IpoDate'})#重命名IPODate列为IpoDate
database_df.isnull()
database_df.isnull().sum()
database_df.append()
database_df.drop_duplicates(inplace=true|false, select=first|last|False)
database_df.dropna()
database_df.fillna()
database_df.corr()
database_df.apply()
database_df.plot()
plt.rcParams.update({'figure.figsize':(10,8),'font-size':20})
```




##保存数据
##总结