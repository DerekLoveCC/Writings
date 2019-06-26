using System;

namespace WeakEventPattern
{
    public static class GCUtils
    {
        public static void TriggerGC()
        {
            Console.WriteLine("Starting GC.");

            GC.Collect(3);
            GC.WaitForPendingFinalizers();
            GC.Collect(3);

            Console.WriteLine("GC finished.");
        }
    }
}