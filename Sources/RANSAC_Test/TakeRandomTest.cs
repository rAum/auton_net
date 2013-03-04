using VisionUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace RANSAC_Test
{
    
    
    /// <summary>
    ///This is a test class for TakeRandomTest and is intended
    ///to contain all TakeRandomTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TakeRandomTest
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
        ///A test for TakeRandom Constructor
        ///</summary>
        [TestMethod()]
        public void TakeRandomConstructorTest()
        {
            uint max_ = 10; 
            uint prime_ = 11;
            try
            {
            	TakeRandom target = new TakeRandom(max_, prime_);
            }
            catch (System.Exception)
            {
                Assert.Fail("Two constructor parameters failed");
            }
        }

        /// <summary>
        ///A test for TakeRandom Constructor
        ///</summary>
        [TestMethod()]
        public void TakeRandomConstructorTest1()
        {
            uint max_ = 30; // TODO: Initialize to an appropriate value
            TakeRandom target = new TakeRandom(max_);
            Assert.IsTrue(target.Prime > max_);
        }

        /// <summary>
        ///A test for Get
        ///</summary>
        [TestMethod()]
        public void GetTest()
        {
            //uint max_ = 0; // TODO: Initialize to an appropriate value
            //TakeRandom target = new TakeRandom(max_); // TODO: Initialize to an appropriate value
            //IEnumerable<uint> expected = null; // TODO: Initialize to an appropriate value
            //IEnumerable<uint> actual;
            //actual = target.Get();
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Shuffle
        ///</summary>
        [TestMethod()]
        public void ShuffleTest()
        {
            //uint max_ = 0; // TODO: Initialize to an appropriate value
            //TakeRandom target = new TakeRandom(max_); // TODO: Initialize to an appropriate value
            //target.Shuffle();
            //Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
