using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;

namespace UTDemo.Common
{
    public static class EnvironmentUtils
    {
        public static void SetEnvironments()
        {
            //"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Common\IntelliTrace\ProfilerProxy\x86\Microsoft.IntelliTrace.ProfilerProxy.dll"
            Environment.SetEnvironmentVariable("INTELLITRACE_PROFILER_DIRECTORY", @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\CommonExtensions\Microsoft\IntelliTrace");
            Environment.SetEnvironmentVariable("COR_ENABLE_PROFILING", "1");
            Environment.SetEnvironmentVariable("COR_PROFILER_PATH", @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\CommonExtensions\Microsoft\IntelliTrace\Microsoft.IntelliTrace.Profiler.dll");
            Environment.SetEnvironmentVariable("COR_PROFILER", "{9317ae81-bcd8-47b7-aaa1-a28062e41c71}");
        }

        public static string GetEnvironments()
        {
            var stringBuilder = new StringBuilder();

            foreach (var item in Environment.GetEnvironmentVariables().OfType<DictionaryEntry>().OrderBy(e => e.Key))
            {
                stringBuilder.AppendLine($"{item.Key}:{item.Value}");
            }

            return stringBuilder.ToString();
        }

        public static string GetProfilerPath()
        {
            const string baseDir = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\CommonExtensions\Microsoft\IntelliTrace";
            string subDir = IntPtr.Size == 8 ? "x64" : string.Empty;

            return Path.Combine(baseDir, subDir, "Microsoft.IntelliTrace.Profiler.dll");
        }
    }
}