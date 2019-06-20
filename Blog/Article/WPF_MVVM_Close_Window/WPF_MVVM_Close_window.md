###问题
>在WPF程序中经常需要用弹出窗口的形式向用户确认操作，用户阅读完相关信息后，点击上面的OK，Cancel按钮关闭窗口，再进行后续操作。当使用MVVM设计模式时，OK和Cancel会binding到ViewModel中的相关属性上，因而在ViewModel中，我们希望在用户点击了Ok或Cancel之后关闭这个弹窗并返回用户的选择结果，但是由于Window类本身所带的属性DialogResult不是一个Dependency Proerty，我们也就不能把它Binding到一个ViewModel属性上，再通过设置该属性来关闭窗口。那么我们该如何在ViewModel关闭该窗口呢？
###解决方案
>这里所讲的一种解决方案是使用Attached Property。如下所示我们创建了一个静态类，它包含一个Attached Property。
```C#
    public static class DialogCloser
    {
        public static readonly DependencyProperty DialogResultProperty = 
            DependencyProperty.RegisterAttached("DialogResult",
                                                typeof(bool?),
                                                typeof(DialogCloser),
                                                new PropertyMetadata(DialogResultChanged));

        private static void DialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window != null)
            {
                window.DialogResult = e.NewValue as bool?;
            }
        }

        public static void SetDialogResult(Window target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);
        }
    }
```
>在使用时，只需要把这个属性Attach到相关窗口上并Binding到后台ViewModel的属性上，下面是一个例子，完整的例子请参见https://github.com/DerekLoveCC/Writings/tree/master/Blog/Code/AStockViewer：

