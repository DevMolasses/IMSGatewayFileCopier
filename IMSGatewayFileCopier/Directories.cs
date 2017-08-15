using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSGatewayFileCopier
{
    class Directories
    {
        private string srcDir = @"C:\Users\tstewart\Desktop\TestSource";
        private string destDir = @"C:\Users\tstewart\Desktop\TestDest";
        public string sourceDirectory { get { return srcDir; } set { } }
        public string destinationDirectory { get { return destDir; } set { } }

        public Directories() { }
    }
}
