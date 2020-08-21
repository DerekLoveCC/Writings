using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Fakes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTDemo.Common;

namespace UTDemo
{
    public class MainClass
    {
        public static void Main()
        {
            Console.WriteLine(EnvironmentUtils.GetEnvironments());
            Console.Read();
            try
            {

                TestCustomized();

                //UseFakesBuiltIn();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void TestCustomized()
        {
            var dirPath = EnvironmentUtils.GetProfilerPath();
            var result = NativeMethods.LoadLibraryW(dirPath);
            var r = NativeMethods.LoadLibraryW(@"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Common\IntelliTrace\ProfilerProxy\x86\Microsoft.IntelliTrace.ProfilerProxy.dll");

            new InstrumentationProvider().Initialize();
        }

        private static void UseFakesBuiltIn()
        {
            var dirPath = EnvironmentUtils.GetProfilerPath();
            var result = NativeMethods.LoadLibraryW(dirPath);
            var r = NativeMethods.LoadLibraryW(@"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Common\IntelliTrace\ProfilerProxy\x86\Microsoft.IntelliTrace.ProfilerProxy.dll");


            using (var s = ShimsContext.Create())
            {
                ShimDateTime.NowGet = () => new DateTime(2020, 10, 10);
                var now = DateTime.Now;
            }

            var now1 = DateTime.Now;
        }
    }
}
