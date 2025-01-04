using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSN.Utils.Core
{
    public class Config
    {
        public static string DateFormat
        {
            get
            {
                return "yyyy-MM-dd";
            }
        }

        public static string DateTimeFormat
        {
            get
            {
                return "yyyy-MM-dd HH:mm";
            }
        }

        public static string DateTimeFormatS
        {
            get
            {
                return "yyyy-MM-dd HH:mm:ss";
            }
        }
    }
}
