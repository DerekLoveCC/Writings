
# 使用Python进行股票数据分析（一）
***
本译文主要介绍如何使用Python进行股票数据的分析，省略了原文简介部分。原文网址为：https://ntguardian.wordpress.com/2018/07/17/stock-data-analysis-python-v2/

## 前言
我将使用quandl和pandas_datareader两个工具包，Anaconda并没有安装这两个包。运行下面的两个命令来安装它们：
```
conda install quandl
conda install pandas-datareader
```
## 获取并可视化股票数据	
### 从quandl获取数据
在分析股票数据之前，我们需要先获得数据并转化成可用的格式。可以从[雅虎财经](https://finance.yahoo.com/)，[谷歌财经](http://finance.google.com/)或许多其他的地方获得股票数据。目前，我推荐使用quandl， 它是社区维护的经融和经济数据提供商。（[雅虎财经](https://finance.yahoo.com/)曾经提供高质量的股票数据，但是从2017年起，其API便不再维护，也不再能从它获得可信的数据了，详情请参考(https://quant.stackexchange.com/questions/35019/is-yahoo-finance-data-good-or-bad-now)

默认情况下，quandl的get函数会返回一个包含所请求数据的Pandas DataFrame。

```Python
import pandas as pd
import quandl
import datetime
 
# We will look at stock prices over the past year, starting at January 1, 2016
start = datetime.datetime(2016,1,1)
end = datetime.date.today()
 
# Let's get Apple stock data; Apple's ticker symbol is AAPL
# First argument is the series we want, second is the source ("yahoo" for Yahoo! Finance), third is the start date, fourth is the end date
s = "AAPL"
apple = quandl.get("WIKI/" + s, start_date=start, end_date=end)
 
type(apple)
apple.head()
``` 
（译者注：上述程序在Jupyter Notebook中的运行情况如下图）
![图1](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/%E5%9B%BE1.png)
我们简单的讨论一下，Open是交易日开始时的股票价格（不需要是上一个交易日的收盘价），High是交易日中的最高股价，Low是交易日中的最低股价，Close是交易日结束时的股价。Volume指交易了多少股票。Adjusted Prices（例如调整后的收盘价）是因为公司行为而调整后的股价。虽然认为股价主要由交易员设定，但股票拆分（公司使每只现存股票分成两个并使价格减半）和分红（支付公司的每股利润）也会影响股价，也需要加以考虑。
### 可视化股票数据
现在我们获得了想要可视化的股票数据了。首先，我将演示怎么使用matplotlib包来实现。注意到apple DataFrame对象有一个方便的方法，plot()，使得绘图更加方便。
```Python
import matplotlib.pyplot as plt   # Import matplotlib
# This line is necessary for the plot to appear in a Jupyter notebook
%matplotlib inline
# Control the default size of figures in this Jupyter notebook
%pylab inline
pylab.rcParams['figure.figsize'] = (15, 9)   # Change the size of plots
 
apple["Adj. Close"].plot(grid = True) # Plot the adjusted closing price of AAPL
```
运行结果如下:
![图2](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/%E5%9B%BE2.png)
一个折线图是可以的，但是每天至少涉及4个变量（开盘价，收盘价，最高价，最低价），我们希望有某种可视化方法能够查看4个变量而不需要绘制4个单独的折线。金融数据通常绘制成日本蜡烛图，之所以这么叫是因为它是18世纪的日本大米交易者创建的。虽然需要一些努力，但是这种图可以用matplotlib创建。

我创建了一个函数并使用它来绘制我们的股票数据，也欢迎读者使用它从pandas DataFrame中更方便地绘制蜡烛图。（代码是基于这个[例子](http://matplotlib.org/examples/pylab_examples/finance_demo.html) ， 你也可以在[这里](http://matplotlib.org/api/finance_api.html)查看相关函数的文档）
译者注：mpl_finance包已经从matplotlib移除，所以需要单独安装，请到<https://github.com/matplotlib/mpl_finance>下载并在下载目录下运行python setup.py install 命令来安装
```Python
from matplotlib.dates import DateFormatter, WeekdayLocator, DayLocator, MONDAY
from mpl_finance import candlestick_ohlc
 
def pandas_candlestick_ohlc(dat, stick="day", adj=False, otherseries=None):
    """
    :param dat: pandas DataFrame object with datetime64 index, and float columns "Open", "High", "Low", and "Close", likely created via DataReader from "yahoo"
    :param stick: A string or number indicating the period of time covered by a single candlestick. Valid string inputs include "day", "week", "month", and "year", ("day" default), and any numeric input indicates the number of trading days included in a period
    :param adj: A boolean indicating whether to use adjusted prices
    :param otherseries: An iterable that will be coerced into a list, containing the columns of dat that hold other series to be plotted as lines
 
    This will show a Japanese candlestick plot for stock data stored in dat, also plotting other series if passed.
    """
    mondays = WeekdayLocator(MONDAY)        # major ticks on the mondays
    alldays = DayLocator()                  # minor ticks on the days
    dayFormatter = DateFormatter('%d')      # e.g., 12
 
    # Create a new DataFrame which includes OHLC data for each period specified
    # by stick input
    fields = ["Open", "High", "Low", "Close"]
    if adj:
        fields = ["Adj. " + s for s in fields]
    transdat = dat.loc[:,fields]
    transdat.columns = pd.Index(["Open", "High", "Low", "Close"])
    if (type(stick) == str):
        if stick == "day":
            plotdat = transdat
            stick = 1 # Used for plotting
        elif stick in ["week", "month", "year"]:
            if stick == "week":
                transdat["week"] = pd.to_datetime(transdat.index).map(lambda x: x.isocalendar()[1]) # Identify weeks
            elif stick == "month":
                transdat["month"] = pd.to_datetime(transdat.index).map(lambda x: x.month) # Identify months
            transdat["year"] = pd.to_datetime(transdat.index).map(lambda x: x.isocalendar()[0]) # Identify years
            grouped = transdat.groupby(list(set(["year",stick]))) # Group by year and other appropriate variable
            plotdat = pd.DataFrame({"Open": [], "High": [], "Low": [], "Close": []}) # Create empty data frame containing what will be plotted
            for name, group in grouped:
                plotdat = plotdat.append(pd.DataFrame({"Open": group.iloc[0,0],
                                            "High": max(group.High),
                                            "Low": min(group.Low),
                                            "Close": group.iloc[-1,3]},
                                           index = [group.index[0]]))
            if stick == "week": stick = 5
            elif stick == "month": stick = 30
            elif stick == "year": stick = 365
 
    elif (type(stick) == int and stick >= 1):
        transdat["stick"] = [np.floor(i / stick) for i in range(len(transdat.index))]
        grouped = transdat.groupby("stick")
        plotdat = pd.DataFrame({"Open": [], "High": [], "Low": [], "Close": []}) # Create empty data frame containing what will be plotted
        for name, group in grouped:
            plotdat = plotdat.append(pd.DataFrame({"Open": group.iloc[0,0],
                                        "High": max(group.High),
                                        "Low": min(group.Low),
                                        "Close": group.iloc[-1,3]},
                                       index = [group.index[0]]))
 
    else:
        raise ValueError('Valid inputs to argument "stick" include the strings "day", "week", "month", "year", or a positive integer')
 
 
    # Set plot parameters, including the axis object ax used for plotting
    fig, ax = plt.subplots()
    fig.subplots_adjust(bottom=0.2)
    if plotdat.index[-1] - plotdat.index[0] < pd.Timedelta('730 days'):
        weekFormatter = DateFormatter('%b %d')  # e.g., Jan 12
        ax.xaxis.set_major_locator(mondays)
        ax.xaxis.set_minor_locator(alldays)
    else:
        weekFormatter = DateFormatter('%b %d, %Y')
    ax.xaxis.set_major_formatter(weekFormatter)
 
    ax.grid(True)
 
    # Create the candelstick chart
    candlestick_ohlc(ax, list(zip(list(date2num(plotdat.index.tolist())), plotdat["Open"].tolist(), plotdat["High"].tolist(),
                      plotdat["Low"].tolist(), plotdat["Close"].tolist())),
                      colorup = "black", colordown = "red", width = stick * .4)
 
    # Plot other series (such as moving averages) as lines
    if otherseries != None:
        if type(otherseries) != list:
            otherseries = [otherseries]
        dat.loc[:,otherseries].plot(ax = ax, lw = 1.3, grid = True)
 
    ax.xaxis_date()
    ax.autoscale_view()
    plt.setp(plt.gca().get_xticklabels(), rotation=45, horizontalalignment='right')
 
    plt.show()
 
pandas_candlestick_ohlc(apple, adj=True, stick="month")
```
![图3](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu3.png)
在蜡烛图中，黑色的蜡烛图标识这天的收盘价高于开盘价（盈利），而红色蜡烛表示这天的开盘价高于收盘价（亏损）。上影线和下影线表示最高价和最低价，主体表示开盘价和收盘价（颜色决定哪端是开盘价，哪端是收盘价）。蜡烛图在金融领域很流行而且[技术分析](https://en.wikipedia.org/wiki/Technical_analysis)中的一些策略根据蜡烛的形状，颜色，位置来做交易决定。今天我不会讲解这些策略。

我们可能想要把多个金融工具绘制到一起；我们可能想要比较股票，把它们和市场比较，或者查看其他证券例如[ETFs](https://en.wikipedia.org/wiki/Technical_analysis)。后面我们也会查看怎样针对某个指标，例如移动平均线（moving average）去绘制一个金融工具。对于这种，你应该使用折线图而非蜡烛图。（你怎么才能绘制多个蜡烛图，一个在另一个上面而不弄乱图表?）

下面，我获取了其他几个技术公司的股票数据并且把他们调整后的收盘价绘制在了一起。

```Python
microsoft, google = (quandl.get("WIKI/" + s, start_date=start, end_date=end) for s in ["MSFT", "GOOG"])
 
# Below I create a DataFrame consisting of the adjusted closing price of these stocks, first by making a list of these objects and using the join method
stocks = pd.DataFrame({"AAPL": apple["Adj. Close"],
                      "MSFT": microsoft["Adj. Close"],
                      "GOOG": google["Adj. Close"]})
 
stocks.head()
```
![图4](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu4.png)

```Python
stocks.plot(grid = True)
```
![图5](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu5.png)
这幅图有什么问题呢？虽然绝对价格很重要（高价股很难购买，这不仅影响它们的波动性而且影响你交易股票的能力），但交易时，我们更关注资产的相对变化而不是它的绝对价格。谷歌的股票比微软或苹果的股票贵得多，而这种差异使得苹果和微软股票的波动性远低于它们实际的波动性（也就是说，他们的价格看着没有太多偏差）。

一种解决方案是在绘图时使用两种不同的比例；一种比例用于苹果和微软，另一种用于谷歌。
```Python
stocks.plot(secondary_y = ["AAPL", "MSFT"], grid = True)
```
![图6](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu6.png)
然而，更好的方案是绘制我们正在想要的信息：股票的收益。这涉及到把数据转换成某种对我们的目标更有用的东西。有多种转换可供我们使用。

一种转换是考虑自利息周期开始以来股票的收益。也就是说，我们用如下公式绘图：
![图7](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu7.png)
这就要求转换stocks对象中的数据，我接下来就会这样做。请注意，我使用了lamda函数，它允许我把一个定义好的小函数当做参数快速传递到另一个函数或者方法中（你可以从这里阅读更多关于lamda函数的信息）。

```python
# df.apply(arg) will apply the function arg to each column in df, and return a DataFrame with the result
# Recall that lambda x is an anonymous function accepting parameter x; in this case, x will be a pandas Series object
stock_return = stocks.apply(lambda x: x / x[0])
stock_return.head() - 1
```
![图8](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu8.png)

```python
stock_return.plot(grid = True).axhline(y = 1, color = "black", lw = 2)
```

![图9](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu9.png)
这个图表更加有用。我们现在能够看到自此周期开始算起每只股票的盈利情况。此外，我们看到这些股票高度相关；他们通常朝同一方向移动，这一事实很难在其他图表中看到。

或者，我们可以绘制每只股票每天的变化情况。一种方法是在比较第t天和第t + 1天时，使用下面的公式绘制一只股票的增长率：
![图10](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu10.png)
但，变化可能以以下不同的方式理解：
![图11](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu11.png)
这些公式不同并可能导致不同的结论，但是有另一种方法来模拟一只股票的增长：使用对数差异。
$\Large Change_{t} = log(Price_{t}) -  log(Price_{t-1})$

（这里log是自然对数，并且我们的定义不强依赖于是使用$\small log(Price_{t}) -  log(Price_{t-1})$还是$\small log(Price_{t+1}) -  log(Price_{t})$）使用对数差的优势是这种差可以被解释为股票的百分比变化，但不依赖于分数的分母。此外，对数差具有理想的性质：对数差的总和可以解释为汇总时间段的总变化（以百分比）（其他公式没有这种性质；他们会高估增长）。对数差也更加清晰地对应于股票价格在连续时间内的建模方式。
我们可以如下获得和绘制stocks中数据的对数差：
```python
import numpy as np
stock_change = stocks.apply(lambda x: np.log(x) - np.log(x.shift(1))) # shift moves dates back by 1.
stock_change.head()
```
![图13](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu13.png)
```python
stock_change.plot(grid = True).axhline(y = 0, color = "black", lw = 2)
```
![图14](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu14.png)
你更喜欢哪种转换？看起来，自周期开始计算收益的方式使得问题中股票的整体趋势更加明显。但是，每天之间的变化是在对股票行为进行建模时实际考虑的更高级方法。所以他们不应该被忽视。我们经常想将股票的表现与整体市场的表现进行比较。[SPY](https://finance.yahoo.com/quote/SPY/)，是SPDR标准普尔500指数交易型开放式指数基金（ETF）的股票代码，是一只试图模仿标准普尔500股票指数构成的基金，因而代表了“市场”中的价值。

SPY数据不能从quandl免费获取，所以我将从雅虎财经中获取数据。（我没有选择。）

下面是我获得SPY数据并将它的表现跟我们的股票表现相比较。

```python
#import pandas_datareader.data as web    # Going to get SPY from Yahoo! (I know I said you shouldn't but I didn't have a choice)
#spyder = web.DataReader("SPY", "yahoo", start, end)    # Didn't work
#spyder = web.DataReader("SPY", "google", start, end)    # Didn't work either
# If all else fails, read from a file, obtained from here: http://www.nasdaq.com/symbol/spy/historical
spyderdat = pd.read_csv("/home/curtis/Downloads/HistoricalQuotes.csv")    # Obviously specific to my system; set to
                                                                          # location on your machine
spyderdat = pd.DataFrame(spyderdat.loc[:, ["open", "high", "low", "close", "close"]].iloc[1:].as_matrix(),
                         index=pd.DatetimeIndex(spyderdat.iloc[1:, 0]),
                         columns=["Open", "High", "Low", "Close", "Adj Close"]).sort_index()
 
spyder = spyderdat.loc[start:end]
 
stocks = stocks.join(spyder.loc[:, "Adj Close"]).rename(columns={"Adj Close": "SPY"})
stocks.head()
```
![图15](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu15.png)
```python
stock_return = stocks.apply(lambda x: x / x[0])
stock_return.plot(grid = True).axhline(y = 1, color = "black", lw = 2)
```
![图16](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu16.png)
```python
stock_change = stocks.apply(lambda x: np.log(x) - np.log(x.shift(1)))
stock_change.plot(grid=True).axhline(y = 0, color = "black", lw = 2)
```
![图17](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu17.png)

##经典风险指标
我们已经可以为我们的股票从目前掌握的数据中计算信息指标了，这些指标可以被视为某种风险指标。

首先，我们想把盈利年化，从而计算年度百分比(APR)，这有助于我们把盈利保持在相同的时间范围内。
```python
stock_change_apr = stock_change * 252 * 100    # There are 252 trading days in a year; the 100 converts to percentages
stock_change_apr.tail()
```
![图18](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu18.png)

这里有些数据初看起来没有道理，但目前还没有问题。

我想要的指标如下：
* 平均回报
* 波动率（回报的标准差）
* $\large \alpha$和$\large \beta$
* 夏普比率

前两个指标从字面意思就可以理解，后面两个需要解释一下。

首先，无风险利率是无风险金融资产的回报率，我用$r_{\tiny RF}$表示。这种资产只是理论上存在，但通常低风险投资工具例如3月期的美国国债的收益率可以被认为几乎是无风险的，因而其收益率可以被用来估算无风险利率。我们如下获得这些工具的数据。
```python
tbill = quandl.get("FRED/TB3MS", start_date=start, end_date=end)
tbill.tail()
```
![图19](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu19.png)

```python
tbill.plot()
```
![图20](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu20.png)
```python
rrf = tbill.iloc[-1, 0]    # Get the most recent Treasury Bill rate
rrf
```
![图21](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu21png.png)
现在，线性回归模型是以下形式的模型：
$y_{\tiny i}=\alpha+\beta x_{\tiny i} +\epsilon_{\tiny i}$
$\epsilon_{\tiny i}$是一个错误处理。思考这个模型的另一种方式是：
$\hat{y_{\tiny i}}=\alpha+\beta x_{\tiny i}$
$\hat{y_{\tiny i}}$是给定$x_{\tiny i}$时$y_{\tiny i}$的预测值。换句话说，线性回归告诉你$x_{\tiny i}$和$y_{\tiny i}$是如何相关的，以及$x_{\tiny i}$的值如何被用于估测$y_{\tiny i}$的值的。$\alpha$是模型的截距，$\beta$是斜率。
特别指出，如果$x$为零，则$\alpha$就是$y$的预测值，而$\beta$表示$x$每改变一个单位时$y$变化多少。在给定样本均值$\bar{x}$和$\bar{y}$，样本标准差$s_{\tiny x}$和$s_{\tiny y}$以及$x$和$y$的相关系数$r$的情况下，有一种计算$\alpha$和$\beta$的简单方法：
$\beta = r\frac{s_y}{s_x}$
$\alpha = \bar{y}-\beta\bar{x}$
在金融中，我们如下使用$\alpha$和$\beta$：
$R_t - r_{RF} = \alpha + \beta(R_{Mt} - r_{RF})+\epsilon_t$
$R_t$是某个金融资产（如股票）的回报率，$R_t-r_{RF}$表示多余回报，或者是超出无风险回报率的回报。$R_{Mt}$是时间$t$的市场回报。而$\alpha$和$\beta$可以如下解释：
* $\alpha$是超出市场的平均超额收益
* $\beta$是股票相对于市场的走势。如果$\beta$>0，则股票通常和市场走势相同，当|$\beta$|>1时股票对市场反应强烈而当|$\beta$|<1时股票对市场反应较弱。

下面我获取了一个pandas序列，其中包含了每只股票与SPY（我们的市场估值）的相关程度。
```python
smcorr = stock_change_apr.drop("SPY", 1).corrwith(stock_change_apr.SPY)# Since RRF is constant it doesn't change the correlation so we can ignore it in our calculation
smcorr
```
>AAPL    0.593097
MSFT    0.720059
GOOG    0.674322
dtype: float64

然后我计算了$\alpha$和$\beta$。
```python
sy = stock_change_apr.drop("SPY", 1).std()
sx = stock_change_apr.SPY.std()
sy
```
>AAPL    339.921782
MSFT    329.308164
GOOG    312.319468
dtype: float64
```python
sx    # Standard deviation for x
```
>188.08029321745738
```python
ybar = stock_change_apr.drop("SPY", 1).mean() - rrf
xbar = stock_change_apr.SPY.mean() - rrf
ybar
```
>AAPL    19.319035
MSFT    21.912806
GOOG    11.316893
dtype: float64

```python
xbar
```
>9.331380746251185

```python
beta = smcorr * sy / sx
alpha = ybar - beta * xbar
beta
```
>AAPL    1.071918
MSFT    1.260744
GOOG    1.119756
dtype: float64
```python
alpha
```
>AAPL     9.316557
MSFT    10.148320
GOOG     0.868025
dtype: float64

夏普比率是另一个著名的风险指标，定义如下：
$Sharpe~ratio=\frac{\bar{R_t}-r_{RF}}{s}$
这里的$s$是股票的波动性。我们想要大的夏普比率。夏普比率大表示股票的超额收益相对于股票的波动性大。此外，夏普比率和统计检验（$t$检验）相结合以决定一只股票的平均收益是否大于无风险利率；比值越大，股票的平均收益越可能大于无风险利率。

你现在面临的挑战是为这里列出的每只股票计算夏普比率，并对其进行解释。根据夏普比率，哪只股票更利于投资呢？
```python
sharpe = (ybar - rrf)/sy
sharpe
```
>AAPL    0.049920
MSFT    0.059406
GOOG    0.028711
dtype: float64
```python
(xbar - rrf)/sx
```
>0.03711915069262122

###移动平均线
图表非常有用。事实上，一些交易员的策略几乎完全基于图表（这些是“技术分析师”，因为基于从图表中找到的模式的交易策略是被称为技术分析交易学说的一部分）。现在让我们考虑怎样找到股票趋势。
对于一系列的$x_t$和时间点$t$，$q$天的移动平均值是过去$q$天的平均值：即，如果$MA_t^q$表示移动平均值，那么：
$\Large MA^q_t = \frac{1}{q} \sum_{i = 0}^{q-1}x_{t-i}$
移动均线平滑了序列并帮助识别趋势。$q$值越大，反应越慢。移动平均过程是一系列$x_t$的短期波动。这种思想是移动平均值处理有助于从“噪音”中辨别出趋势。快速移动平均线具有较小的$q$并且更紧密地跟随股票，而慢移动平均线具有更大的$q$，导致它们对股票的波动响应较慢且更稳定。
pandas提供了轻松计算移动均值的功能。我通过为苹果创建一个20天（一个月）的移动均值来演示它的使用，并将其和股票绘制在一起。
```python
apple["20d"] = np.round(apple["Adj. Close"].rolling(window = 20, center = False).mean(), 2)
pandas_candlestick_ohlc(apple.loc['2016-01-04':'2016-12-31',:], otherseries = "20d", adj=True)
```
![图22](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu22.png)

请注意移动平均值开始得有多晚。直到20天后才能计算它。对于长期移动平均线，这种限制变得更加严重。因为我希望能够计算200天移动平均线，所以我将扩展我们拥有的苹果股票的数据量。也就是说，我们仍将主要关注2016年。
```python
start = datetime.datetime(2010,1,1)
apple = quandl.get("WIKI/AAPL", start_date=start, end_date=end)
apple["20d"] = np.round(apple["Adj. Close"].rolling(window = 20, center = False).mean(), 2)
 
pandas_candlestick_ohlc(apple.loc['2016-01-04':'2016-12-31',:], otherseries = "20d", adj=True)
```
![图23](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/tu23.png)

您会注意到移动平均线比实际股票数据更平滑。此外，这是一个惰性的指标;股价需要高于或低于移动平均线，均线才会改变方向。因此，股价越过移动平均线表示趋势可能会变化，需要应引起注意。交易员通常对多个移动平均线感兴趣，例如20天，50天和200天移动平均线。很容易一次检查多个移动平均线。
```python
apple["50d"] = np.round(apple["Adj. Close"].rolling(window = 50, center = False).mean(), 2)
apple["200d"] = np.round(apple["Adj. Close"].rolling(window = 200, center = False).mean(), 2)
 
pandas_candlestick_ohlc(apple.loc['2016-01-04':'2016-12-31',:], otherseries = ["20d", "50d", "200d"], adj=True)
```
![图24](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/24.png)

20天的移动均线对本地改变最敏感，200天的移动均线最不敏感。在这里，200日均线显示整体熊市：股价随着时间的推移呈下降趋势。20日均线有时看跌，其他时候看涨，预计会有积极的变化。您还可以看到移动平均线的交叉表示趋势的改变。这些交叉点可以被我们用作交易信号，或表明金融证券正在改变方向，并且可能会实现有利可图的交易。

##交易策略
我们现在关注的是设计和评估交易策略。
任何交易者都必须有一套规则来确定她愿意往某只股票上投入多少钱。例如，交易者可以决定在任何情况下她都不会在交易中冒超出投资组合10％以上的风险。此外，在任何交易中，交易者必须有退出策略，即一系列条件决定她何时退出头寸，无论是盈利还是亏损。交易者可以设定目标，这是促使交易者离开头寸的最小利润。同样，交易者可能有一个她愿意承受的最大损失；如果潜在损失会超过此金额，交易者将退出该头寸以防止任何进一步的损失。我们假设在任何特定的交易中所涉及的投资组合金额是固定比例的; 10％似乎是一个很好的数字。在这里，我将演示[移动平均线交叉](http://www.investopedia.com/university/movingaverage/movingaverages4.asp)策略。我们将使用两个移动平均线，一个“快”，另一个“慢”。策略如下：
* 当快速移动均线交叉慢速移动均线时进行交易
* 当快速移动均线再次交叉慢速移动均线时退出交易

当快速移动平均线从下往上穿过慢速移动平均线时，提示进行交易，当后来快速移动平均线向下穿过慢速移动平均线时，退出交易。

我们现在有一个完整的战略。但在我们决定使用它之前，应该首先尝试评估策略的效果。这样做的通常方法是回溯测试，即测试策略对历史数据的盈利能力。例如，查看上图中Apple股票的表现，如果把20天移动平均线作为快速移动均线而50天移动平均线作为慢速移动均线，那么这种策略似乎并不是非常有利可图，至少不是如果你总是采取多头头寸。让我们看看能否自动化回溯测试。我们首先识别出20日均线何时低于50日均线，及何时高于50日均线。
```python
apple['20d-50d'] = apple['20d'] - apple['50d']
apple.tail()
```
![图25](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/25.png)

我们将把这种差异的标志称为regime，也就是说，如果快速移动平均线高于慢移动平均线，那么这是一个看涨regime（多头规则），当快速移动平均线低于缓慢移动平均线时，看跌regime（空头规则）成立。我使用以下代码识别权。
```python
# np.where() is a vectorized if-else function, where a condition is checked for each component of a vector, and the first argument passed is used when the condition holds, and the other passed if it does not
apple["Regime"] = np.where(apple['20d-50d'] > 0, 1, 0)
# We have 1's for bullish regimes and 0's for everything else. Below I replace bearish regimes's values with -1, and to maintain the rest of the vector, the second argument is apple["Regime"]
apple["Regime"] = np.where(apple['20d-50d'] < 0, -1, apple["Regime"])
apple.loc['2016-01-04':'2016-12-31',"Regime"].plot(ylim = (-2,2)).axhline(y = 0, color = "black", lw = 2)
```
![图26](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/26.png)
```python
apple["Regime"].plot(ylim = (-2,2)).axhline(y = 0, color = "black", lw = 2)
```
![图27](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/27.png)
```python
apple["Regime"].value_counts()
```
>1    1323
-1     694
 0      53
Name: Regime, dtype: int64

上面的最后一行表明1005天市场对苹果利空，而600天市场看涨，54天是中性。
交易信号出现在regime改变时。当看涨regime开始时，触发买入信号，当它结束时，触发卖出信号。同样，当看跌regime开始时，触发卖出信号，当它结束时触发买入信号（只有当你对股票做空，或者使用像股票期权这样的衍生品对赌市场时，这才有意义。）

很容易获得这种信号。用$r_t$表示t时刻的regime,并用$s_t$表示t时刻的信号，那么：
$s_t=sign(r_t-r_{t-1})$
$s_t\in\{-1,0,1\}$ -1表示“卖出”，1表示“买入”，0表示无操作。我们可以获得如下信号：
```python
# To ensure that all trades close out, I temporarily change the regime of the last row to 0
regime_orig = apple.loc[:, "Regime"].iloc[-1]
apple.loc[:, "Regime"].iloc[-1] = 0
apple["Signal"] = np.sign(apple["Regime"] - apple["Regime"].shift(1))
# Restore original regime data
apple.loc[:, "Regime"].iloc[-1] = regime_orig
apple.tail()
```
![图28]()
```python
apple["Signal"].plot(ylim = (-2, 2))
```
![图29]()
```python
apple["Signal"].value_counts()
```
>0.0    2014
-1.0     28
1.0      27
Name: Signal, dtype: int64

我们将买入苹果股票23次，并卖出23次。如果我们只对苹果股票做多，那么在这6年期间只会进行23笔交易，相应的，如果我们在每次多头头寸结束时从多头交易转到空头头寸，我们总共将进行23笔交易。（请记住，更频繁的交易不一定是好的;交易从不是免费的。）

你可能会注意到目前的系统不是很强健，因为即使是快速移动平均线一瞬间高于缓慢移动平均线也会触发交易，结果是交易快速结束（这很糟糕，因为现实中，每笔交易都伴有交易费用，能很快地冲抵掉收益）。此外，每次牛市迅速转变为熊市时，如果您构建的交易系统允许牛熊对赌，这将导致一次交易的结束立即引发新的交易，即在相反的方向下注入市场，这似乎有点吹毛求疵。一个好的系统需要更多的证据表明市场正朝着某个方向上移动。但我们暂时不去考虑这些细节。

让我们试着识别出股票在每次卖出和买入时的价格。
```python
apple.loc[apple["Signal"] == 1, "Close"]
```
>Date
2010-03-16    224.450
2010-06-18    274.074
2010-08-16    247.640
2010-09-20    283.230
2011-05-12    346.570
2011-07-14    357.770
2011-12-28    402.640
2012-06-25    570.765
2013-05-17    433.260
2013-07-31    452.530
2013-10-16    501.114
2014-03-11    536.090
2014-03-12    536.610
2014-03-24    539.190
2014-04-25    571.940
2014-10-28    106.740
2015-02-05    119.940
2015-04-28    130.560
2015-10-27    114.550
2016-03-10    101.170
2016-06-23     96.100
2016-06-30     95.600
2016-07-25     97.340
2016-12-21    117.060
2017-08-02    157.140
2017-11-01    166.890
2018-03-08    176.940
Name: Close, dtype: float64

```python
apple.loc[apple["Signal"] == -1, "Close"]
```
>Date
2010-06-11    253.5100
2010-07-22    259.0240
2010-08-17    251.9700
2011-03-30    348.6300
2011-03-31    348.5075
2011-05-27    337.4100
2011-11-17    377.4100
2012-05-09    569.1800
2012-10-17    644.6136
2013-06-26    398.0700
2013-10-04    483.0300
2014-01-28    506.5000
2014-03-17    526.7400
2014-04-22    531.6990
2014-10-17     97.6700
2015-01-05    106.2500
2015-04-16    126.1700
2015-06-25    127.5000
2015-06-26    126.7500
2015-12-18    106.0300
2016-05-05     93.2400
2016-06-27     92.0400
2016-07-11     96.9800
2016-11-15    107.1100
2017-06-27    143.7400
2017-10-03    154.4800
2018-02-06    163.0300
2018-03-27    168.3400
Name: Close, dtype: float64

```python
# Create a DataFrame with trades, including the price at the trade and the regime under which the trade is made.
apple_signals = pd.concat([
        pd.DataFrame({"Price": apple.loc[apple["Signal"] == 1, "Adj. Close"],
                     "Regime": apple.loc[apple["Signal"] == 1, "Regime"],
                     "Signal": "Buy"}),
        pd.DataFrame({"Price": apple.loc[apple["Signal"] == -1, "Adj. Close"],
                     "Regime": apple.loc[apple["Signal"] == -1, "Regime"],
                     "Signal": "Sell"}),
    ])
apple_signals.sort_index(inplace = True)
apple_signals
```
| Date | Price | Regime | Signal |
|:-----------|:------------|:------------|:------------|
|2010-03-16	 |28.844953	    |1	|Buy
|2010-06-11	 |32.579568	    |-1	|Sell
|2010-06-18	 |35.222329	    |1	|Buy
|2010-07-22	 |33.288194	    |-1	|Sell
|2010-08-16	 |31.825192	    |0	|Buy
|2010-08-17	 |32.381657	    |-1	|Sell
|2010-09-20	 |36.399003	    |1	|Buy
|2011-03-30	 |44.803814	    |0	|Sell
|2011-03-31	 |44.788071	    |-1	|Sell
|2011-05-12	 |44.539075	    |1	|Buy
|2011-05-27	 |43.361888	    |-1	|Sell
|2011-07-14	 |45.978431	    |1	|Buy
|2011-11-17	 |48.502445	    |-1	|Sell
|2011-12-28	 |51.744852	    |1	|Buy
|2012-05-09	 |73.147563	    |-1	|Sell
|2012-06-25	 |73.351258	    |1	|Buy
|2012-10-17	 |83.195498	    |-1	|Sell
|2013-05-17	 |56.878472	    |1	|Buy
|2013-06-26	 |52.258721	    |-1	|Sell
|2013-07-31	 |59.408242	    |1	|Buy
|2013-10-04	 |63.831819	    |-1	|Sell
|2013-10-16	 |66.221597	    |1	|Buy
|2014-01-28	 |67.325247	    |-1	|Sell
|2014-03-11	 |71.682490	    |0	|Buy
|2014-03-12	 |71.752021	    |1	|Buy
|2014-03-17	 |70.432269	    |-1	|Sell
|2014-03-24	 |72.097002	    |1	|Buy
|2014-04-22	 |71.095354	    |-1	|Sell
|2014-04-25	 |76.476120	    |1	|Buy
|2014-10-17	 |92.387441	    |-1	|Sell
|2014-10-28	 |100.966883	|1	|Buy
|2015-01-05	 |100.937944	|-1	|Sell
|2015-02-05	 |114.390004	|1	|Buy
|2015-04-16	 |120.331722	|-1	|Sell
|2015-04-28	 |124.518583	|1	|Buy
|2015-06-25	 |122.104986	|0	|Sell
|2015-06-26	 |121.386721	|-1	|Sell
|2015-10-27	 |110.198438	|1	|Buy
|2015-12-18	 |102.440744	|-1	|Sell
|2016-03-10	 |98.271427	    |1	|Buy
|2016-05-05	 |91.122295	    |-1	|Sell
|2016-06-23	 |93.917337	    |1	|Buy
|2016-06-27	 |89.949550	    |-1	|Sell
|2016-06-30	 |93.428693	    |1	|Buy
|2016-07-11	 |94.777350	    |-1	|Sell
|2016-07-25	 |95.129174	    |1	|Buy
|2016-11-15	 |105.787035	|-1	|Sell
|2016-12-21	 |115.614138	|1	|Buy
|2017-06-27	 |143.159139	|-1	|Sell
|2017-08-02	 |156.504989	|1	|Buy
|2017-10-03	 |154.480000	|-1	|Sell
|2017-11-01	 |166.890000	|1	|Buy
|2018-02-06	 |163.030000	|-1	|Sell
|2018-03-08	 |176.940000	|1	|Buy
|2018-03-27	 |168.340000	|1	|Sell

```python
# Let's see the profitability of long trades
apple_long_profits = pd.DataFrame({
        "Price": apple_signals.loc[(apple_signals["Signal"] == "Buy") &
                                  apple_signals["Regime"] == 1, "Price"],
        "Profit": pd.Series(apple_signals["Price"] - apple_signals["Price"].shift(1)).loc[
            apple_signals.loc[(apple_signals["Signal"].shift(1) == "Buy") & (apple_signals["Regime"].shift(1) == 1)].index
        ].tolist(),
        "End Date": apple_signals["Price"].loc[
            apple_signals.loc[(apple_signals["Signal"].shift(1) == "Buy") & (apple_signals["Regime"].shift(1) == 1)].index
        ].index
    })
apple_long_profits
```
| Date | Price | Profit | End Date |
|:------------|:------------|:------------|:------------|
|2010-03-16	|28.844953	|3.734615	|2010-06-11
|2010-06-18	|35.222329	|-1.934135	|2010-07-22
|2010-09-20	|36.399003	|8.404812	|2011-03-30
|2011-05-12	|44.539075	|-1.177188	|2011-05-27
|2011-07-14	|45.978431	|2.524014	|2011-11-17
|2011-12-28	|51.744852	|21.402711	|2012-05-09
|2012-06-25	|73.351258	|9.844240	|2012-10-17
|2013-05-17	|56.878472	|-4.619751	|2013-06-26
|2013-07-31	|59.408242	|4.423577	|2013-10-04
|2013-10-16	|66.221597	|1.103650	|2014-01-28
|2014-03-12	|71.752021	|-1.319753	|2014-03-17
|2014-03-24	|72.097002	|-1.001648	|2014-04-22
|2014-04-25	|76.476120	|15.911321	|2014-10-17
|2014-10-28	|100.966883	|-0.028939	|2015-01-05
|2015-02-05	|114.390004	|5.941719	|2015-04-16
|2015-04-28	|124.518583	|-2.413598	|2015-06-25
|2015-10-27	|110.198438	|-7.757693	|2015-12-18
|2016-03-10	|98.271427	|-7.149132	|2016-05-05
|2016-06-23	|93.917337	|-3.967788	|2016-06-27
|2016-06-30	|93.428693	|1.348657	|2016-07-11
|2016-07-25	|95.129174	|10.657861	|2016-11-15
|2016-12-21	|115.614138	|27.545001	|2017-06-27
|2017-08-02	|156.504989	|-2.024989	|2017-10-03
|2017-11-01	|166.890000	|-3.860000	|2018-02-06
|2018-03-08	|176.940000	|-8.600000	|2018-03-27
现在让我们创建一个1,000,000美元的模拟投资组合，并根据我们的规则看看它的表现如何。这包括：
* 在任何交易中只投入投资组合的10%
* 如果损失超过交易价值的20％，则退出该头寸。

在模拟时，请记住下面这些：
* 交易是按照每次100股批量交易的
* 我们的止损规则涉及在价格跌至指定水平以下时下单卖出股票，因此，我们需要检查在此期间的低点是否足够低以触发止损。实际上，除非我们买入认沽期权，否则我们无法保证我们将按照止损设定的价格出售股票，但无论如何为了简单起见，我们都会以此为卖价。
* 每笔交易都需要给经纪人佣金，应该对其进行核算。这里我们不作考虑。

以下是回溯测试：
```python
# We need to get the low of the price during each trade.
tradeperiods = pd.DataFrame({"Start": apple_long_profits.index,
                            "End": apple_long_profits["End Date"]})
apple_long_profits["Low"] = tradeperiods.apply(lambda x: min(apple.loc[x["Start"]:x["End"], "Adj. Low"]), axis = 1)
apple_long_profits
```
| Date | Price | Profit | End Date | Low |
|:-----------|:------------|:------------|:------------|:------------|
|2010-03-16	|28.844953	|3.734615	|2010-06-11	|25.606402
|2010-06-18	|35.222329	|-1.934135	|2010-07-22	|30.791939
|2010-09-20	|36.399003	|8.404812	|2011-03-30	|35.341333
|2011-05-12	|44.539075	|-1.177188	|2011-05-27	|42.335061
|2011-07-14	|45.978431	|2.524014	|2011-11-17	|45.367990
|2011-12-28	|51.744852	|21.402711	|2012-05-09	|51.471117
|2012-06-25	|73.351258	|9.844240	|2012-10-17	|72.688768
|2013-05-17	|56.878472	|-4.619751	|2013-06-26	|51.942335
|2013-07-31	|59.408242	|4.423577	|2013-10-04	|59.001273
|2013-10-16	|66.221597	|1.103650	|2014-01-28	|65.972629
|2014-03-12	|71.752021	|-1.319753	|2014-03-17	|69.932180
|2014-03-24	|72.097002	|-1.001648	|2014-04-22	|68.371743
|2014-04-25	|76.476120	|15.911321	|2014-10-17	|75.409086
|2014-10-28	|100.966883 |-0.028939	|2015-01-05	|99.652062
|2015-02-05	|114.390004 |5.941719	|2015-04-16	|112.949876
|2015-04-28	|124.518583 |-2.413598	|2015-06-25	|117.651750
|2015-10-27	|110.198438 |-7.757693	|2015-12-18	|102.228192
|2016-03-10	|98.271427	|-7.149132	|2016-05-05	|89.752692
|2016-06-23	|93.917337	|-3.967788	|2016-06-27	|89.421814
|2016-06-30	|93.428693	|1.348657	|2016-07-11	|92.158220
|2016-07-25	|95.129174	|10.657861	|2016-11-15	|94.230069
|2016-12-21	|115.614138 |27.545001	|2017-06-27	|113.342546
|2017-08-02	|156.504989 |-2.024989	|2017-10-03	|149.160000
|2017-11-01	|166.890000 |-3.860000	|2018-02-06	|154.000000
|2018-03-08	|176.940000 |-8.600000	|2018-03-27	|164.940000

```python
# Now we have all the information needed to simulate this strategy in apple_adj_long_profits
cash = 1000000
apple_backtest = pd.DataFrame({"Start Port. Value": [],
                         "End Port. Value": [],
                         "End Date": [],
                         "Shares": [],
                         "Share Price": [],
                         "Trade Value": [],
                         "Profit per Share": [],
                         "Total Profit": [],
                         "Stop-Loss Triggered": []})
port_value = .1  # Max proportion of portfolio bet on any trade
batch = 100      # Number of shares bought per batch
stoploss = .2    # % of trade loss that would trigger a stoploss
for index, row in apple_long_profits.iterrows():
    batches = np.floor(cash * port_value) // np.ceil(batch * row["Price"]) # Maximum number of batches of stocks invested in
    trade_val = batches * batch * row["Price"] # How much money is put on the line with each trade
    if row["Low"] < (1 - stoploss) * row["Price"]:   # Account for the stop-loss
        share_profit = np.round((1 - stoploss) * row["Price"], 2)
        stop_trig = True
    else:
        share_profit = row["Profit"]
        stop_trig = False
    profit = share_profit * batches * batch # Compute profits
    # Add a row to the backtest data frame containing the results of the trade
    apple_backtest = apple_backtest.append(pd.DataFrame({
                "Start Port. Value": cash,
                "End Port. Value": cash + profit,
                "End Date": row["End Date"],
                "Shares": batch * batches,
                "Share Price": row["Price"],
                "Trade Value": trade_val,
                "Profit per Share": share_profit,
                "Total Profit": profit,
                "Stop-Loss Triggered": stop_trig
            }, index = [index]))
    cash = max(0, cash + profit)
 
apple_backtest
```

|Start Port. Value	|End Port. Value	|End Date	|Shares	|Share Price	|Trade Value	|Profit per Share	|Total Profit	|Stop-Loss Triggered|
|------------|------------|------------|------------|------------|------------|------------|------------|------------|
|2010-03-16	 |1.000000e+06	|1.012698e+06	|2010-06-11	 |3400.0	|28.844953	 |98072.841239	 |3.734615	|12697.691096 |	0.0
|2010-06-18	 |1.012698e+06	|1.007282e+06	|2010-07-22	 |2800.0	|35.222329	 |98622.521053	 |-1.934135	|-5415.577333 |	0.0
|2010-09-20	 |1.007282e+06	|1.029975e+06	|2011-03-30	 |2700.0	|36.399003	 |98277.306914	 |8.404812	|22692.991110 |	0.0
|2011-05-12	 |1.029975e+06	|1.027268e+06	|2011-05-27	 |2300.0	|44.539075	 |102439.873355  |-1.177188	|-2707.531638 |	0.0
|2011-07-14	 |1.027268e+06	|1.032820e+06	|2011-11-17	 |2200.0	|45.978431	 |101152.549241  |2.524014	|5552.830218  | 0.0
|2011-12-28	 |1.032820e+06	|1.073486e+06	|2012-05-09	 |1900.0	|51.744852	 |98315.218526	 |21.402711	|40665.151235 |	0.0
|2012-06-25	 |1.073486e+06	|1.087267e+06	|2012-10-17	 |1400.0	|73.351258	 |102691.760672  |9.844240	|13781.935982 |	0.0
|2013-05-17	 |1.087267e+06	|1.078490e+06	|2013-06-26	 |1900.0	|56.878472	 |108069.096937  |-4.619751	|-8777.527400 |	0.0
|2013-07-31	 |1.078490e+06	|1.086452e+06	|2013-10-04	 |1800.0	|59.408242	 |106934.835757  |4.423577	|7962.438409  | 0.0
|2013-10-16	 |1.086452e+06	|1.088218e+06	|2014-01-28	 |1600.0	|66.221597	 |105954.555657  |1.103650	|1765.839598  | 0.0
|2014-03-12	 |1.088218e+06	|1.086239e+06	|2014-03-17	 |1500.0	|71.752021	 |107628.031714  |-1.319753	|-1979.628917 |	0.0
|2014-03-24	 |1.086239e+06	|1.084736e+06	|2014-04-22	 |1500.0	|72.097002	 |108145.503103  |-1.001648	|-1502.472160 |	0.0
|2014-04-25	 |1.084736e+06	|1.107012e+06	|2014-10-17	 |1400.0	|76.476120	 |107066.568572  |15.911321	|22275.849051 |	0.0
|2014-10-28	 |1.107012e+06	|1.106983e+06	|2015-01-05	 |1000.0	|100.966883	 |100966.883069  |-0.028939	|-28.938709	  | 0.0
|2015-02-05	 |1.106983e+06	|1.112331e+06	|2015-04-16	 |900.0	    |114.390004	 |102951.003221  |5.941719	|5347.546691  | 0.0
|2015-04-28	 |1.112331e+06	|1.110400e+06	|2015-06-25	 |800.0	    |124.518583	 |99614.866549	 |-2.413598	|-1930.878038 |	0.0
|2015-10-27	 |1.110400e+06	|1.102642e+06	|2015-12-18	 |1000.0	|110.198438	 |110198.437846  |-7.757693	|-7757.693367 |	0.0
|2016-03-10	 |1.102642e+06	|1.094778e+06	|2016-05-05	 |1100.0	|98.271427	 |108098.569555  |-7.149132	|-7864.045388 |	0.0
|2016-06-23	 |1.094778e+06	|1.090413e+06	|2016-06-27	 |1100.0	|93.917337	 |103309.070918  |-3.967788	|-4364.566368 |	0.0
|2016-06-30	 |1.090413e+06	|1.091897e+06	|2016-07-11	 |1100.0	|93.428693	 |102771.562745  |1.348657	|1483.522558  | 0.0
|2016-07-25	 |1.091897e+06	|1.103621e+06	|2016-11-15	 |1100.0	|95.129174	 |104642.091188  |10.657861	|11723.647322 |	0.0
|2016-12-21	 |1.103621e+06	|1.128411e+06	|2017-06-27	 |900.0	    |115.614138	 |104052.724175  |27.545001	|24790.501098 |	0.0
|2017-08-02	 |1.128411e+06	|1.126994e+06	|2017-10-03	 |700.0	    |156.504989	 |109553.492367  |-2.024989	|-1417.492367 |	0.0
|2017-11-01	 |1.126994e+06	|1.124678e+06	|2018-02-06	 |600.0	    |166.890000	 |100134.000000  |-3.860000	|-2316.000000 |	0.0
|2018-03-08	 |1.124678e+06	|1.119518e+06	|2018-03-27	 |600.0	    |176.940000	 |106164.000000  |-8.600000	|-5160.000000 |	0.0

```python
apple_backtest["End Port. Value"].plot()
```
![图30]()
我们的投资组合价值在六年内增长了13％。考虑到每次交易只使用了投资组合的10%，这个收益还不错。

请注意，这个策略从未触发我们的这条规则：永远不允许损失超过所交易价值的20％。为简单起见，我们将在回测中忽略此规则。更现实的投资组合不会将其价值的10％只押在一只股票上。一个更现实的投资组合会考虑投资多种股票。在任何给定的时刻，涉及多个公司的多个交易可能在进行，而且大部分投资组合都是股票，而不是现金。现在我们将投资多只股票并且仅在移动平均线交叉时退出（不是因为止损），我们需要改变我们回溯测试的方法。例如，我们将使用一个pandas DataFrame来包含所有要考虑股票的所有买卖订单，我们上面的循环必须跟踪更多信息。

我编写了用于创建多个股票的订单数据的函数，以及用于执行回测的函数。
```python
def ma_crossover_orders(stocks, fast, slow):
    """
    :param stocks: A list of tuples, the first argument in each tuple being a string containing the ticker symbol of each stock (or however you want the stock represented, so long as it's unique), and the second being a pandas DataFrame containing the stocks, with a "Close" column and indexing by date (like the data frames returned by the Yahoo! Finance API)
    :param fast: Integer for the number of days used in the fast moving average
    :param slow: Integer for the number of days used in the slow moving average
 
    :return: pandas DataFrame containing stock orders
 
    This function takes a list of stocks and determines when each stock would be bought or sold depending on a moving average crossover strategy, returning a data frame with information about when the stocks in the portfolio are bought or sold according to the strategy
    """
    fast_str = str(fast) + 'd'
    slow_str = str(slow) + 'd'
    ma_diff_str = fast_str + '-' + slow_str
 
    trades = pd.DataFrame({"Price": [], "Regime": [], "Signal": []})
    for s in stocks:
        # Get the moving averages, both fast and slow, along with the difference in the moving averages
        s[1][fast_str] = np.round(s[1]["Close"].rolling(window = fast, center = False).mean(), 2)
        s[1][slow_str] = np.round(s[1]["Close"].rolling(window = slow, center = False).mean(), 2)
        s[1][ma_diff_str] = s[1][fast_str] - s[1][slow_str]
 
        # np.where() is a vectorized if-else function, where a condition is checked for each component of a vector, and the first argument passed is used when the condition holds, and the other passed if it does not
        s[1]["Regime"] = np.where(s[1][ma_diff_str] > 0, 1, 0)
        # We have 1's for bullish regimes and 0's for everything else. Below I replace bearish regimes's values with -1, and to maintain the rest of the vector, the second argument is apple["Regime"]
        s[1]["Regime"] = np.where(s[1][ma_diff_str] < 0, -1, s[1]["Regime"])
        # To ensure that all trades close out, I temporarily change the regime of the last row to 0
        regime_orig = s[1].loc[:, "Regime"].iloc[-1]
        s[1].loc[:, "Regime"].iloc[-1] = 0
        s[1]["Signal"] = np.sign(s[1]["Regime"] - s[1]["Regime"].shift(1))
        # Restore original regime data
        s[1].loc[:, "Regime"].iloc[-1] = regime_orig
 
        # Get signals
        signals = pd.concat([
            pd.DataFrame({"Price": s[1].loc[s[1]["Signal"] == 1, "Adj. Close"],
                         "Regime": s[1].loc[s[1]["Signal"] == 1, "Regime"],
                         "Signal": "Buy"}),
            pd.DataFrame({"Price": s[1].loc[s[1]["Signal"] == -1, "Adj. Close"],
                         "Regime": s[1].loc[s[1]["Signal"] == -1, "Regime"],
                         "Signal": "Sell"}),
        ])
        signals.index = pd.MultiIndex.from_product([signals.index, [s[0]]], names = ["Date", "Symbol"])
        trades = trades.append(signals)
 
    trades.sort_index(inplace = True)
    trades.index = pd.MultiIndex.from_tuples(trades.index, names = ["Date", "Symbol"])
 
    return trades
 
 
def backtest(signals, cash, port_value = .1, batch = 100):
    """
    :param signals: pandas DataFrame containing buy and sell signals with stock prices and symbols, like that returned by ma_crossover_orders
    :param cash: integer for starting cash value
    :param port_value: maximum proportion of portfolio to risk on any single trade
    :param batch: Trading batch sizes
 
    :return: pandas DataFrame with backtesting results
 
    This function backtests strategies, with the signals generated by the strategies being passed in the signals DataFrame. A fictitious portfolio is simulated and the returns generated by this portfolio are reported.
    """
 
    SYMBOL = 1 # Constant for which element in index represents symbol
    portfolio = dict()    # Will contain how many stocks are in the portfolio for a given symbol
    port_prices = dict()  # Tracks old trade prices for determining profits
    # Dataframe that will contain backtesting report
    results = pd.DataFrame({"Start Cash": [],
                            "End Cash": [],
                            "Portfolio Value": [],
                            "Type": [],
                            "Shares": [],
                            "Share Price": [],
                            "Trade Value": [],
                            "Profit per Share": [],
                            "Total Profit": []})
 
    for index, row in signals.iterrows():
        # These first few lines are done for any trade
        shares = portfolio.setdefault(index[SYMBOL], 0)
        trade_val = 0
        batches = 0
        cash_change = row["Price"] * shares   # Shares could potentially be a positive or negative number (cash_change will be added in the end; negative shares indicate a short)
        portfolio[index[SYMBOL]] = 0  # For a given symbol, a position is effectively cleared
 
        old_price = port_prices.setdefault(index[SYMBOL], row["Price"])
        portfolio_val = 0
        for key, val in portfolio.items():
            portfolio_val += val * port_prices[key]
 
        if row["Signal"] == "Buy" and row["Regime"] == 1:  # Entering a long position
            batches = np.floor((portfolio_val + cash) * port_value) // np.ceil(batch * row["Price"]) # Maximum number of batches of stocks invested in
            trade_val = batches * batch * row["Price"] # How much money is put on the line with each trade
            cash_change -= trade_val  # We are buying shares so cash will go down
            portfolio[index[SYMBOL]] = batches * batch  # Recording how many shares are currently invested in the stock
            port_prices[index[SYMBOL]] = row["Price"]   # Record price
            old_price = row["Price"]
        elif row["Signal"] == "Sell" and row["Regime"] == -1: # Entering a short
            pass
            # Do nothing; can we provide a method for shorting the market?
        #else:
            #raise ValueError("I don't know what to do with signal " + row["Signal"])
 
        pprofit = row["Price"] - old_price   # Compute profit per share; old_price is set in such a way that entering a position results in a profit of zero
 
        # Update report
        results = results.append(pd.DataFrame({
                "Start Cash": cash,
                "End Cash": cash + cash_change,
                "Portfolio Value": cash + cash_change + portfolio_val + trade_val,
                "Type": row["Signal"],
                "Shares": batch * batches,
                "Share Price": row["Price"],
                "Trade Value": abs(cash_change),
                "Profit per Share": pprofit,
                "Total Profit": batches * batch * pprofit
            }, index = [index]))
        cash += cash_change  # Final change to cash balance
 
    results.sort_index(inplace = True)
    results.index = pd.MultiIndex.from_tuples(results.index, names = ["Date", "Symbol"])
 
    return results
 
# Get more stocks
(microsoft, google, facebook, twitter, netflix,
amazon, yahoo, ge, qualcomm, ibm, hp) = (quandl.get("WIKI/" + s, start_date=start,
                                                                         end_date=end) for s in ["MSFT", "GOOG", "FB", "TWTR",
                                                                                                 "NFLX", "AMZN", "YHOO", "GE",
                                                                                                 "QCOM", "IBM", "HPQ"])
```
```python
signals = ma_crossover_orders([("AAPL", apple),
                              ("MSFT",  microsoft),
                              ("GOOG",  google),
                              ("FB",    facebook),
                              ("TWTR",  twitter),
                              ("NFLX",  netflix),
                              ("AMZN",  amazon),
                              ("YHOO",  yahoo),
                              ("GE",    ge),
                              ("QCOM",  qualcomm),
                              ("IBM",   ibm),
                              ("HPQ",   hp)],
                            fast = 20, slow = 50)
signals
```
![图31]()

```python
bk = backtest(signals, 1000000)
bk
```
![图32]()

```python
bk["Portfolio Value"].groupby(level = 0).apply(lambda x: x[-1]).plot()
```
![图33]()

一个更现实的投资组合可以投资于12只（科技股）股票中的任何一个，最终增长率约为100％。这有多好呢？虽然表面上还不错，但我们会看到我们可以做得更好。
##基准策略
回归测试仅是评估交易策略效果的一部分。我们希望对策略进行基准测试，或者将其与其他可用（通常是众所周知的）策略进行比较，以确定我们做的有多好。

无论何时评估一个交易系统，有一个策略你都应该检查，此策略超过了除少数有管理的共同基金和投资经理之外的所有策略：购买并持有[SPY](https://finance.yahoo.com/quote/SPY)。有效市场假说认为，任何人都不可能击败市场。因此，应该总是购买仅反映市场构成的指数基金。通过购买和持有SPY，我们可以有效地试图将我们的回报与市场相匹配而不是击败它。

我只看购买和持有SPY的利润。
```python
#spyder = web.DataReader("SPY", "yahoo", start, end)
spyder = spyderdat.loc[start:end]
spyder.iloc[[0,-1],:]
```
|date|Open	|High	|Low	|Close	|Adj Close|
|------------|------------|------------|------------|------------|------------|					
|2015-06-01	|211.94	|212.34	|210.620 |211.57 |211.57
|2019-05-30	|279.11	|280.04	|277.805 |279.03 |279.03

```python
batches = 1000000 // np.ceil(100 * spyder.loc[:,"Adj Close"].iloc[0]) # Maximum number of batches of stocks invested in
trade_val = batches * batch * spyder.loc[:,"Adj Close"].iloc[0] # How much money is used to buy SPY
final_val = batches * batch * spyder.loc[:,"Adj Close"].iloc[-1] + (1000000 - trade_val) # Final value of the portfolio
final_val
```
>1317061.9999999998
```python
# We see that the buy-and-hold strategy beats the strategy we developed earlier. I would also like to see a plot.
ax_bench = (spyder["Adj Close"] / spyder.loc[:, "Adj Close"].iloc[0]).plot(label = "SPY")
ax_bench = (bk["Portfolio Value"].groupby(level = 0).apply(lambda x: x[-1]) / 1000000).plot(ax = ax_bench, label = "Portfolio")
ax_bench.legend(ax_bench.get_lines(), [l.get_label() for l in ax_bench.get_lines()], loc = 'best')
ax_bench
```
![图34]()
购买和持有SPY的效益与我们的交易系统基本一样，至少根据我们目前的如何设置是这样，我们甚至还没有考虑到我们这个更复杂的策略在费用方面的成本。鉴于机会成本以及主动策略相关的费用，我们不应该采用它。

我们能做些什么来改善我们系统的性能呢？对于初学者，我们可以尝试多样化。我们考虑的所有股票都是科技公司，这意味着如果科技行业表现不佳，我们的投资组合将反映出这一点。我们可以尝试开发一个也可以卖空股票或做跌的系统，这样我们就可以利用任何方向的趋势了。我们可以寻找预测股票会达到多高的方式。但是，无论我们做什么，都必须超过这个基准;否则我们的交易系统会有机会成本。

还存在其他的基准策略，如果我们的交易系统超越了“买入并持有SPY”这一策略，我们可以跟他们做比较。这些策略包括：
* 当其月收盘价高于其十个月移动均线时，买入SPY
* 当其十个月的动量为正时，买入SPY。（动量是移动平均过程的第一个区别，或者$MO_t^q = MA_t^q-MA_{t-1}^q$.）

(我在[这里](https://www.r-bloggers.com/are-r2s-useful-in-finance-hypothesis-driven-development-in-reverse/?utm_source=feedburner&utm_medium=email&utm_campaign=Feed%3A+RBloggers+%28R+bloggers%29)第一次阅读了这些策略。)通用的教训仍然有效:不要使用一个带有大量活跃交易的复杂交易系统，如果一个使用了指数基金且不需要频繁交易的简单策略就能超越它。[这实际上是一个非常难以满足的要求。](http://www.nytimes.com/2015/03/15/your-money/how-many-mutual-funds-routinely-rout-the-market-zero.html?_r=0)

最后一点，假设你的交易系统确实设法超越了所有在回归测试中给它的基准策略。那回归测试能预测将来的收益么？根本不能。[回归测试具有过度拟合的倾向](http://papers.ssrn.com/sol3/papers.cfm?abstract_id=2745220)，所以，回测预测高增长并不意味着未来增长可以持续。有针对过度拟合的策略，例如做[前瞻性分析](https://ntguardian.wordpress.com/2017/06/19/walk-forward-analysis-demonstration-backtrader/)并将数据集的一部分（可能是最新的部分）作为最终测试集来确定策略是否盈利，然后再使用“sitting on”策略，通过这两个过滤器仍能存活的策略，再看它在当前的市场中是否仍能盈利。
##总结
虽然这个课程以一个令人沮丧的说明结束，但请记住，[有效市场假说也有许多批评者](http://www.nytimes.com/2009/06/06/business/06nocera.html)。我自己的观点是，随着交易变得更加算法化，击败市场将变得更加困难。也就是说，还是有可能击败市场的，尽管共同基金似乎无法做到这一点（但请记住，共同基金表现如此糟糕的部分原因是因为费用，这不是指数基金所担心的）。

本课程非常简短，仅涵盖一种策略：基于移动平均线的策略。还有许多其他被使用的交易策略。此外，我们从未深入讨论做空股票，货币交易或股票期权。特别的，股票期权是一个丰富的课题，它提供了许多不同的方式来押注股票的趋势方向。您可以在使用Python的衍生品分析一书中阅读更多关于衍生品（包括股票期权和其他衍生品）的信息：数据分析，模型，模拟，校准和套期保值，[可以从犹他大学图书馆获得这本书](http://proquest.safaribooksonline.com.ezproxy.lib.utah.edu/9781119037996)。

另一种资源（我在撰写本课程时把它作为参考资料）是O'Reilly的Python for Finance一书，[也可以从犹他大学图书馆获得。](http://proquest.safaribooksonline.com.ezproxy.lib.utah.edu/book/programming/python/9781491945360)

如果您对研究算法交易感兴趣，那么您将从何处开始？我不建议使用我上面编写的代码进行回溯测试；对于这个任务，有更好的工具包。python有一些用于算法交易的库，例如[pyfolio](https://quantopian.github.io/pyfolio/)（用于分析），[zipline](http://www.zipline.io/beginner-tutorial.html)（用于回测和算法交易），以及[backtrader](https://www.backtrader.com/)（也用于回测和交易）。zipline似乎很受欢迎，因为它是由[quantopian](https://www.quantopian.com/)使用和开发的，这是一个“众包对冲基金”，允许用户使用他们的数据进行回溯测试,甚至在给他们一些利润的情况下，可以从他们的作者那里获得可获利的策略许可。但是，我更喜欢backtrader并撰写了有关使用它的[博客文章](https://ntguardian.wordpress.com/tag/backtrader/)。两者比较，它可能更复杂，但这是更强大功能的代价。我是它的设计的粉丝。我也建议学习R，因为它有许多用于分析财务数据的软件包（比Python更多），并且在Python中使用R函数非常容易（正如我在[这篇文章](https://ntguardian.wordpress.com/2017/06/28/stock-trading-analytics-and-optimization-in-python-with-pyfolio-rs-performanceanalytics-and-backtrader/)中所展示的那样）。

您可以在我的[博客](https://ntguardian.wordpress.com/category/economics-and-finance/)上阅读有关在金融中使用R和Python的更多信息。

请记住，有可能（如果不常见）在股票市场上赔钱。虽然很难找到像股票那样的回报，但任何投资策略都应该认真对待投资。本课程旨在为评估股票交易和投资提供一个起点，更一般地说，分析时态数据，我希望您能继续探索这些思想。