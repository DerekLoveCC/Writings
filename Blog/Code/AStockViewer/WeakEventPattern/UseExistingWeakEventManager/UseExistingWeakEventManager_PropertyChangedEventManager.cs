using System;
using System.ComponentModel;
using System.Windows;

namespace WeakEventPattern.UseExistingWeakEventManager
{
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
}

namespace WeakEventPattern.UseExistingWeakEventManager
{
    public class PropertyChangedEventSource : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }
    }

    public class PropertyChangedEventEventListener : IWeakEventListener
    {
        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            HandledEventArgs(managerType, sender, e);

            return true;
        }

        private void HandledEventArgs(Type managerType, object sender, EventArgs e)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"In event Handler. Message:[{(sender as PropertyChangedEventSource).Name}]");
            Console.ForegroundColor = originalColor;
        }

        ~PropertyChangedEventEventListener()
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"{nameof(PropertyChangedEventEventListener)} is garbage collected.");
            Console.ForegroundColor = originalColor;
        }
    }
}