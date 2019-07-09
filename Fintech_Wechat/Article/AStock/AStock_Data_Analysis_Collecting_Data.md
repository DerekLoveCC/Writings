# A股数据分析之收集数据：股票列表和股价
***
>数据是进行数据分析的前提，本文主要讲述如何使用Python收集中国沪深两市的基本股票数据：股票列表和股价。
##1. 股票列表
>总所周知，对于A股，中国有两个交易所即上海证券交易所和深圳证券交易所。我们主要从他们的官方网站上获得所有上市的A股列表。

>对于上海交易所，我们从(http://www.sse.com.cn/assortment/stock/list/share/)下载，当打开该页面时，会看到在右上角有一个下载按钮，如下图所示：
![上交所股票列表]()
那么我们如何通过Python来下载这些数据呢？我们还是直接来上代码吧，如下：
```Python
from urllib import request

#Download A-Stock stock list

sse_stock_list_url = 'http://query.sse.com.cn/security/stock/downloadStockListFile.do?csrcCode=&stockCode=&areaName=&stockType=1'
request_headers = {'X-Requested-With': 'XMLHttpRequest',
                   'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) ' 'Chrome/56.0.2924.87 Safari/537.36',
                   'Referer': 'http://www.sse.com.cn/assortment/stock/list/share/'
                  }

req = request.Request(sse_stock_list_url, headers=request_headers)
resp = request.urlopen(req)
result = resp.read().decode('gb2312')#please use gb2312 to decode otherwise you will not get correct data

```
>这里需要注意的是必须提供headers，否则会提示如下错误：
```Json
null({"jsonCallBack":"null","success":"false","error":"系统繁忙...","errorType":"ExceptionInterceptor"})
```
>上述程序运行结果如下图：

![SSE股票列表代码执行结果]()

>返回的股票列表是一个用\t分隔的csv文件，由于我们已经通过decode函数把数据解析成字符串了，所以下面使用pandas直接解析该字符串：
```python
import pandas as pd
from io import StringIO

TESTDATA = StringIO(result)
df = pd.read_csv(TESTDATA, sep='\t')
print(df)
```
运行结果如下：
![Pandas读取SSE股票列表]()

>对于深交所的股票列表，可以通过类似的方法获得，这是URL和发送的request参数有所不同。打开http://www.szse.cn/market/stock/list/index.html， 在该页面上同样有一个可以下载股票列表的按钮，如下图所示：
![SZSE股票列表网页]()
下面是下载该列表的python code:
```Python
szse_stock_list_url = 'http://www.szse.cn/api/report/ShowReport?SHOWTYPE=xlsx&CATALOGID=1110&TABKEY=tab1'
szse_stock_list_file = 'szse_stock_list.xlsx'
request.urlretrieve(szse_stock_list_url, szse_stock_list_file)
```
>以上代码会把股票列表下载到当前目录下的一个名为szse_stock_list.xlsx的文件中，格式为excel，下面我们使用pandas读取该文件：
```python
data = pd.read_excel(szse_stock_list_file)
print(data)
```
以上代码运行结果如下：
![SZSE股票列表网页]()

##2.股票价格
>在上一部分，我们获得了沪深两市的股票列表并且把它们导入到了pandas中，下面就来看看如何获得这些股票的每日股价信息。这些我们使用网易财经，下面是一个例子：

>http://quotes.money.163.com/service/chddata.html?code=0600138&start=20040101&end=20190710&fields=TCLOSE;HIGH;LOW;TOPEN;LCLOSE;CHG;PCHG;TURNOVER;VOTURNOVER;VATURNOVER;TCAP;MCAP

>URL中的几个参数我们需要解释一下：
1. code： &emsp;股票代码，这个信息我们已经在上面部分获得.注意：对0开头的股票需要在原始code前加1，对于6开头的需要加0
2. start: &emsp;&emsp;开始日期
3. end:   &emsp;&emsp; 结束日期，和start一起表明我们想要获取哪个时间段的股价信息
4. fields:&emsp;&emsp;列出你想要获得哪些方面的数据如开盘价（TOPEN)

>下面是获得股价的代码以及运行结果，由于上一部分获得股票太多，这里只用600138作为例子：
```python
stock_code='0600138'#注意：在原始code前加了个0
start_date='20040101'
end_date='20190710'
stock_price_csv = '600138.csv'
url = f'http://quotes.money.163.com/service/chddata.html?code={stock_code}&' \
            f'start={start_date}&end={end_date}&' \
            f'fields=TCLOSE;HIGH;LOW;TOPEN;LCLOSE;CHG;PCHG;TURNOVER;VOTURNOVER;VATURNOVER;TCAP;MCAP'
request.urlretrieve(url,stock_price_csv)
```
>导入到pandas中的代码如下
```python
stock_price_data = pd.read_csv(stock_price_csv, encoding='gbk')
print(stock_price_data)
```
>运行结果如下：
![股价获取结果]()



