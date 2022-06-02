using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DecimalDemoProject
{
    public static class TestDelete
    {
        public static int NextInt32(this Random rng)
        {
            int firstBits = rng.Next(0, 1 << 4) << 13;
            int lastBits = rng.Next(0, 1 << 13);
            return firstBits | lastBits;
        }

        public static decimal NextDecimal(this Random rng)
        {
            
            bool sign = rng.Next(2) == 1;
            return new decimal(rng.NextInt32(),
                               rng.NextInt32(),
                               rng.NextInt32(),
                               sign,
                               10);
        }
    }
}
