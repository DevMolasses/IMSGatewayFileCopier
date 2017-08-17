using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSGatewayFileCopier
{
    class Directories
    {
        private string srcDir = @"D:\Sinet\DAC_TimeSeries";
        private string destDir = @"E:\RawData";
        public string sourceDirectory { get { return srcDir; } set { } }
        public string destinationDirectory { get { return destDir; } set { } }

        public Directories() { }
    }
}
