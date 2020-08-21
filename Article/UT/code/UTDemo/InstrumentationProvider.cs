using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace UTDemo
{
    internal sealed class InstrumentationProvider
    {
        internal delegate bool NativeCanDetour([MarshalAs(UnmanagedType.LPStruct), In] Guid mvid, int methodDef);
        internal delegate int NativeSetDetourProvider(IntPtr addr);

        private static readonly IntPtr detourProviderAddress = typeof(InstrumentationProvider).GetMethod("DetourProvider", BindingFlags.Static | BindingFlags.NonPublic).MethodHandle.GetFunctionPointer();
        private RegistryKey classesRootKey = Registry.ClassesRoot;
        private IntPtr profilerModule;
        private NativeSetDetourProvider setDetourProvider;
        private NativeCanDetour canDetour;
        private bool enabled;

        public void Initialize()
        {
            string profilerPath = this.ResolveProfilerPath();
            this.profilerModule = InstrumentationProvider.LoadProfilerModule(profilerPath);
            setDetourProvider = LibraryMethods.GetFunction<NativeSetDetourProvider>(this.profilerModule, "SetDetourProvider");
            canDetour = LibraryMethods.GetFunction<NativeCanDetour>(this.profilerModule, "CanDetour");
            if (setDetourProvider(InstrumentationProvider.detourProviderAddress) != 0)
            {
                throw new Exception(profilerPath);
            }
            this.enabled = true;
        }

        public bool IsDetoursEnabled()
        {
            return this.enabled;
        }

        public bool CanDetour(MethodBase method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            if (!this.IsDetoursEnabled())
                throw new InvalidOperationException();
            return this.canDetour(method.Module.ModuleVersionId, method.MetadataToken);
        }

        //public IDisposable AcquireProtectingThreadContext()
        //{
        //    return (IDisposable)new IntelliTraceInstrumentationProvider.ProtectingContext();
        //}

        //public bool IsThreadProtected
        //{
        //    get
        //    {
        //        return IntelliTraceInstrumentationProvider.ProtectingContext.IsThreadProtected;
        //    }
        //}

        internal RegistryKey ClassesRootKey
        {
            get
            {
                return this.classesRootKey;
            }
            set
            {
                this.classesRootKey = value;
            }
        }

        private string ResolveProfilerPath()
        {
            var dir = IntPtr.Size != 8 ? @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\CommonExtensions\Microsoft\IntelliTrace" : @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\CommonExtensions\Microsoft\IntelliTrace\x64";

            return dir + "\\Microsoft.IntelliTrace.Profiler.dll";

            //string environmentVariable1 = Environment.GetEnvironmentVariable(IntPtr.Size == 8 ? "MicrosoftInstrumentationEngine_ConfigPath64_FakesInstrumentation" : "MicrosoftInstrumentationEngine_ConfigPath32_FakesInstrumentation", EnvironmentVariableTarget.Process);
            //if (File.Exists(environmentVariable1))
            //{
            //    XmlReaderSettings settings = new XmlReaderSettings()
            //    {
            //        ProhibitDtd = true,
            //        XmlResolver = (XmlResolver)null
            //    };
            //    XmlReader xmlReader = XmlReader.Create(environmentVariable1, settings);
            //    if (xmlReader.ReadToDescendant("Module"))
            //    {
            //        string path2 = xmlReader.ReadInnerXml();
            //        return Path.Combine(Path.GetDirectoryName(environmentVariable1), path2);
            //    }
            //}
            //string environmentVariable2 = Environment.GetEnvironmentVariable("COR_PROFILER_PATH", EnvironmentVariableTarget.Process);
            //if (!string.IsNullOrEmpty(environmentVariable2))
            //    return environmentVariable2;
            //string environmentVariable3 = Environment.GetEnvironmentVariable("INTELLITRACE_PROFILER_DIRECTORY", EnvironmentVariableTarget.Process);
            //if (!string.IsNullOrEmpty(environmentVariable3))
            //{
            //    string str = environmentVariable3;
            //    return IntPtr.Size != 8 ? str + "\\Microsoft.IntelliTrace.Profiler.dll" : str + "\\x64\\Microsoft.IntelliTrace.Profiler.dll";
            //}
            //string environmentVariable4 = Environment.GetEnvironmentVariable("COR_PROFILER", EnvironmentVariableTarget.Process);
            //if (string.IsNullOrEmpty(environmentVariable4))
            //    throw new UnitTestIsolationException(FakesFrameworkResources.FailedToResolveProfilerPath);
            //string name = "CLSID\\" + environmentVariable4 + "\\InprocServer32";
            //SYSTEM_INFO SystemInfo = new SYSTEM_INFO();
            //NativeMethods.GetSystemInfo(ref SystemInfo);
            //if (IntPtr.Size == 4 && SystemInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE.PROCESSOR_ARCHITECTURE_AMD64)
            //    name = "Wow6432Node\\" + name;
            //using (RegistryKey registryKey = this.classesRootKey.OpenSubKey(name))
            //{
            //    if (registryKey == null)
            //        throw new UnitTestIsolationException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, FakesFrameworkResources.FailedToOpenProfilerRegistryKey, (object)name));
            //    return (string)registryKey.GetValue((string)null);
            //}
        }

        private static IntPtr LoadProfilerModule(string profilerPath)
        {
            try
            {
                return LibraryMethods.GetModuleHandle(profilerPath);
            }
            catch (Win32Exception ex)
            {
                throw;
            }
        }

        internal static void DetourProvider(
          object receiver,
          RuntimeMethodHandle methodHandle,
          RuntimeTypeHandle declaringTypeHandle,
          RuntimeTypeHandle[] genericMethodTypeArgumentHandles,
          out object detourDelegate,
          out IntPtr detourPointer)
        {
            detourDelegate = (object)null;
            detourPointer = IntPtr.Zero;
            //if (IntelliTraceInstrumentationProvider.ProtectingContext.IsThreadProtected)
            //    return;
            //using (new IntelliTraceInstrumentationProvider.ProtectingContext())
            //{
            //    MethodBase method = MethodBase.GetMethodFromHandle(methodHandle, declaringTypeHandle);
            //    if (method.IsGenericMethodDefinition)
            //    {
            //        Type[] typeArray = new Type[genericMethodTypeArgumentHandles.Length];
            //        for (int index = 0; index < genericMethodTypeArgumentHandles.Length; ++index)
            //            typeArray[index] = Type.GetTypeFromHandle(genericMethodTypeArgumentHandles[index]);
            //        method = (MethodBase)((MethodInfo)method).MakeGenericMethod(typeArray);
            //    }
            //    detourDelegate = (object)UnitTestIsolationRuntime.GetDetour(receiver, method);
            //    if (detourDelegate == null)
            //        return;
            //    detourPointer = detourDelegate.GetType().GetMethod("Invoke").MethodHandle.GetFunctionPointer();
            //}
        }

        //private sealed class ProtectingContext : IDisposable
        //{
        //    [ThreadStatic]
        //    private static int instanceCount;

        //    public ProtectingContext()
        //    {
        //        ++IntelliTraceInstrumentationProvider.ProtectingContext.instanceCount;
        //    }

        //    public static bool IsThreadProtected
        //    {
        //        get
        //        {
        //            return IntelliTraceInstrumentationProvider.ProtectingContext.instanceCount > 0;
        //        }
        //    }

        //    public void Dispose()
        //    {
        //        --IntelliTraceInstrumentationProvider.ProtectingContext.instanceCount;
        //    }
        //}
    }
}