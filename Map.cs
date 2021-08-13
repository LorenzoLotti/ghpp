using System;

namespace Ghpp
{
    internal sealed class Map
    {
        const uint MinValue = uint.MinValue, MaxValue = 33554431U;
        internal bool[] Bits { get; }

        internal Map(uint? bits25 = null)
        {
            var count = MathF.ILogB(MaxValue + 1);
            Bits = new bool[count];
            bits25 ??= (uint)Randomizer.Generator.Next((int)MinValue, (int)MaxValue + 1);

            for (var i = 0; i < count; i++)
                Bits[i] = (bits25 >> i & 1) == 1;
        }

        internal static Map GetSymmetricMap()
        {
            Map map;

            do
                map = new Map();
            while (!IsSymmetric(map));

            return map;
        }

        internal static bool IsSymmetric(Map map)
        {
            var size = (int)MathF.Sqrt(map.Bits.Length);
            var mid = size / 2;
            var cols = new byte[2, 5];

            for (int x = 0, y = -1, col = 0, i = 0; i < map.Bits.Length; i++, x++)
            {
                if (i % size == mid)
                {
                    col = 1;
                    x = -1;
                }
                else
                {
                    if (i % size == 0)
                    {
                        col = 0;
                        x = 0;
                        y++;
                    }

                    cols[col, y] |= (byte)((map.Bits[i] ? 1 : 0) << (col == 0 ? x : 1 - x));
                }
            }

            var result = true;

            for (var y = 0; y < size; y++)
                result &= cols[0, y] == cols[1, y];

            return result;
        }
    }
}
