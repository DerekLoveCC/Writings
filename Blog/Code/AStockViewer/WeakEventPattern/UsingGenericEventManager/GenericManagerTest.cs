using System;
using System.Windows;

namespace WeakEventPattern.UsingGenericEventManager
{
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

            //trigger gc and the listener object will be collected.
            GCUtils.TriggerGC();

            //trigger event and listener.HandleEvent stil will NOT be executed
            eventSource.Raise("Second Message");

            Console.Read();
        }
    }
}