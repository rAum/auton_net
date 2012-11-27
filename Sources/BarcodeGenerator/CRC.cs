using System;
using System.Collections.Generic;
using System.Text;

namespace BarcodeGenerator
{
    public class CRC
    {
        public static readonly bool[] Poly4 = {false, false, true, true};

        private static bool BoolXor(bool a, bool b)
        {
            return a != b;
        }

        private static bool[] Calculate(bool[] input, bool[] poly, bool[] padding)
        {
            bool[] current = new bool[poly.Length];

            for (int pos = 0; pos < poly.Length; ++pos)
                current[pos] = (pos < input.Length) ? input[pos] : padding[pos - input.Length];

            for (int pos = 0; pos < input.Length; ++pos)
            {
                bool prev = current[0];
                for (int i = 0; i < poly.Length - 1; ++i)
                    current[i] = current[i + 1];
                current[poly.Length - 1] = (pos + poly.Length < input.Length) ? input[pos + poly.Length] : padding[pos + poly.Length - input.Length];

                if (prev)
                {
                    for (int i = 0; i < poly.Length; ++i)
                        current[i] = BoolXor(current[i], poly[i]);
                }

            }
            return current;
        }

        public static bool[] GetChecksum(bool[] data)
        {
            bool[] poly = Poly4;
            return Calculate(data, poly, new bool[poly.Length]);
        }

        public static bool Validate(bool[] data, bool[] checksum)
        {
            bool[] poly = Poly4;
            bool[] result = Calculate(data, poly, checksum);

            foreach (bool elem in result)
                if (elem)
                    return false;

            return true;
        }
     
    }
}
