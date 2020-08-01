>在.Net开发过程中，经常会使用一些没有源码的第三方库，在代码出了问题时，如果怀疑跟该库的内部实现有关，我们该怎么办呢？首先，自然会想到反编译去看看代码或者联系作者，然而，有没有办法让我们在debug时进入这个第三方库，并看看里面在运行时到底发生了什么呢？本文就来介绍三种debug第三方库，希望能够对你有所帮助。

>先介绍一下我们的样例代码，下面这段代码比较简单，主要功能：从text.csv文件中读取每行数据并在控制台显示。其中用到的类CsvConfiguration，CsvReader，CsvDataReader来自第三方库CsvHelper，可以通过Nuget下载，这里假设我们想要调试CsvDataReader类的Read方法。完整工程请参考：https://github.com/DerekLoveCC/Writings/tree/master/Code/DebugWithoutSourceCodeInDotNet

```csharp
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var textReader = new StreamReader(@".\test.csv"))
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false
                };
                var csvReader = new CsvReader(textReader, config);
                var csvDataReader = new CsvDataReader(csvReader);
                while (csvDataReader.Read())
                {
                    for (int i = 0; i < csvDataReader.FieldCount; i++)
                    {
                        Console.Write(csvDataReader.GetString(i) + " ");
                    }
                    Console.WriteLine(" ");
                }
            }

            Console.Read();
        }
    }
```
###方法一 使用dnSpy
>dnSpy构建在ILSpy的基础上，开源免费，不但可以反编译代码而且能够调试，关于dnSpy的更多信息请访问:https://github.com/0xd4d/dnSpy  ，见下面我们一起来看看具体操作。
1. 下载dnSpy，并根据目标程序是64还是32位，打开对应的exe。本例中由于目标程序是32位的，所以打开了32位的dnSpy
2. 用dnSpy打开CsvHelper的dll，并找到CsvDataReader类的Read方法，点击左侧来设置调试断点，如下图所示：
   ![dnspsy set debug point](https://github.com/DerekLoveCC/Writings/raw/master/Article/DebugWithoutSourceCodeInDotNet/image/dnspy_set_debug_point.png)
3. 在菜单栏，选择调试->开始调试，或者F5打开要调试的exe，如下图所示，然后点击确定开始调试
   ![dnspy open exe](https://github.com/DerekLoveCC/Writings/raw/master/Article/DebugWithoutSourceCodeInDotNet/image/dnspy_open_exe.png)
4. 程序会自动在第二步中的断点处停下，接下来的调试工作跟在VS里基本一样了，包括快捷键也是一样的，如下图：
     ![dnspy debug](https://github.com/DerekLoveCC/Writings/raw/master/Article/DebugWithoutSourceCodeInDotNet/image/dnspy_debug.png)

>总结，dnSpy功能很强大，对于.net的系统库也是可以的，从此调试无忧。此外，dnSpy可以附加到已运行的进程上，但是由于JIT的优化，使得这种方式可能无法获得想要的信息。关于编译优化和运行时优化，咱们以后再聊。

###方法二 使用dotPeek + Visual Studio
>Visual Studio就无需介绍了，dotPeek是大名鼎鼎的JetBrains出品的免费工具，可以到：https://www.jetbrains.com/decompiler/ 下载。这种方法的基本思想就是把dotPeek作为VS的Symbol Server，下面是使用方法：
1. 根据目标程序是64还是32位，打开对应的dotPeek，本例是32bit所以打开的是32位dotPeek
2. 用dotPeek打开CsvHelper的dll，然后在工具栏里点击“Start Symbol Server”按钮开启Symbol Server，如下图：
![start symbol Server](https://github.com/DerekLoveCC/Writings/raw/master/Article/DebugWithoutSourceCodeInDotNet/image/dotpeek_start_symbol_server.png)
如果是第一次打开symbol server会弹出下面的配置框，请根据你的情况选择，笔者选择了“All Assemblies”，之后可以在Tools->Options->Symbol Server里修改这一选项
![dotpeek symbol server config](https://github.com/DerekLoveCC/Writings/raw/master/Article/DebugWithoutSourceCodeInDotNet/image/dotpeek_symbolserver_config.png)
3. 现在就开始配置VS了，在VS里通过Tools->Options打开Options配置窗口，在Debugging/General下，取消“Enable Just My Code”， 选中“Suppress JIT optimization on module load”，如下图：
![dotpeek symbol server config](https://github.com/DerekLoveCC/Writings/raw/master/Article/DebugWithoutSourceCodeInDotNet/image/dotpeek_vs_config.png)
4. 设置VS的Symbol Server，回到dotPeek，打开Tools->Options\Symbol Server，拷贝一下地址，然后在Visual Studio的Tools->Options\Debugging\Symbols, 添加一个新的地址，请查看下图：
![dotpeek get symbol server addr](https://github.com/DerekLoveCC/Writings/raw/master/Article/DebugWithoutSourceCodeInDotNet/image/dotpeek_symbol_server_addr.png)
![dotpeek vs config symbol server addr](https://github.com/DerekLoveCC/Writings/raw/master/Article/DebugWithoutSourceCodeInDotNet/image/dotpeek_vs_add_symbol_server.png)
5. 现在，在VS就可以正常地debug了，第一次由于需要生成和加载pdb文件，所有可能慢点。笔者测试效果图如下：
![dotpeek vs config symbol server addr](https://github.com/DerekLoveCC/Writings/raw/master/Article/DebugWithoutSourceCodeInDotNet/image/dotPeek_TestResult.png)
###方法三 Resharper
>Resharper也是JetBrains的付费产品，使用也很方便，请查看下面步骤：

1. 首先用VS启动debug，打开Debug->Windows->Modules, 然后继续debug直到CsvHelper程序集加载
2. 右键点击CsvHelper程序集，选择“Load Symbols with ReSharper Decompiler”，等symbol加载完之后，就可以正常debug了，可以通过F11进入Read方法的内部，下面是一些相关截图
![dotpeek vs config symbol server addr](https://github.com/DerekLoveCC/Writings/raw/master/Article/DebugWithoutSourceCodeInDotNet/image/dotPeek_TestResult.png)


![Fintech技术汇](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Fintech.jpg)