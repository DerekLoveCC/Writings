## Visual Studio中，你应该知道的几个Immediate Window小技巧

> 作为.NET老司机的你，一定在Debug的时候使用过VS的Immediate Window，但是，你是否了解它的以下功能呢？如果是.NET新手，莫慌，它非常简单易用，Debug并动手实践一下，就可立马掌握。下面我们一起看看吧:

> 首先，通过Debug->Windows->Immediate或者快捷键Ctrl+D,I打开Immediate窗口，如下图所示:
![](https://img2020.cnblogs.com/blog/498574/202008/498574-20200805000654517-1848132137.png)

> 接下来一起看看它的几个使用方法
### 1. 访问非公有成员，执行语句如Linq
>这是Immediate Window的基本功能，也是最通常的用法，在debug的时候，我们可以在immediate window里执行诸如访问属性/域，执行Linq等的操作：
![](https://img2020.cnblogs.com/blog/498574/202008/498574-20200805000714996-1632476202.png)

### 2. 访问异常和返回值

> 在deug的时候，你有没有遇到过类似下面的代码，第一部分catch住了异常，但啥也没干，第二部分代码调用的函数有返回值，但是并未声明本地变量去接收返回值。当然这都不是好代码，然而项目大了，啥人都有，有时就能碰到这种代码，假设你在debug时，想知道异常是啥，GetPersonsAgeUnder18返回了啥，咋办？答案请看截图：

``` csharp
    try
    {
        MethodWhichThrowException();
    }
    catch
    {
    }
```
``` csharp
    GetPersonsAgeUnder18(persons);
```
![](https://img2020.cnblogs.com/blog/498574/202008/498574-20200805000751260-1179232676.png)
![](https://img2020.cnblogs.com/blog/498574/202008/498574-20200805000800569-531904576.png)

### 3. 通过ID访问对象

> 有时候为了追踪一个对象，我们会给该对象生成一个Id，然后无论在什么上下文里，我们都可以通过这个Id来访问该对象，注意：这个Id并不会阻碍对象的垃圾收集。请看下面的例子：
![](https://img2020.cnblogs.com/blog/498574/202008/498574-20200805000824168-1652675365.png)

### 4. 重新调用函数

> 一个函数调用完了，如果想重新进入函数体debug怎么办，一种方法是有鼠标拖过去，另一种就是使用Immediate Window直接调用，这里假设GetPersonsAgeUnder18以及调用完了，但是想debug看看它内部的实现，怎么做？首先，再GetPersonsAgeUnder18中下一个断点，然后再Immediate Window中调用GetPersonsAgeUnder18， VS会自动在断点处停下，如下所示
![](https://img2020.cnblogs.com/blog/498574/202008/498574-20200805001053683-2046152193.png)

### 5. 消除边界效应

> 通过上面的例子，我们可以看出在Immediate Window中调用函数，与执行正常代码中调用并无区别，所以如果函数改变了类的状态，那么你在Immediate Window中调用该函数，同样会改变类的状态，那么有没有办法让Immediate Window中执行的函数不改变类的状态呢？方法就是在函数调用后面加上nse，即no side effective, 请看下面的例子：
![](https://img2020.cnblogs.com/blog/498574/202008/498574-20200805000853254-544264391.png)


> 需要说明的是，nse并不是对所有的函数都有效，只是对一些简单的函数有效。完整代码请访问：https://github.com/DerekLoveCC/Writings/tree/master/Article/VSImmediateWindow/code/ImmediateWindowDemo


![Fintech技术汇](https://img2020.cnblogs.com/blog/498574/202008/498574-20200801213206265-563825556.jpg)