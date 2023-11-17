>表达式体成员(Expression Bodied Member)

表达式体成员是C#6.0中引入且在后续版本中不断加强的功能，它能够以一种更加简洁、可读的方式来实现类的成员，其语法如下：
```c#
member => expression;
```
expression：任何合法的表达式

>可以使用表达式体的类成员
1. 方法
2. 只读属性
3. 属性
4. 构造方法
5. 终结器
6. 索引


下面举例说明：
>表达式体方法

对于有返回值的表达式体方法，其由单个表达式构成，该表达式的值的类型要与方法的返回值类型相匹配，即表达式值的类型与方法的返回值类型相同或者能够进行隐式转换。对于没有返回值的表达式方法，其方法体为一条执行操作的语句，如调用Console.WriteLine。下面是相关的例子：
```c#
private int _age;
private int GetAge() => _age;

public void PrintName() => Console.WriteLine(LastName);
```
>表达式体只读属性

通过以下语法使用表达式体定义只读属性
```c#
PropertyType PropertyName => expression;
```
举例如下：
```c#
public class ExpressionBody
{
    public string LastName => "John";
}
```
>表达式体属性

属性的get和set都可以用表达式体，举例如下：
```c#
public class ExpressionBody
{
    private string firstName;
    public string FirstName
    {
        get => firstName;
        set => firstName = value;
    }
}
```
>表达式体构造函数

使用表达式体的构造函数，通常只包含一个复制语句或者一个方法调用，例子如下：
```c#
public class ExpressionBody
{
    public ExpressionBody(string name) => FirstName= name;
    public ExpressionBody() => Init();
    private void Init()
    {
        Console.WriteLine("in init method");
    }
}
```
>表达式体终结器

类的终结器用于释放一些资源，如非托管资源等，使用表达式体的例子如下：
```c#
public class ExpressionBody
{
    public ExpressionBody(string name) => FirstName = name;
    ~ExpressionBody() => FirstName = null;
}
```
>表达式体索引器

和属性一样，索引器的get和set都可以使用表达式体，例子如下：
```c#
 public class ExpressionBody
{
    private string[] Names = new[] { "Alice", "Bob","Chris", "David", "Eric" };
    public string this[int no]
    {
        get => Names[no];
        set => Names[no] = value;
    }
}
```

<center/>

![Fintech技术汇](https://img2020.cnblogs.com/blog/498574/202008/498574-20200801213206265-563825556.jpg)
