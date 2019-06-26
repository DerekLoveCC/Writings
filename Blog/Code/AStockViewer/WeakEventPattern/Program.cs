using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeakEventPattern.CustomizedEventManager;
using WeakEventPattern.NormalEventRegister;
using WeakEventPattern.UseExistingWeakEventManager;
using WeakEventPattern.UsingGenericEventManager;

namespace WeakEventPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            //ProblemInNormalRegister.Test();
            //UseExistingWeakEventManager_PropertyChangedEventManager.Test();
            //GenericManagerTest.Test();
            CustomizeEventManagerTest.Test();
        }
    }
}
