# Prism EventAggregator
## EventAggregator简介
> 本部分主要讲述EventAggregator是什么，解决了什么问题。
## 主要知识点
> 1. 多播
> 2. 转发规则，基于类型（EventBase子类），内置的PubSubEvent
> 3. 线程模式
>   * publisher thread (default option)
>   * UI thread (要求Aggregator对象在UI线程上创建)
>   * Background Thread
> 4. filter
> 5. weak vs strong reference, weak有性能问题，strong需要unsubscribe
> 6. unsubscribe
>   * 使用原方法
>   * 使用Subscriber Token
## 代码示例
## 总结
