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

>总结，dnSpy功能很强大，调试无忧。此外，dnSpy可以附加到已运行的进程上，但是由于JIT的优化，使得这种方式可能无法获得想要的信息。关于编译优化和运行时优化，咱们以后再聊。

###方法二 

###方法三


![Fintech技术汇](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Fintech.jpg)