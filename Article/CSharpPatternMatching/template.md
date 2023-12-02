>什么是模式匹配(Pattern Matching)？

模式匹配是在C#7.0中引入的功能，在后续的版本8，9，10，11中不断增强。我们可以使用它来判断一个表达式的值是否具有某些特征，并在满足时执行一些操作，其优点是：语法精简，表现力强，提高了代码的可读性。

>如何使用？

目前可以在以下三种构造中使用模式匹配
1. is表达式
2. switch表达式
3. switch语句

>模式的类型

目前模式可以分为:type pattern, declaration pattern, constant pattern，relational pattern，property pattern，list pattern，var pattern，discard pattern，logical pattern，positional pattern十类。

1. type和declaration pattern
   

通常Type pattern和declaration pattern一起使用来检查表达式的运行时类型，如果其运行时类型和目标类型匹配，那么将表达式的值赋值给一个新声明的变量。

当表达式的值非空且满足下面的任何一个条件时，则认为类型匹配：
* 表达式值的运行时类型与目标类型一样
``` c# 
public static void TypeEqual(object obj)
{
  if (obj is string isStr)
  {
      Console.WriteLine("is a string:" + isStr);
  }
}
TypeEqual("hello world")
```

* 表达式值的运行时类型是目标类型的子类
```c#
public static void Inherit(object obj)
{
   if (obj is TextReader textReader)
   {
      Console.WriteLine($"it is a text reader, real type is {textReader.GetType()}");
   }
}
Inherit(new StringReader("this is a string to read"));
```

* 当目标类型是接口时，表达式值的运行时类型实现了该接口
```c#
public static void ImplementsInterface(object obj)
{
  if (obj is IEnumerable<char> charStr)
  {
      Console.WriteLine($"{charStr}'s type implements IEnumerable<char> interface");
  }
}
ImplementsInterface("a string")
```

* 表达式值的运行时类型可隐式引用转化为目标类型，关于隐式引用转化，请参考：https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/conversions#1028-implicit-reference-conversions
* 对于可空值类型，目标类型为其底层类型
```c#
public static void NullableValueType<T>(T nullableInt){
  if (nullableInt is int intVal)
  {
      Console.WriteLine($"Value of int? is : {intVal}");
  }
  if (nullableInt is double doubleVal)
  {
      Console.WriteLine($"Value of double? is : {doubleVal}");
  }
}
NullableValueType((int?)10);
NullableValueType((double?)10.1);
```

* 表达式的运行时类型可以装箱，拆箱为目标类型
```c#
public static void BoxedValue(object t)
{
  if (t is int unboxedInt)
  {
      Console.WriteLine($"Unboxed int value : {unboxedInt}");
  }
  if (t is double unboxedDouble)
  {
      Console.WriteLine($"Unboxed double value : {unboxedDouble}");
  }
}
BoxedValue(10);
BoxedValue(10.0d);
```

2. 常量模式（constant pattern）
   
   测试一个表达式的值是否等于指定的常量，常量可以是字符/字符串，整数数字，浮点数数字等字面常量，也可以是null，枚举或者const常量，总之需要是在编译时就确定的量。此外，除了Span<char>和ReadOnlySpan<char>可以和常量字符串比较外，其他的表达式值必须能够转化为对应常量的类型。
```c#
public static void Test(object obj)
{
  if (obj is null)
  {
    Console.WriteLine("obj is null");
  }
  if (obj is "hello")
  {
    Console.WriteLine("obj is string constant: hello");
  }
  if (obj is 'h')
  {
    Console.WriteLine("obj is char: h");
  }
  if (obj is 1)
  {
    Console.WriteLine("obj is int const: 1");
  }
  if (obj is 1.0d)
  {
    Console.WriteLine("obj is double const: 1.0");
  }
}
Test(null);
Test("hello");
Test('h');
Test(1);
Test(1.0);
```

3. 关系模式（relational pattern）
   
   将表达式的值与指定常量进行比较，即使用关系运算符>,<,>=,<=将表达式的值与一个常量或者常量表达式相比较。如果表达式的值是null或者无法通过拆箱转换，可空转换为目标常量的类型，那么认为模式不匹配。
```c#
public static void Test(int salary)
{
   var result = salary switch
   {
      < 10000 => "staff",
      >= 10000 and < 100000 => "manager",
      >= 100000 => "CEO"
   };
   Console.WriteLine(result);
}
Test(1000);
Test(99999);
Test(100000);
```
4. 逻辑模式（logical pattern）
   
   使用模式连接符：and, or, not对其他pattern进行组合. not表示否定，即如果原模式匹配，则连接后的模式不匹配，原模式不匹配，则连接后的模式匹配。or表示所连接的任意一个模式匹配，则匹配。and表示所连接的模式都匹配，才匹配。
```c#
public static void Test(int score)
{
    var r = score switch
    {
        < 0 or > 100 => "invalid score",
        >= 0 and < 60 => "bad",
        >= 60 and < 80 => "good",
        >= 80 and <= 100 => "great"
    };
    Console.WriteLine(r);
 }
 ```
  它们的的优先级从高到底为：not > and > or。只有在is表达式中，它们的优先级大于逻辑运算符，其他情况下其优先级小于逻辑运算符||，！，&&，|，&。
  
  需要注意的是，运行时先校验哪个pattern是不确定的，所以没有逻辑运算符中的短路逻辑。

5. 属性模式（property pattern）

   把表达式值的属性或字段与嵌套的模式进行比较，嵌套模式指出现在其他模式中的模式，比如出现在property模式中的relational模式，即模式里的模式。

   当表达式的值非空，且与属性或字段对应的每个嵌套模式都match时，property模式才算match。下面看一个微软官网上的例子：
```c#
public static string TakeFive(object input) => input switch
{
    string { Length: >= 5 } s => s.Substring(0, 5),
    string s => s,
    ICollection<char> { Count: >= 5 } symbols => new string(symbols.Take(5).ToArray()),
    ICollection<char> symbols => new string(symbols.ToArray()),
    null => throw new ArgumentNullException(nameof(input)),
    _ => throw new ArgumentException("Not supported input type."),
};
Console.WriteLine(TakeFive("Hello, world!"));  // output: Hello
Console.WriteLine(TakeFive("Hi!"));  // output: Hi!
Console.WriteLine(TakeFive(new[] { '1', '2', '3', '4', '5', '6', '7' }));  // output: 45
Console.WriteLine(TakeFive(new[] { 'a', 'b', 'c' }));  // output: abc
```
在上面的switch表达式例子中，同时使用了类型，声明和属性模式，如：string { Length: >= 5 } s => s.Substring(0, 5)表示当input的类型是string且长度大于5时，将其值赋给s并返回s的前5个字符。

property模式是递归模式，所以我们可以使用任何模式作为其嵌套模式，如下面的例子使用了property模式作为另一个property pattern的嵌套模式：
```c#
public record Point(int X, int Y);
public record Segment(Point Start, Point End);

static bool IsAnyEndOnXAxis(Segment segment) => segment is { Start: { Y: 0 } } or { End: { Y: 0 } };
static bool IsAnyEndOnXAxis(Segment segment) => segment is { Start.Y: 0 } or { End.Y: 0 };//在c#10及之后的版本，可以使用扩展的属性模式，语法更精简
```

6. 位置模式（positional pattern）
   
   用于解构表达式的值，然后把结果与相应的嵌套模式进行匹配，表达式值的类型需要支持解构操作，元组和record本身已经支持解构，所以可以直接使用位置模式。
   
   位置模式的形式有点像元组，它使用圆括号包围一些其他的嵌套模式如：常量模式，关系模式
```c#
public static void Test(object obj)
{
    var result = obj switch
    {
        (1, 2) => "number: 1 and 2",
        ("x", "y") => "string: x and y",
        (2, > 5) => "first number is 2 and second number > 5",
        _ => "unsupported"
    }
    Console.WriteLine(result);
}
Test((1, 2));//输出"number: 1 and 2"
Test(("x", "y"));//输出string: x and y
Test((2, 6));//输出first number is 2 and second number > 5
Test("notsupported");//输出unsupported

7. 变量模式（var pattern）

var模式匹配任何表达式，包括null，并把值赋给一个新声明的变量。var模式可以在switch表达式或语句中，可以和when一起使用。微软官网上的例子如下：
```c#
public record Point(int X, int Y);

static Point Transform(Point point) => point switch
{
    var (x, y) when x < y => new Point(-x, y),
    var (x, y) when x > y => new Point(x, -y),
    var (x, y) => new Point(x, y),
};

static void TestTransform()
{
    Console.WriteLine(Transform(new Point(1, 2)));  // output: Point { X = -1, Y = 2 }
    Console.WriteLine(Transform(new Point(5, 2)));  // output: Point { X = 5, Y = -2 }
}
```
8.弃元模式（discard pattern）

和变量模式一样，弃元模式能够匹配任何表达式，包括null，不同的地方在于弃元模式不会把值赋给一个新的变量。比如下面的例子中，如果level大于等于1000，那么将匹配弃元“_”并打印出super。
```c#
public static void Test(int level)
{
    var result = level switch
    {
        < 0 => "invalid",
        >= 0 and < 100 => "normal",
        >= 100 and < 1000 => "excel",
        _ => "super"
    };
    Console.WriteLine(result);
}

Test(-1);//输出invalid
Test(11);//输出normal
Test(200);//输出excel
Test(1000);//输出super
```

9.列表模式（list pattern）

>常见的使用场景

1. 空检查
2. 类型测试
3. 比较离散的值




* https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching

* https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/patterns

<center/>

![Fintech技术汇](https://img2020.cnblogs.com/blog/498574/202008/498574-20200801213206265-563825556.jpg)
