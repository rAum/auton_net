using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VisionFilters;
using System.Drawing;
using RANSAC.Functions;

namespace RANSAC_Test
{
    /// <summary>
    /// Testing Least Squares Fitting for parabola.
    /// Correct data is obtained from wolfram alpha "quadratic fit".
    /// </summary>
    [TestClass]
    public class LeastSquareFit
    {
        private double delta; // defines how much values can vary from the exact value.

        public LeastSquareFit()
        {
            delta = 0.005;
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestInterpolationOfParabola()
        {
            List<Point> points = new List<Point>()
            {
                new Point(2,3),
                new Point(4,2),
                new Point(1,8)
            };

            Parabola result = Parabola.fit(points);

            Parabola correct = new Parabola(1.5, -9.5, 16.0);

            Assert.AreEqual(result.a, correct.a, delta, "coefficient 'a' is wrong.");
            Assert.AreEqual(result.b, correct.b, delta, "coefficient 'b' is wrong.");
            Assert.AreEqual(result.c, correct.c, delta, "coefficient 'c' is wrong.");
        }

        [TestMethod]
        public void TestFittingLine()
        {
            List<Point> points = new List<Point>()
            {
                new Point(30,32),
                new Point(20,22),
                new Point(3,5)
            };

            Parabola result = Parabola.fit(points);
            Parabola correct = new Parabola(0.0, 1.0, 2.0);

            Assert.AreEqual(result.a, correct.a, delta, "coefficient 'a' is wrong.");
            Assert.AreEqual(result.b, correct.b, delta, "coefficient 'b' is wrong.");
            Assert.AreEqual(result.c, correct.c, delta, "coefficient 'c' is wrong.");
        }

        [TestMethod]
        public void TestFittingRandomData()
        {
            List<Point> points = new List<Point>()
            {
                new Point(31,  0),
                new Point(7,  -1),
                new Point(3,  -3),
                new Point(9,  -5),
                new Point(5,  -7),
                new Point(1, -10),
                new Point(7, -15),
                new Point(3, -25)
            };

            Parabola result = Parabola.fit(points);
            Parabola correct = new Parabola(-0.02011, 1.08419, -14.2176);

            Assert.AreEqual(result.a, correct.a, delta, "coefficient 'a' is wrong.");
            Assert.AreEqual(result.b, correct.b, delta, "coefficient 'b' is wrong.");
            Assert.AreEqual(result.c, correct.c, delta, "coefficient 'c' is wrong.");
        }

        [TestMethod]
        public void TestFittingRandomData2()
        {
            List<Point> points = new List<Point>()
            {
                new Point(720,  0),
                new Point(245,320),
                new Point(245, 54),
                new Point(689,32),
                new Point(530,754),
                new Point(732,24),
                new Point(323,240)
            };

            Parabola result = Parabola.fit(points);
            Parabola correct = new Parabola(-0.010085, 9.47823, -1583.3);

            Assert.AreEqual(result.a, correct.a, delta, "coefficient 'a' is wrong.");
            Assert.AreEqual(result.b, correct.b, delta, "coefficient 'b' is wrong.");
            Assert.AreEqual(result.c, correct.c, delta, "coefficient 'c' is wrong.");
        }
    }
}
