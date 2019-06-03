
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
![图一](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/%E5%9B%BE1.png)
我们简单的讨论一下，Open是交易日开始时的股票价格（不需要是上一个交易日的收盘价），High是交易日中的最高股价，Low是交易日中的最低股价，Close是交易日结束时的股价。Volume指有多少交易了股票。Adjusted Prices（例如调整后的收盘价）是因为公司行为而调整后的股价。虽然认为股价主要由交易员设定，但股票拆分（公司使每只现存股票分成两个并使价格减半）和分红（支付公司的每股利润）也会影响股价，也需要加以考虑。
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
![图二](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/Stock%20Data%20Analysis%20with%20Python/%E5%9B%BE2.png)
一个折线图是可以的，但是每天至少涉及4个变量（开盘价，收盘价，最高价，最低价），我们希望有某种可视化方法能够查看4个变量而不需要绘制4个单独的折线。金融数据通常绘制成日本蜡烛图，之所以这么叫是因为它是18世纪的日本大米交易者创建的。虽然需要一些努力，但是这种图可以用matplotlib创建。

我创建了一个函数并使用它来绘制我们的股票数据，也欢迎你使用它从pandas DataFrame中更方便地绘制蜡烛图。（代码是基于这个[例子](http://matplotlib.org/examples/pylab_examples/finance_demo.html) ， 你也可以在[这里](http://matplotlib.org/api/finance_api.html)查看相关函数的文档）
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