>微信公众号：**[Fintech极客](#jump_fintech)**
作者为软件开发工程师，就职于金融信息科技类公司，通过CFA一级，分享计算机和金融相结合领域的技术和知识。

# A股数据分析之收集数据：公司详细信息

***
>分析股票时, 我们往往需要查看公司的一些基本信息，当然可以在网上搜索公司名称，然后进入其官方网站查看，但是如果想获取很多公司的信息，这种人工的方式肯定是不行的。本文主要介绍如何用Python从网络上抓取公司的详细信息。

>许多财经网站上都有上市公司的介绍信息，比如新浪财经，网易财经和东方财富网，经过对比发现，网易财经的信息更规整也更容易获取一些。在浏览器中输入网址：http://quotes.money.163.com/f10/gszl_600011.html， 将打开如下所示的网页：

![Company Info](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/AStock/company_info/163_companyInfo.png)

>该网页中包含华能国际的公司资料信息，如公司简介，IPO资料，公司高管等等，下面我们就一起看看如何使用Python把其中的公司简介和IPO资料信息抓取下来。
我们会用到python的两个库：1.requests 2.bs4，如果没有安装，请使用pip或者Anancoda进行安装。

>第一步，我们分析一下网址 http://quotes.money.163.com/f10/gszl_600011.html, 很容易发现网址的最后一部分 gszl_600011.html 中的gszl是公司资料的汉语拼音缩写，下划线后面的600011是股票代码，所以给定任意股票代码，我们只需要替换掉网址中的股票代码就能得到新股票的公司资料url

>第二步，我们引入要使用的库，然后使用requests获取相应的页面，具体代码如下所示：

![Company Info html](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/AStock/company_info/163_companyInfoHtml_code.png)

>第三步，我们获取html之后，就可以使用bs4中的BeautifulSoup类来解析了。首先，在获得的html文本中找到我们想要的信息(使用Notepad等文件编辑器打开，并搜索即可)，会发现我们想要的信息就table标记中，而该table的class属性中包含table_details，如下图所示：

![Company Info table in html](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/AStock/company_info/163_companyInfo_tableInHtml.png)

>所以我们可以使用BeautifulSoup的select函数来获取这些table，代码如下：

![Company Info table](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/AStock/company_info/163_companyInfo_table.png)

>获得table之后，我们就可以通过如下的代码来得到具体的公司信息了，信息最后会放到名为result的字典中，以下是执行结果截图：

![execute result](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/AStock/company_info/execute_result.png)

>到此，获取公司的基本信息的方法就介绍完毕了，读者只需把相应的代码封装成函数，并传入相应的url即可获得相应的dic，url的构造请参考第一步中的解析。
完整notebook请参见:https://github.com/DerekLoveCC/Writings/blob/master/Fintech_Wechat/Code/AStockDataAnalysis/CrawlCompanyDetailInfo.ipynb

<a id="jump_fintech"></a>
![Fintech极客](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Fintech.jpg)