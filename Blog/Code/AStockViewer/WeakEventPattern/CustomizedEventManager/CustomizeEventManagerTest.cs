using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeakEventPattern.CustomizedEventManager
{
    public class CustomizeEventManagerTest
    {
        public static void Test()
        {
            var eventSource = new CustomEventSource();
            var listener = new CustomWeakEventListener();

            //add listener for eventSource via CustomizedWeakEventManager
            CustomizedWeakEventManager.AddListener(eventSource, listener);

            //trigger event and listener.ReceiveWeakEvent will be executed
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
