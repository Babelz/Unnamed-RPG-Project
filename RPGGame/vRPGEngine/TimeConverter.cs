using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine
{
    public static class TimeConverter
    {
        public static int ToMilliseconds(float seconds)
        {
            return (int)seconds * 1000;
        }
        public static int ToMilliseconds(int seconds)
        {
            return seconds * 1000;
        }

        public static float ToSeconds(int millis)
        {
            return millis / 1000.0f;
        }
    }
}
