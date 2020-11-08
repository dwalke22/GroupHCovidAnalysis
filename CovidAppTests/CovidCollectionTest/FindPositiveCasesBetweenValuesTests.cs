using System;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAppTests.CovidCollectionTest
{
    /// <summary>
    ///     EmptyCollection                                     Exception
    ///     NoDaysWithZero(2 days out of 4)                     2
    ///     DaysWithZero(3 days out of 6)                       3
    ///     DaysAddedOutOfOrderWithZero(3 days out of 6)        3
    /// </summary>
    [TestClass]
    public class FindPositiveCasesBetweenValuesTests
    {
        [TestMethod]
        public void TestAllDataInBounds()
        {
            var testDataCollection = new CovidDataCollection();
            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 6, 10, 10, 10, 0);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 7, 10, 10, 10, 0);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 8, 10, 10, 10, 0);

            testDataCollection.Add(validData);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);

            Assert.AreEqual(3, testDataCollection.FindPositiveCasesBetweenValues(5, 10));
        }

        [TestMethod]
        public void TestNoDataInBounds()
        {
            var testDataCollection = new CovidDataCollection();
            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 6, 10, 10, 10, 0);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 7, 10, 10, 10, 0);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 8, 10, 10, 10, 0);

            testDataCollection.Add(validData);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);

            Assert.AreEqual(0, testDataCollection.FindPositiveCasesBetweenValues(11, 20));
        }

        [TestMethod]
        public void TestLowerBoundHigherThanUpperBound()
        {
            var testDataCollection = new CovidDataCollection();
            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 6, 10, 10, 10, 0);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 7, 10, 10, 10, 0);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 8, 10, 10, 10, 0);

            testDataCollection.Add(validData);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);

            Assert.AreEqual(0, testDataCollection.FindPositiveCasesBetweenValues(10, 5));
        }

        [TestMethod]
        public void TestSomeDataInBounds()
        {
            var testDataCollection = new CovidDataCollection();
            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 6, 10, 10, 10, 0);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 7, 10, 10, 10, 0);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 8, 10, 10, 10, 0);

            testDataCollection.Add(validData);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);

            Assert.AreEqual(2, testDataCollection.FindPositiveCasesBetweenValues(7, 10));
        }

        [TestMethod]
        public void TestEmptyCollection()
        {
            var testDataCollection = new CovidDataCollection();

            Assert.ThrowsException<InvalidOperationException>(() =>
                testDataCollection.FindPositiveCasesBetweenValues(1, 10));
        }
    }
}