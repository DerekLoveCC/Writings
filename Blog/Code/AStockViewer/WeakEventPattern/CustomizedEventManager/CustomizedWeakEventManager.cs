using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WeakEventPattern.CustomizedEventManager
{
    public class CustomizedWeakEventManager : WeakEventManager
    {
        private static CustomizedWeakEventManager CurrentManager
        {
            get
            {
                CustomizedWeakEventManager manager = (CustomizedWeakEventManager)GetCurrentManager(typeof(CustomizedWeakEventManager));

                if (manager == null)
                {
                    manager = new CustomizedWeakEventManager();
                    SetCurrentManager(typeof(CustomizedWeakEventManager), manager);
                }

                return manager;
            }
        }


        public static void AddListener(CustomEventSource source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedAddListener(source, listener);
        }

        public static void RemoveListener(CustomEventSource source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        protected override void StartListening(object source)
        {
            (source as CustomEventSource).CustomEvent += DeliverEvent;
        }

        protected override void StopListening(object source)
        {
            (source as CustomEventSource).CustomEvent -= DeliverEvent;
        }
    }
}
