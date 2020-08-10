# Spring Boot VS Spring Cloud
>作为一名初学者，经常搞不清楚Spring Boot和Spring Cloud的区别，本文做了一些简单的总结。

1. 两者都是开源的框架，都是Spring Framework的一部分；在微服务方面，Spring Boot主要用来创建微服务，而Spring Cloud用来进行配置管理。

2. 两者提供的功能罗列如下:

|Spring Cloud|Spring Boot|
| ---- | ---- |
| 智能路由和服务发现|创建独立的Spring应用程序| 
| 服务间的调用管理| 创建Web应用程序：可以使用嵌入的Tomcat,Jetty,Undertow直接创建Http服务器| 
|负载均衡|外部化配置 | 
|Leader选举|安全：内置了基本的Http安全认证   | 
| 全局锁|应用程序事件和监听器   | 
| 分布式配置和分布式消息|   | 
3. 注解(Annotations)
   下面列出Spring Cloud和Spring Boot的5个主要注解

|Spring Cloud|Spring Boot|
| ---- | ---- |
|@EnableConfigServer：表示该应用程序为配置服务器|@SpringBootApplication：Spring Boot 1.2.0中引入的，相当于@Configuration，@EnableAutoConfiguration，@ComponentScan三个注解的结合
|
|@EnableEurekaServer：用于服务发现|@EnableAutoConfiguration：在Spring Boot的version低于1.1或者没有使用@SpringBootApplication的情况下，需要加上该annotation|
|@EnableDiscovreyClient：用于发现其他服务|@ContextConfiguration：用于为JUnit Test提供SpringBoot的部分配置|
|@EnableCircuitBreaker：当相关服务调用失败时，使用Circut Breaker模式继续运行|@SpringApplicationConfiguration：和@ContextConfiguration作用一样，但提供全部配置|
|@HystrixCommand(fallbackMethod="fallbackMethodName")：用于标记回调方法|@ConditionalOnBoot：定义了几个条件注解|

4. 各自的优缺点

优点：

|Spring Cloud|Spring Boot|
| ---- | ---- |
|提供云服务开发|能够快速开发和运行独立的Spring Web应用|
|基于微服务的架构配置|按需配置Spring功能，自动加载Bean|
|提供服务间通信|不需要基于XML的配置，内置了Tomcat，Jetty从而避免了复杂的部署|
|基于Spring Boot模型|无需部署WAR文件|

缺点：

|Spring Cloud|Spring Boot|
| ---- | ---- |
|有很多的依赖|对应用程序的控制比较弱，很多不需要的依赖也会被部署|


![Fintech技术汇](https://img2020.cnblogs.com/blog/498574/202008/498574-20200801213206265-563825556.jpg)