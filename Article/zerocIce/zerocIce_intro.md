# 分布式RPC框架ZeroC Ice简介

>开发分布式或较大型的软件时，必不可少的要进行系统间通信，目前比较常用的框架有Http RestFul，Thrift，gRPC等等，今天分享的ZeroC Ice也是其中一员。

>ZeroC公司出品的Ice（Internet Communication Engine）框架专注于RPC通信，经过了10多年的发展，已经非常的成熟，它的主要优点是高性能，跨语言，跨平台，面向对象，开源等等，可以查看其官方网站（https://zeroc.com/products/ice） 了解更多。这里简单介绍一下它的架构和使用方法。

### 架构和基本概念
>先上一张从ZeroC官方网站上截的图：

![ZeroC Ice Arch](https://img2020.cnblogs.com/blog/498574/202008/498574-20200804005502541-687998359.png)

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

### 使用方法

>在了解了Ice的结构和基本概念之后，让我们动手写个demo看看具体怎么使用吧。为了体现Ice的跨语言和跨平台功能，我们这里用Java实现server端，用C#实现client端。程序的主要功能：client可以通过向sever发送A股的股票代码来获得其对应公司的详细信息， 下面我们一起看看具体的步骤。（注：这里的公司信息都是dummy的）

1. 下载并安装Ice
   >从 https://zeroc.com/download/Ice/3.7/Ice-3.7.4.msi 下载Ice 3.7.4并安装，笔者把它安装在了D:\Program Files下面，打开D:\Program FilesZeroC\Ice-3.7.4\bin文件夹，我们可以看到有名为slice2java.exe, slice2cs.exe的程序，这些就是用来自动生成Java和C#代码的，我们在下面会用到。除了Java和C#的代码生成器，还有许多其他语言的，如：cpp, php等等
![install dir](https://img2020.cnblogs.com/blog/498574/202008/498574-20200804005631596-1121223076.png)

2. 定义client和service的交互接口：这里我们定义两个class：CompanyInfo和AStockService：
   
``` java
   module com
   {
      module astock
      {
          class CompanyInfo
          {
              int id;
              string name;
              string addr;
          }
      }
  }

  module com
  {
      module astock
      {
          interface AStockService
          {
              CompanyInfo GetCompanyInfo(int id);
          }
      }
  }   
```

3. 使用1中的工具把2中定义的类生成C#和Java对应的代码
   > 打开cmd,执行下图中的命令，通过执行slice2java和slice2cs程序，我生成了如下图所示的代码，其中C# code只有一个文件，而Java code有四个文件
![](https://img2020.cnblogs.com/blog/498574/202008/498574-20200804005724792-1862506735.png)

4. 生成Code的简单介绍
   > C#虽然只有一个文件，但是里面包含了三个接口，三个类，一个委托，如下图：
![](https://img2020.cnblogs.com/blog/498574/202008/498574-20200804005842929-1895117817.png)
   * AStockServiceOperations_接口：包含了我们定义的操作即GetCompanyInfo
   * AStockService接口：我们定义的接口，它继承自AStockServiceOperations_
   * AStockServicePrx接口：客户端代理接口
   * CompanyInfo类： 用于传递数据的DTO
   * AStockServiceDisp_类： 服务端的dispatch抽象类，即上文中说到的skeleton，它实现了我们定义的接口AStockService
   * AStockServicePrxHelper： 客户端的代理类，它实现了AStockServicePrx接口

   > Java的code生成了两个类，两个接口
   * AStockService接口：我们定义的接口，也是服务端的skeleton
   * AStockServicePrx接口：客户端代理接口
   * CompanyInfo类： 用于传递数据的DTO
   * _AStockServicePrxI类：客户端代理类

5. 编写服务端代码
   >用熟悉的IDE新建一个工程，笔者使用的Intelij Idea,把上面生成的Java代码加到项目中，并添加Ice的Maven依赖，如下图所示，当然也可以手动下载ice jar包，并手动添加。
![](https://img2020.cnblogs.com/blog/498574/202008/498574-20200804005929215-996068789.png)


   下面创建一个类AStockServiceServer，实现AStockService接口，其GetCompanyInfo方法返回一个dummy CompanyInfo对象，如下所示
   
   ``` java
   
    public class AStockServiceServer implements AStockService {
        @Override
        public CompanyInfo GetCompanyInfo(int id, Current current) {
            CompanyInfo info = new CompanyInfo();
            info.id = 1234;
            info.name = "中国平安";
            info.addr = "深圳";
            return info;
        }
    }

   ```

   >创建包含main的class AStockServiceServerMain，如下所示:
   
```java
    public class AStockServiceServerMain {
        public static void main(String[] args) {
            try (Communicator communicator = Util.initialize()) {//创建communicator
            ObjectAdapter oa = communicator.createObjectAdapterWithEndpoints("AStockServiceAdapter", "default -p 10000");//创建一个Adatper，Id是AStockServiceAdapter，绑定到10000端口

            AStockServiceServer servant = new AStockServiceServer();//我们的服务
            oa.add(servant, Util.stringToIdentity("AStockService"));//把我们创建的服务加到上面创建的adapter里
            oa.activate();//激活adapter
            System.out.println("AStock Service Server is running");//输出启动log
            communicator.waitForShutdown();//等待结束
        }
    }
}
```

6. 编写客户端代码
   >用VS新建一个控制台程序，并把上面生成的C#代码加入项目中，然后添加Ice的nuget包，如下图所示：
![](https://img2020.cnblogs.com/blog/498574/202008/498574-20200804010000246-1545202394.png)


   >在main函数中编写如下代码：

```java

    class Program
    {
        static void Main(string[] args)
        {
            using (var communicator = Util.initialize(ref args))//创建Communicator对象
            {
                ObjectPrx basePrx = communicator.stringToProxy("AStockService:default -p 10000");//创建客户端基类代理

                AStockServicePrx aStockServicePrx = AStockServicePrxHelper.checkedCast(basePrx);//把基类代理转换为子类代理
                var companyInfo = aStockServicePrx.GetCompanyInfo(1000);//调用GetCompanyInfo方法

                Console.WriteLine($"id:{companyInfo.id} name:{companyInfo.name} addr:{companyInfo.addr}");//输出返回结果
            }
        }
    }

```

7. 联调
>先运行Java服务端，然后再启动C#程序可以得到如下结果，可以看到Client端成功的获取到了CompanyInfo对象。
![](https://img2020.cnblogs.com/blog/498574/202008/498574-20200804010018014-404661481.png)


### 总结
>本文介绍了ZeroC Ice的概念并用一个demo详细说明了具体使用方法，期望对读者能够有所帮助


![Fintech技术汇](https://img2020.cnblogs.com/blog/498574/202008/498574-20200801213206265-563825556.jpg)