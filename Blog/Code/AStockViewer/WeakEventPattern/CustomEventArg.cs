using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeakEventPattern
{
    public class CustomEventArg: EventArgs
    {
        public string Message { get; set; }
    }
}
