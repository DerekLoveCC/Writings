using System;

namespace WeakEventPattern
{
    public class CustomEventSource
    {
        public event Action<object, CustomEventArg> CustomEvent;

        public void Raise(string msg)
        {
            CustomEvent?.Invoke(this, new CustomEventArg
            {
                Message = msg,
            });
        }
    }
}