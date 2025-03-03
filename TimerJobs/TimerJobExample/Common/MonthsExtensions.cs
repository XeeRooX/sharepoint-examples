using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerJobExample.Common
{
    public static class MonthsExtensions
    {
        public static IEnumerable<int> GetNumbersOfMonth(this Months months)
        {
            var result = Enum.GetValues(months.GetType())
                .Cast<Enum>().Where(months.HasFlag)
                .Select(x => Array.IndexOf(Enum.GetValues(typeof(Months)), x) + 1)
                .Where(x => x != 0);

            return result;
        }

        public static Months ParseAndReplace(string months)
        {
            try
            {
                return (Months)Enum.Parse(typeof(Months), months);
            }
            catch
            {
                return Months.None;
            }

        }

    }
}
