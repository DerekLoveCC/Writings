>微信公众号：**[Fintech技术汇](#jump_fintech)**
作者为软件开发工程师，就职于金融信息科技类公司，通过CFA一级，分享计算机和金融相结合领域的技术和知识。

## 股票指数
之前一段时间忙于各种事情，就把公号暂时放下了一段时间，新的一年开始了，将会继续写一些和股票，计算机技术相关的文章。股票技术目前主要取材于CFA及中国A股，计算机技术涉猎的会广一些，单从编程语言的角度来说，主要会涉及到Python，C++，C#和Java。

本篇文章主要介绍股票指数相关的概念，主要内容如下：
### 指数的定义，历史和用途
股票指数是用来反映股票市场中各类型股票的整体水平及其变动情况的指标，简单的说，指数就是衡量一个国家，地区，或者行业的整体情况的指标。它其实就是一个资产的组合，资产组合中的股票作为一个整体反映出某市场的整体资产价格走势。

最早的股票指数可以追溯到1884年，是由道·琼斯公司的创始人查理斯·道所编制并发表在其自己编辑出版的《每日通讯》上，在此之后，股票指数发生了许多变化并衍生出各种计算方式，本文将会给出指数的几种计算方法。

股票指数的用途主要在以下5个方面：

1. 反映市场情绪或者投资者的信心：这一点容易理解，也是股指的最初目的
2. 用于衡量和建模收益率，系统风险和调整风险后的业绩：例如CAPM模型$K_e=R_f+\beta(R_m-R_f)$中的$R_m$就可以用类似S&P500, TOPIX的指数收益率替代。
3. 作为资产分配模型中资产类别的代理：股指能够反映所选择的代表性股票组合的风险和收益，它们提供了用于建模不同资产类别收益率和回报的历史数据。
4. 作为衡量基金经理业绩的基准：主动型基金经理的收益至少要高于股指，也就是通常所说的跑赢大盘，这也是基金经理的基本目标。
5. 为新的投资产品建模组合：例如第一个ETF就是基于已经存在的指数创建的
### 指数的构造与管理
构造，管理一个股指与构造，管理一个股票投资组合类似，需要解决以下五方面的问题：

1. 指数想要衡量的目标市场是什么? 比如你想要衡量某个地区还是某个行业等，选定了目标市场，可以作为指数成分股的股票也就定了。
2. 应该选择目标市场中的哪些股票作为成分股？成分股的数目可以是固定的，如S&P500,沪深330等；也可以是可变的如TOPIX。对于选择的标准，大部分指数是使用相对客观的规则，但也有些指数使用相对主观的方法，例如TOPIX虽然所选择的股票数量不固定，但是一只股票想要纳入TOPIX（即进入第一区First Section)必须满足特定的条件（市值，股东数，流通股数等），然而S&P500和The Sensex of Bombay指数有一个选择委员会来决定哪些股票作为成分股，所以就相对主观些。
3. 对于选定的成分股，应该分配多大的权重？权重决定了该成分股有多少会被纳入股指，对股指的价值有很大影响。有很多决定权重的方法：价格加权，相同权重，市值加权，基本面加权。每种加权方法都有其优缺点。
4. 什么时候对股指做再平衡？通常会定期调整成分股的权重
5. 什么时候重审股指的成分股？对包含的成分股进行调整，即是否移除某些或者加入新的成分股

以上5条中的前三条是关于怎样构造指数的，后面两条是关于指数管理的。
### 股票指数的相关计算
前面讲到成分股权重的设计方法有价格加权，相同权重，市值加权等，下面分别举例说明是如何计算。

假设有一个指数包含如下的三只股票：
|股票名字|流通股份数|2020-01-17时刻股价|2020-01-23时刻股价|
|----|----:|----:|----:|
|鲁抗医药|852,000,000|7.77|11.39|
|通策医疗|321,000,000|106.6|102.5|
|正海生物|43,000,000|81.11|77.22|
 1. 价格加权: 初始时把成分股的价格相加再除以股票数量
```
 在2020-01-17日的Index Value=(7.77+106.6+81.11)/3=65.16
 在2020-01-23日的Index Value=(11.39+102.5+77.22)/3=63.7
 股指涨幅为：(63.7-65.16)/65.16=-0.0224=-2.24%

 假设通策医疗在2020-02-03会进行一股变两股的拆股，其股价就会变为102.5/2=61.2，
 此时我们就需要进行再平衡操作，计算方法是：(11.39+61.2+77.22)/n=63.7，
 从而得到n=2.35。以后再计算该指数时就要使用新的2.35而非最初的3。
```

 2. 相同权重
```
 假设2020-01-17日为基准日，设计指数里所有股票价值之和为30,000且股指为1000，
 由于采用相同权重，所以每只股票分得10,000。
 从而鲁抗医药有10,000/7.77=1287股纳入指数，
 通策医疗有10,000/106.6=94股纳入指数，
 正海生物有10,000/81.11=123股纳入指数。
 除数因子为：30000/1000=30。
 在2020-01-23日，指数就为:(1287*11.39+94*102.5+123*77.22)/30=1126.4
 股指涨幅为：(1126.4-1000)/1000=12.64%
```
 3. 市值加权
```
  假设2020-01-17日为基准日，股指为1000，此刻的成分股总市值为：852,000,000*7.77+321,000,000*106.6+43,000,000*81.11=44,326,370,000。
  所以除数因子为：44,326,370,000/1000=44,326,370。
  在2020-01-23日，成分股总市值为：852,000,000*11.39+321,000,000*102.5+43,000,000*77.22=45,927,240,000。
  所以此刻股票指数为：45,927,240,000/44,326,370=1036.12。
  涨幅为：(1036.12-1000)/1000=3.61%
```
通过上述的例子可以看出，不同的指数计算方法会得出不同的结果
### 指数的类别
1. 主板市场指数(Broad Market Index)：顾名思义，该指数通常包含市场上90%以上的股票市值，能够很好的衡量市场的整体趋势
2. 多市场指数(Multi-Market Index)：通过容纳不同国家或区域的股票来构建指数，从而能够反映多个市场的情况。通常会根据地理位置或经济发展水平来分组，比如拉丁美洲股票指数，发展中国家股票指数等等。
3. 行业/区域指数(Sector Index)：以不同的行业或者区域作为指数衡量目标市场。比如房地产指数，银行业指数，医疗行业指数。
4. 投资风格指数：通过划分不同的投资风格来区分指数市场，如成长型，价值型，小盘股，大盘股等。

### 总结
本文简单介绍了一些股票指数相关的知识，算是对基础知识的一个积累。

<a id="jump_fintech"></a>
![Fintech技术汇](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Fintech.jpg)