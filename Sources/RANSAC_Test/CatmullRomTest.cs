using VisionUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;

namespace RANSAC_Test
{
    
    
    /// <summary>
    ///This is a test class for CatmullRomTest and is intended
    ///to contain all CatmullRomTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CatmullRomTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for CatmullRom Constructor
        ///</summary>
        [TestMethod()]
        public void CatmullRomConstructorTest()
        {
            PointF[] points_ = { new PointF(320, 640), new PointF(300, 600), new PointF(200, 300), new PointF(180, 200) };
            CatmullRom target = new CatmullRom(points_);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for at
        ///</summary>
        [TestMethod()]
        public void atTest()
        {
            PointF[] points_ = null; // TODO: Initialize to an appropriate value
            CatmullRom target = new CatmullRom(points_); // TODO: Initialize to an appropriate value
            float y = 0F; // TODO: Initialize to an appropriate value
            PointF expected = new PointF(); // TODO: Initialize to an appropriate value
            PointF actual;
            actual = target.at(y);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for p
        ///</summary>
        [TestMethod()]
        public void pTest()
        {
            PointF[] points_ = null; // TODO: Initialize to an appropriate value
            CatmullRom target = new CatmullRom(points_); // TODO: Initialize to an appropriate value
            int i = 0; // TODO: Initialize to an appropriate value
            PointF expected = new PointF(); // TODO: Initialize to an appropriate value
            PointF actual;
            actual = target.p(i);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for value
        ///</summary>
        [TestMethod()]
        public void valueTest()
        {
            float t = 0F; // TODO: Initialize to an appropriate value
            PointF a = new PointF(); // TODO: Initialize to an appropriate value
            PointF b = new PointF(); // TODO: Initialize to an appropriate value
            PointF c = new PointF(); // TODO: Initialize to an appropriate value
            PointF d = new PointF(); // TODO: Initialize to an appropriate value
            PointF expected = new PointF(); // TODO: Initialize to an appropriate value
            PointF actual;
            actual = CatmullRom.value(t, a, b, c, d);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
