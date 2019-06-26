###问题
>通过传统的方式监听事件（即通过C#的+=语法），有可能会导致内存泄漏，原因是事件源会保持对事>件Handler所在对象的强引用从而阻碍GC回收它，这样事件handler对象的生命周期收到了事件源对象的影响。
###解决方案
>此问题有两个解决办法：1) 确保通过-=语法反注册事件处理器 2)使用弱事件模式（Weak Event Pattern）。本文主要讲解Weak Event Pattern。

>在使用Weak Event Pattern时，主要涉及到两个类：WeakEventManager和IWeakEventListener。WeakEventManager的StartListening和StopListening函数可以被子类覆盖来进行事件监听的注册和反注册。IWeakEventListener只有下面一个函数，当事件触发时，WeakEventManager会调用该函数从而执行Handler的代码。
```C# 
bool ReceiveWeakEvent (Type managerType, object sender, EventArgs e);
```
>根据构造WeakEventManager的方法不同，可分为如下三种方式来应用Weak Event Pattern：
>1. 使用.Net Framework自带的Event Manager或者第三方库中的Event Manager（例如Prism）
>2. 使用泛型类WeakEventManager<EventSource, SomeEventEventArgs>
>3. 自己编写Event Manager

>首先我们通过如下代码来看看传统的事件注册方式有什么问题，代码里通过+=语法注册了listener.HandleEvent方法到eventSource.CustomEvent事件上，在设置listener为null后并触发GC，但listener对象并不会被GC收集且会继续处理新的消息,图一是其运行结果。
```C#
    public static class ProblemInNormalRegister
    {
        public static void Test()
        {
            var eventSource = new CustomEventSource();
            var listener = new CustomEventListener();

            //register event listener
            eventSource.CustomEvent += listener.HandleEvent;

            //trigger event and listener.HandleEvent will be executed
            eventSource.Raise("First Message from CustomEventSource");

            //set listener to null
            listener = null;

            //trigger gc but the listener object will not collected.
            GCUtils.TriggerGC();

            //trigger event and listener.HandleEvent stil will be executed
            eventSource.Raise("Second Message from CustomEventSource");

            Console.Read();
        }
    }
```
![图一]()
>接下来，让我们通过3种不同的方式应用Weak Event Pattern来解决这个问题。

>1. 使用.Net Framework自带的Event Manager
系统自带了一些像CollectionChangedEventManager，PropertyChangedEventManager的Weak Event Manager，每个事件管理器只处理一类事件，比如PropertyChangedEventManager是处理INotifyPropertyChanged.PropertyChanged事件的，完整列表请参考:https://docs.microsoft.com/en-us/dotnet/api/system.windows.weakeventmanager?view=netframework-4.8。 如果你要处理的事件恰好对应某个系统自带的Event Manager，那么你可以直接按照如下的方法拿来使用，其运行结果如图二所示，可以看到listener在被设置成null之后被回收了，从而也就不能在处理新的消息了
```C#
    public class UseExistingWeakEventManager_PropertyChangedEventManager
    {
        public static void Test()
        {
            var source = new PropertyChangedEventSource();
            var listener = new PropertyChangedEventEventListener();

            //Register listener to the source
            PropertyChangedEventManager.AddListener(source, listener, nameof(PropertyChangedEventSource.Name));

            //change property change and the ReceiveWeakEvent of PropertyChangedEventEventListener method will be called
            source.Name = "first name";

            //set listener to null so it will be ready for gc
            listener = null;

            //trigger gc and the listener will be collected.
            GCUtils.TriggerGC();

            //change property value but the ReceiveWeakEvent of PropertyChangedEventEventListener method will NOT be called
            source.Name = "second name"; 

            Console.Read();
        }
    }
```
![图二]()

>2. 使用泛型类WeakEventManager<EventSource, SomeEventEventArgs>
按照如下代码使用.net framework的泛型WeakEventManager，非常方便而且listener在设置为null之后可以被回收，运行结果如图三所示。
```C#
    public class GenericManagerTest
    {
        public static void Test()
        {
            var eventSource = new CustomEventSource();
            var listener = new CustomEventListener();

            //add handler listener.HandleEvent to CustomEvent of CustomEventSource
            WeakEventManager<CustomEventSource, CustomEventArg>.AddHandler(eventSource, "CustomEvent", listener.HandleEvent);

            //trigger event and listener.HandleEvent will be executed
            eventSource.Raise("First message");

            //set listener to null
            listener = null;

            //trigger gc but the listener object will be collected.
            GCUtils.TriggerGC();

            //trigger event and listener.HandleEvent stil will NOT be executed
            eventSource.Raise("Second Message");

            Console.Read();
        }
    }
```
![图三]()

>3. 自己编写Event Manager


####其他资源
1. https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/weak-event-patterns
2. https://www.codeproject.com/articles/738109/the-net-weak-event-pattern-in-csharp
