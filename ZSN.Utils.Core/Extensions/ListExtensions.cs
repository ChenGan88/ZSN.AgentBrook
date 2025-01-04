using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZSN.Utils.Core.Extensions
{
    public static class ListExtensions
    {
        public static decimal SumOrZero(this IEnumerable<decimal> source)
        {
            return source == null || !source.Any() ? 0 : source.Sum();
        }
    }
}
