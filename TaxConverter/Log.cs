using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxConverter
{
    class Log
    {
        StreamWriter w;
        public Log(StreamWriter w)
        {
            this.w = w;
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
        }

        public void append(String message)
        {
            //  w.WriteLine("  :");
            w.WriteLine(DateTime.Now.ToLongTimeString() + ":" + message);
            //  w.WriteLine("-----------------------------------------------------------");
        }
    }

}
