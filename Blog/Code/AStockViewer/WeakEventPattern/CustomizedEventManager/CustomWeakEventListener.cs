using System;
using System.Windows;

namespace WeakEventPattern.CustomizedEventManager
{
    public class CustomWeakEventListener : IWeakEventListener
    {
        public void HandleEvent(object sender, CustomEventArg arg)
        {
            Console.WriteLine($"In evnet handler, received message:[{arg.Message}]");
        }

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            HandleEvent(sender, e as CustomEventArg);

            return true;
        }

        ~CustomWeakEventListener()
        {
            Console.WriteLine($"{nameof(CustomEventListener)} is collected");
        }
    }
}