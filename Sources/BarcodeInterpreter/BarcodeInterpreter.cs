using System;
using System.Collections.Generic;
using System.Text;

namespace Barcode
{
    public static class BarcodeInterpreter
    {
        public static int Read(bool[] input)
        {
            // -1: invalid length
            // -2: invalid preamble
            // -3: no such code
            // -4: crc padding bytes invalid
            // -5: crc invalid
            // -6: suffix invalid
            if (input.Length != 3 + 10 + 6 + 3)
                return -1;

            if (input[0] != true || input[1] != false || input[2] != true)
                return -2;

            bool[] code = new bool[10];
            for (int i = 0; i < 10; ++i)
                code[i] = input[3 + i];

            int mcode = 0;
            for (int pos = 0; pos < 10; ++pos)
            {
                mcode *= 2;
                if (code[pos])
                    mcode++;
            }

            int k;

            try
            {
                k = MCode.Decode(mcode);
            }
            catch (ArgumentException)
            {
                return -3;
            }

            bool[] crc = new bool[4];
            crc[0] = input[13];
            crc[1] = input[14];
            crc[2] = input[16];
            crc[3] = input[17];

            if (input[14] == input[15] || input[17] == input[18])
                return -4;

            if (!CRC.Validate(code, crc))
                return -5;

            if (input[19] != false || input[20] != false || input[21] != true)
                return -6;

            return k;
        }
    }
}
    