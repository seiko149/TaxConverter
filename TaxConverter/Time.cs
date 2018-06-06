using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxConverter
{
    class Time
    {
        public Time()
        {

        }

        public String getTimeStamp()
        {
            DateTime value = DateTime.Now;
            return value.ToString("yyyyMMddHHMM");
        }

    }
}
