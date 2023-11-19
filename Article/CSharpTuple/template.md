>元组

元组即Tuple是C#7.0中引入的功能，使用精简的语法将数据元素组合到一个轻量级的数据结构中。

>声明、初始化和访问元组

先看一些例子：
```c#
public void CreateTuple()
{
    //匿名tuple
    (string, double) anonymouseTuple1 = ("apple", 12.1);
    Console.WriteLine($"Fruit: {anonymouseTuple1.Item1}, Price: ${anonymouseTuple1.Item2}");

    var anonymouseTuple = ("apple", 12.1);
    Console.WriteLine($"Fruit: {anonymouseTuple.Item1}, Price: ${anonymouseTupleItem2}");
    
    //命名tuple的方式1-和值一起指定元素的名字
    var namedTuple1 = (Fruit: "apple", Price: 12.1);
    Console.WriteLine($"Fruit: {namedTuple1.Fruit}, Price: ${namedTuple1.Price}");
    Console.WriteLine($"Fruit: {namedTuple1.Item1}, Price: ${namedTuple1.Item2}");

    var fruit = "apple";
    var price = 12.1;
    var namedTuple1_1 = (fruit, price);
    Console.WriteLine($"Fruit: {namedTuple1_1.fruit}, Price: ${namedTuple1_1.price}");
    Console.WriteLine($"Fruit: {namedTuple1_1.Item1}, Price: ${namedTuple1_1.Item2}");

    //命名tuple的方式2-在变量的声明中同时指定元素的类型和名字
    (string Fruit, double Price) namedTuple2 = ("apple", 12.1);
    Console.WriteLine($"Fruit: {namedTuple2.Fruit}, Price: ${namedTuple2.Price}");
    Console.WriteLine($"Fruit: {namedTuple2.Item1}, Price: ${namedTuple2.Item2}");

    //命名tuple-当同时使用方式1和方式2，方式2优先级更高
    (string Fruit1, double Price1) namedTuple3 = (Fruit: "apple", Price: 12.1);
    Console.WriteLine($"Fruit: {namedTuple3.Fruit1}, Price: ${namedTuple3.Price1}");
    Console.WriteLine($"Fruit: {namedTuple3.Item1}, Price: ${namedTuple3.Item2}");


    (string, double) namedTuple4 = (Fruit: "apple", Price: 12.1);
    //Console.WriteLine($"Fruit: {namedTuple1_1.Fruit}, Price: ${namedTuple1_1.Price}");//编译错误
    Console.WriteLine($"Fruit: {namedTuple4.Item1}, Price: ${namedTuple4.Item2}");
}
```
从上面的例子中我们可以看到，在创建元组时，我们可以指定元组里元素的类型和名字，其中名字可以省略，而类型也可以通过使用var来让编译器进行自动推断。当不指定元素的名字时，只能通过默认名：Item1，Item2，... ,Itemn来访问元组的元素，当制定了元素的名称时，既可以通过Item1，Item2，... ,Itemn访问，也可以通过指定的名字访问。
指定元组元素名称的方式有三种：
1. 在其初始化表达式中，和值一起指定，如上面例子中的namedTuple1
2. 在变量的声明中同时指定元素的类型和名字，如上面例子中的namedTuple2
3. 通过变量的名字推断，如上面例子中的namedTuple1_1，会使用变量的名字fruit和price作为相应元组元素的名字

当同时使用1，2两种方式指定元素的名字时，方式2优先级高，如例子中的namedTuple3和namedTuple4， 其中namedTuple3需要使用Fruit1和Price1访问，而namedTuple4由于在声明中只指定了类型，没有指定名字，所以只能使用Item1，Item2访问。

需要指出的是，在编译的时候，编译器会将非默认名替换成默认名即Item1，Item2等，并删除非默认名，所以非默认名在运行时是不存在的。

Tuple是值类型，所有的元素都是public的，个数可以很多,且它们的值是可以改变的。

从C#12.0开始，可以为Tuple定义别名，例子如下：
```c#
global using Fruit = (string Name, double Price);
```

>使用场景

1. 最常见的使用场景是作为函数的返回值。在之前，为了从函数中返回多个值，要么使用class或者struct，要么使用out或ref，现在元组提供了一种更加方便的方法。
2. 在某些情况下取代匿名类，如在Linq查询中

>元组的比较

元组支持相等(==)和不相等(!=)运算符，以==为例，其逻辑是以短路的方式挨个比较相同位置的元素，如果发现不相同则立即停止比较并返回false，如果所有元素都相等则返回true。此外，只有满足以下两个条件的元组之间才可以比较，否则无法编译：
1. 元素的个数必须相同
2. 相同位置上的两个元素之间可以比较

举例如下：
```c#
public void CompareTest()
{
    var t1 = (Name: "apple", Price: 12.1, Weight: 100);
    var t2 = (Name: "apple", Price: 12.1);
    var t3 = (Name: "apple", Price: (12.1, 1));
    var t4 = (Name: "apple", Price: 12.1);
    var t5 = (Name: "apple1", Price: 12.11);
    var t6 = ("apple", 12.1);
    var t7 = ("apple1", 12.1);
    var t8 = (Name1: "apple", Price1: 12.1);

    //var r1 = t1 == t2;//由于元素个数不同，无法编译
    //var r2 = t2 == t3;//虽然元素个数相同，但第二个元素即Price无法比较，所以无法编译
    
    var r3 = t2 == t4;//true
    var r4 = t2 == t6;//true

    var r5 = t2 == t5;//false
    var r6 = t2 == t7;//false

    var r7 = t2 == t8;//true
}
```

从上面的例子中，我们还可以看到元素的名字在比较时无关紧要。

>Tuple Vs System.Tuple

1. 本文中的Tuple对应的类型是System.ValueTuple，是值类型，而System.Tuple是引用类型
2. System.ValueTuple是可变的，System.Tuple是不可变的
3. System.ValueTuple的成员是字段（Field），System.Tuple的成员是属性

>元组的赋值

元组之间可以赋值，但需要满足如下条件：
1. 元组的元素个数相同
2. 相同位置的元素类型相同，或者右操作数的类型可以隐式转为对应左操作符的类型

其基本逻辑是按照元组中元素的顺序挨个赋值，而元素的名字会被忽略，下面看一个例子：

```c#
(string Name, double Price, int Weight) t1 = ("apple", 12.1, 10);
(string Name1, double Price1, double Weight1) t2 = t1;
Console.WriteLine($"{t2.Name1},{t2.Price1},{t2.Weight1}");
```

>元组解构

可以通过元组的解构将其元素分配给不同的变量，下面是以下例子：
```c#

public void TupleDeconstruct()
{
    var t1 = ("apple", 12.1, 10);
    //使用var
    var (name, price, weight) = t1;
    Console.WriteLine($"{name}, ${price}, {weight}");
    //指定每个元素的类型
    (string name1, double price1, doubleweight1) = t1;
    Console.WriteLine($"{name1}, ${price1}, {weight1}");
    //混合使用var和具体类型
    (var name2, double price2, var weight2) = t1;
    Console.WriteLine($"{name2}, ${price2}, {weight2}");
    //使用已存在的变量
    string name3;
    double price3;
    double weight3;
    (name3, price3, weight3) = t1;
    Console.WriteLine($"{name3}, ${price3}, {weight3}");

    //使用弃员，忽略不想要的元素
    (string name4, double price4, _) = t1;
    Console.WriteLine($"{name4}, ${price4}");
}

```



<center/>

![Fintech技术汇](https://img2020.cnblogs.com/blog/498574/202008/498574-20200801213206265-563825556.jpg)
