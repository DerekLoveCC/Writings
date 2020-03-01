>微信公众号：**[Fintech极客](#jump_fintech)**
作者为软件开发工程师，就职于金融信息科技类公司，通过CFA一级，分享计算机和金融相结合领域的技术和知识。

#C# 8.0中的新功能
>C# 8.0已经推出来好长一段时间了， 由于公司目前主要使用的还是6.0版本，加上之前个人事情较多，一直没有总结，今天主要查看和测试微软官方文档中的内容：https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8

##只读成员（Readonly members）
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

##默认接口方法（Default interface methods）
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
>在NormalCustomer_TestGetName函数中，被注释掉的那行显示出，实现类NormalCustomer的引用是无法调用在接口ICustomer中实现的函数GetName的，只能通过ICustomer的引用调用GetName。然而，FintechCustomer_TestGetName中，我们可以看到无论是FintechCustomer还是ICustomer的引用都可以调用GetName函数，并且调用的是同一个函数。

<a id="jump_fintech"></a>
![Fintech极客](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Fintech.jpg)