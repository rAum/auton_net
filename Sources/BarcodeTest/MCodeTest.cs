using BarcodeGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BarcodeTest
{
    [TestClass()]
    public class MCodeTest
    {
        [TestMethod()]
        public void DecodeTest()
        {
            int c = 9;
            int expected = 5;
            int actual;
            actual = MCode.Decode(c);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void EncodeTest()
        {
            int k = 17;
            int expected = 45;
            int actual;
            actual = MCode.Encode(k);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Out-of-range number did not throw any exception")]
        public void EncodeOutOfRangeTest()
        {
            int k = MCode.Count;
            int encoded = MCode.Encode(k); // should throw
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Invalid number did not throw any exception")]
        public void DecodeOutOfRangeTest()
        {
            int c = 100000;
            int decoded = MCode.Decode(c); // should throw
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Invalid number did not throw any exception")]
        public void DecodeNoSuchMCodeTest()
        {
            int c = 3;
            int decoded = MCode.Decode(c); // should throw
        }



        [TestMethod()]
        public void CountTest()
        {
            int count = MCode.Count;

            int num = count - 1;
            int encoded = MCode.Encode(num);
            int decoded = MCode.Decode(encoded);

            Assert.AreEqual(num, decoded);
        }
    }
}
