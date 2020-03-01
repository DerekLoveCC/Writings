>微信公众号：**[Fintech极客](#jump_fintech)**
作者为软件开发工程师，就职于金融信息科技类公司，通过CFA一级，分享计算机和金融相结合领域的技术和知识。

#C# 8.0中的新功能
>C# 8.0已经推出来好长一段时间了， 由于公司目前主要使用的还是6.0版本，加上之前个人事情较多，一直没有总结，今天主要查看和测试微软官方文档中的内容：https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8

###只读成员（Readonly members）
>在struct前面加上readonly，表示该struct是不可变的，它里面的所有属性都必须是只读的。只读成员使得控制更加精细，也就是说程序员可以指明把哪个成员设置成只读的。下面是一段代码示例：
```java
    public struct ReadonlyMembers
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Distance => Math.Sqrt(X * X + Y * Y + Z * Z);

        public readonly override string ToString() => $"({X}, {Y},{Z}) is {Distance} from the origin";

        public readonly void TestReadOnly()
        {
            //X = 10;//Complier error: Cannot assign to 'X' because it is read-only
        }
    }
```
>上面的代码片段包含了两个被readonly修饰的函数：ToString和TestReadOnly。在TestReadOnly中，如果试图给X赋值，编译器就会给出注释中的错误信息。
此外，ToString中使用的Distance会导致一个编译警告：CS8656	Call to non-readonly member 'ReadonlyMembers.Distance.get' from a 'readonly' member results in an implicit copy of 'this'. 这是因为编译器并不认为get不会改变struct的状态，所以必须显示的设置Distance为readonly，然而对于自动属性则不需要，编译器会认为其get访问器不会修改状态，所以在ToString中访问X,Y,Z成员不会导致编译警告。

###默认接口方法（Default interface methods）
>在之前的版本中，接口不能包含实现部分，所以一旦在接口中添加新的成员，所有实现该接口的地方都需要修改，而这个新特性使得在添加新成员的时候能给出其实现，从而使得实现接口的地方不需要进行修改。有一点需要注意的是接口的实现并不会像类继承那样继承接口中的实现，所以在调用接口中的实现时，要使用接口类型而不能使用实现类型。注：该功能在 .Net Framewok4.8中不支持，编译错误提示如下图，而在 .Net Core 3.0中是支持的。

![.Net Framework Error](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/DotNet/CSharp8_0/DefaultInterfaceImplNotSupportInDotNetFramework.png)

>下面举一个默认接口实现的例子：
```java
    public interface ICustomer
    {
        string Name { get; set; }

        int OrderCount { get; set; }

        public string GetOrderCount();

        public string GetName()//default implemented method
        {
            return Name;
        }

        protected void PrintName()
        {
            Console.WriteLine("Name:" + Name ?? "");
        }

        private void DummyPrivateMethod()
        {
            Console.WriteLine("Name:" + Name ?? "");
        }
    }

    public class FintechCustomer : ICustomer
    {
        public string Name { get; set; }
        public int OrderCount { get; set; } = 100;

        public string GetOrderCount()
        {
            return $"FintechCustomer order count:{OrderCount}";
        }

        public string GetName()//override default implemented method
        {
            return $"FintechCustomer Name:{Name}";
        }

        public FintechCustomer(string name) => Name = name;

    }

    public class NormalCustomer : ICustomer
    {
        public string Name { get; set; }
        public int OrderCount { get; set; }

        public string GetOrderCount()
        {
            return $"Normal customer order count:{OrderCount}";
        }

        public NormalCustomer(string name)
        {
            Name = name;
            OrderCount = 0;
        }
    }
```
>上述代码中包含了一个接口ICustomer和两个实现类FintechCustomer，NormalCustomer。有以下几点需要注意：
1. ICustomer接口提供了GetName的具体实现
2. FintechCustomer类提供了自己的GetName实现，从而覆盖了ICustomer中的GetName
3. NormalCustomer类没有提供自己的GetName实现，但也没有继承ICustomer中的GetName，通下面的调用示例，我们可以验证这点
4. 在ICustomer接口内部，实现成员可以加各种修饰符如：private, protected, public等，但它们都不会被继承。

>代码调用示例如下：
```Java
    public class DefaultInterfaceImplementationUnitTest
    {
        [TestMethod]
        public void FintechCustomer_TestGetName()
        {
            var name = "FintechCustomer1";
            var fc = new FintechCustomer(name);
            ICustomer ic = fc;
            Assert.AreEqual($"FintechCustomer Name:{name}", fc.GetName());
            Assert.AreEqual($"FintechCustomer Name:{name}", ic.GetName());
        }

        [TestMethod]
        public void NormalCustomer_TestGetName()
        {
            var name = "NormalCustomer1";
            var nc = new NormalCustomer(name);
            ICustomer ic = nc;
            //Assert.AreEqual(name, nc.GetName());//编译错误： Error CS1061  'NormalCustomer' does not contain a definition for 'GetName'...
            Assert.AreEqual(name, ic.GetName());
        }
    }
```
>在NormalCustomer_TestGetName函数中，被注释掉的那行显示出，实现类NormalCustomer的引用是无法调用在接口ICustomer中实现的函数GetName的，只能通过ICustomer的引用调用GetName。然而，FintechCustomer_TestGetName中，我们可以看到无论是FintechCustomer还是ICustomer的引用都可以调用GetName函数，并且调用的都是FintechCustomer中的函数。

###更多的模式匹配
>C# 7.0通过is和switch引入了类型和常量匹配，C# 8.0中扩展了使用它们的地方。这些特性主要是为了支持数据与功能相分离的编程范式，当数据和功能是分开的或者算法并不依赖于运行时对象的类型时，可以考虑使用这些特性，它们提供了另一种表达设计的方式。下面会讲到switch表达式和property，tuple，postition三种模式匹配。

####switch表达式(Switch Expression)
>先看一个例子
```java
        public static RGBColor FromRainbow(Rainbow colorBand) =>
            colorBand switch
            {
                Rainbow.Red => new RGBColor(0xFF, 0x00, 0x00),
                Rainbow.Orange => new RGBColor(0xFF, 0x7F, 0x00),
                Rainbow.Yellow => new RGBColor(0xFF, 0xFF, 0x00),
                Rainbow.Green => new RGBColor(0x00, 0xFF, 0x00),
                Rainbow.Blue => new RGBColor(0x00, 0x00, 0xFF),
                Rainbow.Indigo => new RGBColor(0x4B, 0x00, 0x82),
                Rainbow.Violet => new RGBColor(0x94, 0x00, 0xD3),
                _ => throw new ArgumentException(message: "invalid enum value", paramName: nameof(colorBand)),
            };

        public enum Rainbow
        {
            Red,
            Orange,
            Yellow,
            Green,
            Blue,
            Indigo,
            Violet
        }
```
>从上面例子中的FromRainbow，我们可以看到switch表达式相较于switch语句发生了很大的变化，主要在以下四点：
1. 变量的名字（colorBand）放在了switch关键字前面，这样编译器就能容易switch语句和switch表达式。
2. =>取代了case 和 :并且每一句的结尾用的是,而非;，这样更加的简约和直观。
3. default分支被_取代。
4. 整块代码是一个表达式而非语句。

>最后，switch表达式必须返回一个值或者抛出异常，如果一次调用没有case匹配上，则会抛出InvalidOperation异常，编译器也会在switch分支不包含所有情况的时候给出警告。

####属性模式（Property Pattern）
>下面还是从一个例子讲起
```java
       public static decimal ComputeSalesTax(Address location, decimal salePrice) =>
            location switch
            {
                { State: "WA" } => salePrice * 0.06M,
                { State: "MN" } => salePrice * 0.75M,
                { State: "MI" } => salePrice * 0.05M,
                // other cases removed for brevity...
                _ => 0M
            };

        public class Address
        {
            public string State { get; set; }
        }
```
>上面代码中的ComputeSalesTax函数用到了switch表达式，而switch表达式内部的匹配则用到了属性模式。例如下面的一句表示：如果location的State属性值为"WA"，那么就返回salePrice * 0.06M：
```java
 { State: "WA" } => salePrice * 0.06M,
```

####元组模式(tuple pattern)
>有些算法要求多个输入，而元组模式允许在switch表达式中使用的tuple来进行多个值的匹配，下面是一个例子：
```java
    public static string RockPaperScissors(string first, string second)
    => (first, second) switch
    {
        ("rock", "paper") => "rock is covered by paper. Paper wins.",
        ("rock", "scissors") => "rock breaks scissors. Rock wins.",
        ("paper", "rock") => "paper covers rock. Paper wins.",
        ("paper", "scissors") => "paper is cut by scissors. Scissors wins.",
        ("scissors", "rock") => "scissors is broken by rock. Rock wins.",
        ("scissors", "paper") => "scissors cuts paper. Scissors wins.",
        (_, _) => "tie"
    };
```
>上面是剪刀石头布的游戏，可以看到switch表达式使用了元组还匹配只返回那条消息，其中(_,_)=>"tie"表示未找到任何匹配时，返回"tie"。

####位置模式（Positional patterns）
>C# 7.0引入了Deconstruct函数，如果一个类中还有Deconstrut函数，那么就可以直接把该类的属性分解到不同的变量中。在C# 8.0中，可以通过位置模式在switch表达式中进行多个属性的匹配，下面是一个例子：
```java
        public static Quadrant GetQuadrant(Point point) => point switch
        {
            (0, 0) => Quadrant.Origin,
            var (x, y) when x > 0 && y > 0 => Quadrant.One,
            var (x, y) when x < 0 && y > 0 => Quadrant.Two,
            var (x, y) when x < 0 && y < 0 => Quadrant.Three,
            var (x, y) when x > 0 && y < 0 => Quadrant.Four,
            var (_, _) => Quadrant.OnBorder,
            _ => Quadrant.Unknown
        };

        public class Point
        {
            public int X { get; }
            public int Y { get; }

            public Point(int x, int y) => (X, Y) = (x, y);

            public void Deconstruct(out int x, out int y) =>
                (x, y) = (X, Y);
        }
```
>本例中，Point类包含了Deconstruct函数，从而可以把它的X,Y分解到单独的变量中。在GetQuadrant函数中，把point对象通过Deconstruct函数分解到x,y两个变量中，并通过when来判断是否满足该分支条件。

###using声明
>在之前的版本中，我们可以使用using语句来保证实现了IDisposable的对象在生命周期结束后，其Dispose方法被调用。在C# 8.0中，我们using声明来做同样的事并且code更加紧凑。下面是一个打开文件的例子：
```java
static int WriteLinesToFile(IEnumerable<string> lines)
{
    using var file = new System.IO.StreamWriter("WriteLines2.txt");
    // Notice how we declare skippedLines after the using statement.
    int skippedLines = 0;
    foreach (string line in lines)
    {
        if (!line.Contains("Second"))
        {
            file.WriteLine(line);
        }
        else
        {
            skippedLines++;
        }
    }
    // Notice how skippedLines is in scope here.
    return skippedLines;
    // file is disposed here
}
```
>上例中，在函数调用返回时会自动调用file对象的Dispose方法，如果using声明的对象没有实现IDisposable，编译器会生成错误信息。

###静态本地函数(Static local functions)
>如果不想本地函数访问即捕获它外面的本地变量，我们可以加上static进行限制，下面是一个例子：
```java
        public int Add(int x, int y)
        {
            return x + y;

            int GetX()
            {
                return x;
            }

            static int GetY()
            {
                return y;//Compiler error:A static local function cannot contain a reference to 'y'
            }
        }
```
>上面的例子中，Add函数包含两个local函数，其中GetY是static的，但是它引用了外围的本地变量y，所以会有注释中的编译错误。

###Disposable ref structs
>因为ref的结构体不能实现任何接口，当然IDisposable也不行，所以它就不能被dispose了。在C# 8.0中，只要它有一个可访问的void Dispose()函数，就可以被Dispose，例子如下：
```Java
    ref struct DisposableRefStruct
    {
        public void Dispose()
        {
            Console.WriteLine("ref struct dispose method is called");
        }
    }

    public class DisposableRefStructTester
    {
        public static void Test()
        {
            using (var s = new DisposableRefStruct())
            {
                Console.WriteLine("Test Disposable Ref Struct");
            }
        }
```
>从例子中可以看出DisposableRefStruct没有实现IDisposable接口，但是有void Dispose()方法，所以也可以用字using语句中。该功能同样适用于readonly ref struct

###可空引用类型（Nullable reference types）
>可以通过在工程文件中加上下面的配置来打开可空引用类型检查。打开之后，编译器会认为所有的引用类型都是不可以赋空值的，必须像之前的可空值类型一样，在类型后面加上？来表示这个引用类型可以为空。具体细节请参考：
https://docs.microsoft.com/en-us/dotnet/csharp/nullable-references
https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/nullable-reference-types
 https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/upgrade-to-nullable-references
```xml
<Nullable>enable</Nullable>
```
###异步流(Asynchronous streams)
>该功能是对应于同步IEnumerable<T>的异步版本，异步流函数具有下面三个特点：
1. 使用async修饰
2. 返回类型为：IAsyncEnumerable<T>
3. 函数包含yield return语句来实现enumerator范式
下面是一个例子：
```java
        public static async IAsyncEnumerable<int> GenerateSequence()
        {
            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(100);
                yield return i;
            }
        }

        public static async Task ConsumeAsyncStream()
        {
            await foreach (var i in GenerateSequence())
            {
                Console.WriteLine(i);
            }
        }
```
>上例中GenerateSequence返回一个异步流而ConsumeAsyncStream通过在foreach前面加一await来枚举该异步流。

###索引和范围（Indices and ranges）
>索引和范围为访问序列中的单个元素或范围提供了简洁的语法。这个概念有点像Python中的切片，只是语法稍有不同。该功能的实现依赖于以下的两个新类型和两个新操作符：
1. System.Index，表示序列中的一个索引
2. ^操作符，表示索引是从后面开始的
3. System.Range，表示序列的一个子序列
4. ...操作符，表示一个range的开始和结束
>一些代码示例如下：
```java
public class IndicesAndRanges
    {
        public string[] words = new string[]
                               {
                                            // index from start      index from end
                                "The",      // 0                     ^9
                                "quick",    // 1                     ^8
                                "brown",    // 2                     ^7
                                "fox",      // 3                     ^6
                                "jumped",   // 4                     ^5
                                "over",     // 5                     ^4
                                "the",      // 6                     ^3
                                "lazy",     // 7                     ^2
                                "dog"       // 8                     ^1
                               };           // 9 (or words.Length)   ^0

        public void Test()
        {
            Console.WriteLine($"The last word is {words[^1]}"); // writes "dog"
            var quickBrownFox = words[1..4];// "quick", "brown", and "fox"
            var lazyDog = words[^2..^0];//"lazy" and "dog"

            var allWords = words[..]; // contains "The" through "dog".
            var firstPhrase = words[..4]; // contains "The" through "fox"
            var lastPhrase = words[6..]; // contains "the", "lazy" and "dog"


            Range phrase = 1..4;//Range phrase = 1..4;
            var text = words[phrase];
        }
    }
```

###Null合并赋值
>C# 8.0引入了null合并赋值运算符??=。只有当左操作数为null时，才能将其右操作数的值赋给左操作数。例子如下：
```java
List<int> numbers = null;
int? i = null;

numbers ??= new List<int>();
numbers.Add(i ??= 17);
numbers.Add(i ??= 20);

Console.WriteLine(string.Join(" ", numbers));  // output: 17 17
Console.WriteLine(i);  // output: 17
```

###非托管构造类型
>在之前的版本中，构造类型（包含至少一个类型参数的类型）不能为非托管类型。从C#8.0开始，如果构造的值类型仅包含非托管类型的字段，则该类型不受管理，例子如下：
```java
public struct Coords<T>
{
    public T X;
    public T Y;
}
```
>它和其他非托管类型一样，可以创建指向它的指针或在stack上分配该类型的内存块。

###嵌套表达式中的 stackalloc
>C# 8.0中，如果stackalloc表达式结果的类型为System.Span<T>或者System.ReadOnlySpan<T>，则可以在其他表达式中使用该stackalloc表达式, 下面是一个具体的例子：
```java
Span<int> numbers = stackalloc[] { 1, 2, 3, 4, 5, 6 };
var ind = numbers.IndexOfAny(stackalloc[] { 2, 4, 6 ,8 });
Console.WriteLine(ind);  // output: 1
```
###字符串插值的增强
>字符串插值中\$和@标记的顺序可以任意放置：\$@"..." 和 @\$"..."都是正确的，而在早期的 C# 版本中，\$标记必须出现在@标记之前。


<a id="jump_fintech"></a>
![Fintech极客](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Fintech.jpg)