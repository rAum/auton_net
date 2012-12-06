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
        public void GetRandomSampleTest()
        {
            List<Point> input = null; // TODO: Initialize to an appropriate value
            int samplesCount = 0; // TODO: Initialize to an appropriate value
            List<Point> expected = null; // TODO: Initialize to an appropriate value
            List<Point> actual;
            actual = RANSAC.RANSAC.GetRandomSample(input, samplesCount);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for fit
        ///</summary>
        [TestMethod()]
        public void fitTest()
        {
            int iterations = 0; // TODO: Initialize to an appropriate value
            int init_samples = 0; // TODO: Initialize to an appropriate value
            int n = 0; // TODO: Initialize to an appropriate value
            double error_threshold = 0F; // TODO: Initialize to an appropriate value
            List<Point> inputData = null; // TODO: Initialize to an appropriate value
            Parabola expected = null; // TODO: Initialize to an appropriate value
            Parabola actual;
            actual = RANSAC.RANSAC.fit(iterations, init_samples, n, error_threshold, inputData);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
