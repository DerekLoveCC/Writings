# 分布式RPC框架ZeroC Ice简介

>开发分布式或较大型的软件时，必不可少的要进行系统间通信，目前比较常用的框架有Http RestFul，Thrift，gRPC等等，今天分享的ZeroC Ice也是其中一员。

>ZeroC公司出品的Ice（Internet Communication Engine）框架专注于RPC通信，经过了10多年的发展，已经非常的成熟，它的主要优点是高性能，跨语言，跨平台，面向对象，开源等等，可以查看其官方网站（https://zeroc.com/products/ice） 了解更多。这里简单介绍一下它的架构和使用方法。

### 架构和基本概念
>先上一张从ZeroC官方网站上截的图：

![ZeroC Ice Arch]("../image/arch.png")

>从图中可以看出，使用Ice的程序分为客户端和服务器端，而两端的代码都由以下三部分组成：
1. 应用程序代码，这是由使用者编写的部分
2. 自动生成的代码，由Ice提供的工具自动生成的，为client端生成的叫proxy，为server端生成的叫skeleton
3. Ice库代码，这部分是Ice框架的核心部分，各个平台都有对应的库

>从整体来看，Ice框架的结构还是很清晰的，下面介绍几个关键的概念：

* Slice（Specification Language for Ice）：用于定义client和server通信接口的领域特定语言（DSL），在 https://doc.zeroc.com/ice/3.7/the-slice-language 中有关于它的详细信息
* Communicator：Ice运行时的入口点，它所关联的资源包括：线程池，运行时的配置属性，对象工厂，Logger（用于处理Ice运行时产生的log消息），Plug-in manager，Object Adapters等
* Ice Object：是一个抽象的概念，它具用类型，标识等信息，有点像C#或Java中类的概念
* Servants：上面说到Ice Object是抽象概念，而这里的Servant就是提供具体操作的实体，它负责响应client请求的，一个Servant可以对应一个或者多个Ice Object
* Proxy：代表Ice Object，client使用proxy来和server交互
* Ice Object Adapter：存在服务端的communicator中，负责Ice运行时和Server端应用代码的交互，它会和一个或多个Endpoint绑定并接收client端的请求，然后把请求转给对应的Servant从而执行应用代码

https://zeroc.com/download/Ice/3.7/Ice-3.7.4.msi

### 使用方法

>在了解了Ice的结构和基本概念之后，让我们动手写个demo看看具体怎么使用吧。为了体现Ice的跨语言和跨平台功能，我们这里用Java实现server端，用C#实现client端。程序的主要功能：client可以通过向sever发送A股的股票代码来获得其对应公司的详细信息， 下面我们一起看看具体的步骤。（注：这里的公司信息都是dummy的）







![Fintech技术汇](https://img2020.cnblogs.com/blog/498574/202008/498574-20200801213206265-563825556.jpg)