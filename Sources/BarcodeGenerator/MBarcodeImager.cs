using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace BarcodeGenerator
{
    static class MBarcodeImager
    {
        private readonly static Color backColor = Color.White;
        private readonly static Color foreColor = Color.Black;
        private readonly static Color textColor = Color.FromArgb(220, 220, 220);

        private readonly static int numBits = 10;

        public static Image Create(int content, Size dimensions)
        {
            Bitmap bitmap = new Bitmap(dimensions.Width, dimensions.Height);
            Graphics graphics = Graphics.FromImage(bitmap);

            graphics.Clear(backColor);

            int encoded = MCode.Encode(content);

            bool[] clearArea = new bool[] { false };
            bool[] preamble = new bool[] { true, false, true };
            bool[] data = fillBarsWithNumber(encoded);
            bool[] checksum = CRC.GetChecksum(data);
            bool[] ending = new bool[] { false, false, true };

            int size = preamble.Length + data.Length + checksum.Length + 2 + ending.Length;

            LinkedList<bool> bars = new LinkedList<bool>();


            foreach (bool bit in clearArea)
                bars.AddLast(bit);
            foreach (bool bit in preamble)
                bars.AddLast(bit);
            foreach (bool bit in data)
                bars.AddLast(bit);

            bars.AddLast(checksum[0]);
            bars.AddLast(checksum[1]);
            bars.AddLast(!checksum[1]);

            bars.AddLast(checksum[2]);
            bars.AddLast(checksum[3]);
            bars.AddLast(!checksum[3]);

            foreach (bool bit in ending)
                bars.AddLast(bit);
            foreach (bool bit in clearArea)
                bars.AddLast(bit);
            
            float barWidth = ((float)dimensions.Width) / bars.Count;

            SolidBrush barBrush = new SolidBrush(foreColor);

            int pos = 0;
            foreach (bool bit in bars)
            {
                if (bit == true)
                {
                    graphics.FillRectangle(barBrush, barWidth * pos, 0.0f, barWidth, dimensions.Height);
                }

                ++pos;
            }

            float fontHeight = barWidth * 0.9f;
            SolidBrush textBrush = new SolidBrush(textColor);
            System.Drawing.Font drawFont = new System.Drawing.Font("Courier New", fontHeight, GraphicsUnit.Pixel);
            graphics.DrawString(content.ToString(), drawFont, textBrush, (float)(barWidth * bars.Count - 3.9f * barWidth), (float)(dimensions.Height - fontHeight * 1.1f));
            return bitmap;
        }

        private static bool[] fillBarsWithNumber(int num)
        {
            bool[] bars = new bool[numBits];
            int pos = numBits;
            while (pos > 0)
            {
                bars[--pos] = (num % 2 == 1);
                num /= 2;
            }
            return bars;
        }
    }
}
