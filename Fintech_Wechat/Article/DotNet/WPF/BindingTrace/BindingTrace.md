微信公众号：**[Fintech极客](#jump_fintech)**
作者为软件开发工程师，就职于金融信息科技类公司，通过CFA一级，分享计算机和金融相结合领域的技术和知识。

#WPF中的Binding Trace
>在WPF中，关于Binding的问题，由于不能直接调试跟踪，所以有时候很难fix，但是Binding支持log输出，通过相关的log信息，我们可以解决绝大部分问题。下面我看看如何使用它。
###在XAML中添加Binding跟踪
>添加命名空间diag
```python
<Window x:Class="WpfApplication.Views.ItemsControlWindow"
        xmlns:diag="clr-namespace:System.Diagnostics;assembly=WindowsBase"
```
>在Binding中设置Trace Level
```python
 <ItemsControl x:Name="myItemsControl"
               Grid.Row="2"
               Margin="10"
               IsTextSearchEnabled="True"
               ItemsSource="{Binding MyTodoList, diag:PresentationTraceSources.TraceLevel=High}">
```
>在ItemsSource的Binding里，我们设置了diag:PresentationTraceSources.TraceLevel=High， High表示输出详细信息，具体的可用值如下：
1. High	3	Traces all additional information.
2. Low	1	Traces some additional information.
3. Medium	2	Traces a medium amount of additional information.
4. None	0	
Traces no available additional information.
###调试运行并查看输出信息
>使用visual studio调试运行，并打开output窗口可以看到如下图所示的信息，这些信息清楚的记录了Binding的整个过程，对解决Binding问题很有帮助
![WPF Binding](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Article/DotNet/WPF/BindingTrace/BindingTraceOutput.png)

<a id="jump_fintech"></a>
![Fintech极客](https://github.com/DerekLoveCC/Writings/raw/master/Fintech_Wechat/Fintech.jpg)