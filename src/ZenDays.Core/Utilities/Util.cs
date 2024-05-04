using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenDays.Core.Utilities
{
    public static class Util
    {
        public static DateTime ConvertToDateTime(string value, string separador)
        {
            return DateTime.ParseExact(value, $"dd{separador}MM{separador}yyyy", CultureInfo.InvariantCulture);
        }
    }
}
