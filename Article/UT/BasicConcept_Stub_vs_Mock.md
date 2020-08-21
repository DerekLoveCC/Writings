Fake包括Stub和mock

Stub:基于状态验证，不验证stub的属性，只能对接口或抽象类，虚方法提供解决方法

Mock:基于行为验证，可以验证mock对象的属性.MS的fakes框架通过在运行时改变编译的代码来提供运行时方法拦截，从而能够处理实例方法或静态方法

Stub和Mock结合设计与测试的思想也是不同的，前者称为classical的方式，而后者称为mockist风格。
一般情况下，实现mock的框架都会包括stub的功能，mock在检测调用是否发生，怎么发生，在测试边界效应（副作用），协议，对象间的交互时，非常有用，mock的缺点是：容易被过度使用，在不必要的情况下而使用mock的行为验证，从而导致测试与实现代码的紧耦合，进而阻止了重构

MS的fakes框架有两种test doubles：stub和shims
shims能够让你因为现实的原因，做本不应该做的事。再考虑隔离依赖时，优先考虑使用stub而非shims

引入Shims的原因：
在为遗留代码写UT时，有时为了隔离依赖就必须重构，而重构是什么呢？Martin老爷子如是说：
Refactoring is a disciplined technique for restructuring an existing body of
code, altering its internal structure without changing its external behavior.
翻译一下：重构是这样一种受过训练的技术，即重新组织代码的现有内容，更改它的内部结构，但是不改变它的外部行为。那么问题来了, 怎么保证我的改动不会改变它的外部行为呢?答案是UT。
可是，我现在还没有UT呀，我此刻要重构就是为了写UT呀！这时，我们就进入了一个矛盾的状态：UT需要重构来隔离依赖，而重构又需要UT做保证。Bingo！鸡生蛋，蛋生鸡的问题顺利发生。
如果你对重构非常有信心，或者写测试的时候能够用stub很好的隔离依赖，那么就不存在该问题，也就没有必要使用shims。使用shims后，我们就可以不重构的情况下隔离依赖，甚至能够隔离比如静态成员，实例成员之类的代码，从而写UT更方便，但是使用Shims写完UT后是否就万事大吉了呢？答案是否定的，请记住，shims时魔鬼，它很容易让你写出紧耦合的程序，并不能提高程序的质量。那我们该怎么办呢？很简单，再用shims写完UT之后，我们就有了重构的安全网，这时我们就应该开始对产品代码进行重构，使它变成易于写UT的，然后再用classical即Stub的方式重写UT。


http://peterprovost.org/blog/2012/04/15/visual-studio-11-fakes-part-1/
http://peterprovost.org/blog/2012/04/25/visual-studio-11-fakes-part-2/
http://peterprovost.org/blog/2012/11/29/visual-studio-2012-fakes-part-3/
https://martinfowler.com/articles/mocksArentStubs.html
http://peterprovost.org/blog/2012/05/31/my-take-on-unit-testing-private-methods/


![Fintech技术汇](https://img2020.cnblogs.com/blog/498574/202008/498574-20200801213206265-563825556.jpg)