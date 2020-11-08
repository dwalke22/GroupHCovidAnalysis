using System;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAppTests.CovidCollectionTest
{
    /// <summary>
    ///     TestAllDataInBounds                     True
    ///     TestNoDataInBounds                      False
    ///     TestInvertedBounds                      False
    ///     TestHighestInBoundsWithOthers           True
    ///     TestHighestInBoundsAlone                True
    ///     TestHighestOutsideBounds                False
    ///     TestEmptyCollection                     Exception
    /// </summary>
    [TestClass]
    public class BoundsContainHighestIncreaseTests
    {
        [TestMethod]
        public void TestAllDataInBounds()
        {
            var testDataCollection = new CovidDataCollection();
            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 6, 10, 10, 10,0);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 7, 10, 10, 10,0);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 8, 10, 10, 10,0);

            testDataCollection.Add(validData);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);

            Assert.AreEqual(true, testDataCollection.BoundsContainHighestIncrease(1, 10));
        }

        [TestMethod]
        public void TestNoDataInBounds()
        {
            var testDataCollection = new CovidDataCollection();
            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 6, 10, 10, 10,0);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 7, 10, 10, 10, 0);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 8, 10, 10, 10, 0);

            testDataCollection.Add(validData);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);

            Assert.AreEqual(false, testDataCollection.BoundsContainHighestIncrease(9, 12));
        }

        [TestMethod]
        public void TestInvertedBounds()
        {
            var testDataCollection = new CovidDataCollection();
            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 6, 10, 10, 10, 0);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 7, 10, 10, 10, 0);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 8, 10, 10, 10, 0);

            testDataCollection.Add(validData);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);

            Assert.AreEqual(false, testDataCollection.BoundsContainHighestIncrease(12, 2));
        }

        [TestMethod]
        public void TestHighestInBoundsWithOthers()
        {
            var testDataCollection = new CovidDataCollection();
            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 6, 10, 10, 10, 0);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 10, 10, 10, 10, 0);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 12, 10, 10, 10, 0);

            testDataCollection.Add(validData);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);

            Assert.AreEqual(true, testDataCollection.BoundsContainHighestIncrease(7, 15));
        }

        [TestMethod]
        public void TestHighestInBoundsAlone()
        {
            var testDataCollection = new CovidDataCollection();
            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 6, 10, 10, 10, 0);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 7, 10, 10, 10, 0);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 22, 10, 10, 10, 0);

            testDataCollection.Add(validData);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);

            Assert.AreEqual(true, testDataCollection.BoundsContainHighestIncrease(12, 25));
        }

        [TestMethod]
        public void TestHighestOutsideBounds()
        {
            var testDataCollection = new CovidDataCollection();
            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 6, 10, 10, 10, 0);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 7, 10, 10, 10, 0);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 88, 10, 10, 10, 0);

            testDataCollection.Add(validData);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);

            Assert.AreEqual(false, testDataCollection.BoundsContainHighestIncrease(3, 9));
        }

        [TestMethod]
        public void TestEmptyCollection()
        {
            var testDataCollection = new CovidDataCollection();

            Assert.ThrowsException<InvalidOperationException>(() =>
                testDataCollection.BoundsContainHighestIncrease(1, 10));
        }
    }
}