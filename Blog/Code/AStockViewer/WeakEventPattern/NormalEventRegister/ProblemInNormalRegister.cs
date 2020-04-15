using System;

namespace WeakEventPattern.NormalEventRegister
{
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

            //trigger gc but the listener object will NOT collected.
            GCUtils.TriggerGC();

            //trigger event and listener.HandleEvent stil will be executed
            eventSource.Raise("Second Message from CustomEventSource");

            Console.Read();
        }
    }
}