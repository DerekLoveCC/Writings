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

* 表达式值的运行时类型是目标类型的子类
  ```c#
  public static void Inherit(object obj)
  {
    if (obj is Array array)
    {
        Console.WriteLine($"it is an array and its length is {array.Length}");
    }
  }
  Inherit(new int[] { 1, 2, 3, 4, 5 })

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
  
1. constant pattern
   
   测试一个表达式的值是否等于指定的常量，常量可以是字符/字符串，整数数字，浮点数数字等字面常量，也可以是null，枚举或者const常量，总之需要是在编译时就确定的量。此外，除了Span<char>和ReadOnlySpan<char>可以和常量字符串比较外，其他的表达式值必须能够转化为对应常量的类型。

2. relational pattern
   
   将表达式的值与指定常量进行比较，即使用关系运算符>,<,>=,<=将表达式的值与一个常量或者常量表达式相比较。如果表达式的值是null或者无法通过拆箱转换，可空转换为目标常量的类型，那么认为模式不匹配。

3. logical pattern
   
   使用模式连接符：and, or, not对其他pattern进行组合

not：表示否定，即如果原模式匹配，则连接后的模式不匹配，原模式不匹配，则连接后的模式匹配

or： 表示所连接的任意一个模式匹配，则匹配

and: 表示所连接的模式都匹配，才匹配

它们的的优先级从高到底为：not > and > or。只有在is表达式中，它们的优先级大于逻辑运算符，其他情况下其优先级小于逻辑运算符||，！，&&，|，&。

需要注意的是，运行时先校验哪个pattern是不确定的，所以没有逻辑运算符中的短路逻辑。

5. positional pattern
   
   用于解构表达式的值，然后把结果与相应的模式进行匹配，这些模式被称为嵌套模式，因为它们出现在positional模式里，即模式里的模式。

   

6. property pattern：
7. list pattern:
8. var pattern:
9.  discard pattern:
10. :

>常见的使用场景

1. 空检查
2. 类型测试
3. 比较离散的值




* https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching

* https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/patterns

<center/>

![Fintech技术汇](https://img2020.cnblogs.com/blog/498574/202008/498574-20200801213206265-563825556.jpg)
