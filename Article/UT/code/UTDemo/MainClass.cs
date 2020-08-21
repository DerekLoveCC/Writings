using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Fakes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTDemo
{
    public class MainClass
    {
        public static void Main()
        {
            PlatformThread pt = new PlatformThread();


            var dllPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\CommonExtensions\Microsoft\IntelliTrace";
            Environment.SetEnvironmentVariable("INTELLITRACE_PROFILER_DIRECTORY", @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\CommonExtensions\Microsoft\IntelliTrace");
            Environment.SetEnvironmentVariable("COR_ENABLE_PROFILING", "1");
            Environment.SetEnvironmentVariable("COR_PROFILER", "{ 9317ae81-bcd8-47b7-aaa1-a28062e41c71}");


            var result = NativeMethods.LoadLibraryW(dllPath + "\\Microsoft.IntelliTrace.Profiler.dll");


            using (var s = ShimsContext.Create())
            {
                ShimDateTime.NowGet = () => new DateTime(2020, 10, 10);
                var now = DateTime.Now;
            }

            var now1 = DateTime.Now;
        }
    }
}
