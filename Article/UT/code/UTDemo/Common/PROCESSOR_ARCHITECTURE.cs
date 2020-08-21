using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTDemo.Common
{
    internal enum PROCESSOR_ARCHITECTURE : ushort
    {
        INTEL = 0,
        IA64 = 6,
        AMD64 = 9,
        UNKNOWN = 65535, // 0xFFFF
    }
}
