using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTDemo
{
    internal struct SYSTEM_INFO
    {
        public PROCESSOR_ARCHITECTURE wProcessorArchitecture;
        public ushort wReserved;
        public uint dwPageSize;
        public int lpMinimumApplicationAddress;
        public int lpMaximumApplicationAddress;
        public uint dwActiveProcessorMask;
        public uint dwNumberOfProcessors;
        public uint dwProcessorType;
        public uint dwAllocationGranularity;
        public ushort wProcessorLevel;
        public ushort wProcessorRevision;
    }
}
