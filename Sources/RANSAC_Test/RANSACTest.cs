using RANSAC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Collections.Generic;
using RANSAC.Functions;

namespace RANSAC_Test
{
    
    
    /// <summary>
    ///This is a test class for RANSACTest and is intended
    ///to contain all RANSACTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RANSACTest
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
        ///A test for GetRandomSample
        ///</summary>
        [TestMethod()]
        public void GetRandomSampleSimpleTest()
        {
            List<Point> input = new List<Point>()
            {
                new Point(1,1),
                new Point(2,2),
                new Point(3,3),
                new Point(4,4)
            };

            int samplesCount = 2;
            List<Point> actual;
            actual = RANSAC.RANSAC.GetRandomSample(input, samplesCount);

            Assert.AreEqual(actual.Count, samplesCount, "number of random samples mismatch.");
            Assert.AreNotEqual(actual[0], actual[1], "the same points are in the set.");
        }
    }
}
