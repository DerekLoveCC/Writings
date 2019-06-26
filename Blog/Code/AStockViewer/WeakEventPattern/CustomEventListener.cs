using System;

namespace WeakEventPattern
{
    public class CustomEventListener
    {
        public void HandleEvent(object sender, CustomEventArg arg)
        {
            Console.WriteLine($"In evnet handler, received message:[{arg.Message}]");
        }

        ~CustomEventListener()
        {
            Console.WriteLine($"{nameof(CustomEventListener)} is collected");
        }
    }
}