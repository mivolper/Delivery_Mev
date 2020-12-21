using delivery.USC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delivery.Flags
{
    public static class StaticFlag
    {
        public static string CustomerCode { get; set; }

        public static DataRow StRow { get; set; }

        public static Order Order { get; set; }


    }
}
