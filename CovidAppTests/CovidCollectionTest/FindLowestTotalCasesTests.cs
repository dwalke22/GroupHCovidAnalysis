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
    public class FindLowestTotalCasesTests
    {
        [TestMethod]
        public void TestAllLowestTestWithPositiveIncreaseAndNegativeIncrease()
        {
            var testDataCollection = new CovidDataCollection();
            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 6, 10, 10, 10, 0);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 7, 10, 10, 10, 0);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 8, 10, 10, 10, 0);

            testDataCollection.Add(validData);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);

            Assert.AreEqual(16, testDataCollection.FindLowestTotalCases().TotalTest);
        }

        [TestMethod]
        public void TestAllLowestTestWithNoPositiveIncrease()
        {
            var testDataCollection = new CovidDataCollection();
            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 400, 10, 10, 10, 0);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 200, 0, 10, 10, 0);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 0, 100, 10, 10, 0);

            testDataCollection.Add(validData);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);

            Assert.AreEqual(100, testDataCollection.FindLowestTotalCases().TotalTest);
        }

        [TestMethod]
        public void TestAllLowestTestWithNoNegativeIncrease()
        {
            var testDataCollection = new CovidDataCollection();
            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 150, 10, 10, 10, 0);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 50, 0, 10, 10, 0);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 0, 200, 10, 10, 0);

            testDataCollection.Add(validData);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);

            Assert.AreEqual(50, testDataCollection.FindLowestTotalCases().TotalTest);
        }


        [TestMethod]
        public void TestEmptyCollection()
        {
            var testDataCollection = new CovidDataCollection();

            Assert.ThrowsException<InvalidOperationException>(() => testDataCollection.FindLowestTotalCases());
        }
    }
}