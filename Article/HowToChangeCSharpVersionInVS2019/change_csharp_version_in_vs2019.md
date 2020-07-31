对于VS2019之前的版本，可以在项目属性里方便地修改C#语言的版本，具体细节请参考: [如何在Visual studio中修改所使用C#语言的版本](https://www.cnblogs.com/dereklovecc/p/4649721.html)， 但是在VS2019中，这一修改选项被禁止了，如下图所示：
![](https://img2018.cnblogs.com/blog/498574/202002/498574-20200228220846147-150478008.png)

根据图中链接网站里的提示，可以通过项目的工程文件来修改版本，具体如下图：
![](https://img2018.cnblogs.com/blog/498574/202002/498574-20200228221042295-494441689.png)

亲测好用。另附C#各个版本如下：

|版本|解释|
|----|---|
|preview    |编译器接受最新预览版中的所有有效语言语法|
|latest     |编译器接受最新发布的编译器版本（包括次要版本）中的语法|
|latestMajor|编译器接受最新发布的编译器主要版本中的语法，该选项是默认选项|
|8.0        |C# 8.0 或更低版本中所含的语法|
|7.3        |编译器只接受 C# 7.3 或更低版本中所含的语法|
|7.2        |编译器只接受 C# 7.2 或更低版本中所含的语法|
|7.1        |编译器只接受 C# 7.1 或更低版本中所含的语法|
|7          |编译器只接受 C# 7.0 或更低版本中所含的语法|
|6          |编译器只接受 C# 6.0 或更低版本中所含的语法|
|5          |编译器只接受 C# 5.0 或更低版本中所含的语法|
|4          |编译器只接受 C# 4.0 或更低版本中所含的语法|
|3          |编译器只接受 C# 3.0 或更低版本中所含的语法|
|ISO-2      |编译器只接受 ISO/IEC 23270:2006 C# (2.0) 中所含的语法|
|ISO-1      |编译器只接受 ISO/IEC 23270:2003 C# (1.0/1.2) 中所含的语法|