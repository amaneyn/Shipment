using System.Collections.Generic;
using System.Linq;

namespace Shipment.SDK.Extension
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<int> ConvertToInt(this IEnumerable<string> source)
        {
            int x = 0;
            var result = source.Where(str => int.TryParse(str, out x))
                                .Select(str => x);

            return result;
        }

        public static bool HasElements<T>(this IEnumerable<T> source)
        {
            if (source != null && source.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
