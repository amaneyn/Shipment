using System.Collections.Generic;
using System.Security.Cryptography;

namespace Shipment.SDK.Helper
{
    public static class RandomNumberHelper
    {
        public static IEnumerable<int> GetUniqueRandomNumbers(int minValue, int maxValue, int count)
        {
            var randomNumbers = new HashSet<int>();

            for (int i = 0; i < count; i++)
            {
                int randomNumber = GetRandomInt(minValue, maxValue);
                bool uniqueNumberAdded = randomNumbers.Add(randomNumber);
                while (!uniqueNumberAdded)
                {
                    randomNumber = GetRandomInt(minValue, maxValue);
                    uniqueNumberAdded = randomNumbers.Add(GetRandomInt(minValue, maxValue));
                }
            }
            return randomNumbers;
        }

        public static int GetRandomInt(int minValue, int maxValue)
        {
            if (maxValue == int.MaxValue)
            {
                // Because upper bound of GetInt32 is exclusive, it cannot generate int.MaxValue
                return RandomNumberGenerator.GetInt32(minValue, maxValue);
            }
            // max + 1 because second parameter of GetInt32 is not inclusive
            return RandomNumberGenerator.GetInt32(minValue, maxValue + 1);
        }
    }
}
