using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BarcodeGenerator;

namespace BarcodeTest
{
    [TestClass]
    public class CRCUnitTest
    {
        [TestMethod]
        public void CRCCalculateVerifyTest()
        {
            Random r = new Random();

            {
                bool[] data1 = { true, false, false, true, true, false };

                bool[] crc = CRC.GetChecksum(data1);

                Assert.IsTrue(CRC.Validate(data1, crc));

                int badpos = r.Next(crc.Length);
                crc[badpos] = !crc[badpos];

                Assert.IsFalse(CRC.Validate(data1, crc));
            }

            {
                bool[] data1 = { false, true, false, true, true, false };

                bool[] crc = CRC.GetChecksum(data1);

                int badpos = r.Next(crc.Length);
                crc[badpos] = !crc[badpos];

                Assert.IsFalse(CRC.Validate(data1, crc));
            }

            {
                bool[] data1 = { true, false, false, true, true, false, true, false };

                bool[] crc = CRC.GetChecksum(data1);

                int badpos = r.Next(crc.Length);
                crc[badpos] = !crc[badpos];

                Assert.IsFalse(CRC.Validate(data1, crc));
            }

            Assert.Inconclusive("This test is worthless. Rewrite it in a better way please");
            
        }
    }
}
