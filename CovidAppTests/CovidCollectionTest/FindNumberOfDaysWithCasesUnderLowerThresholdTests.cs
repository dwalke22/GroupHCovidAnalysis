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
    public class FindNumberOfDaysWithCasesUnderLowerThresholdTests
    {
        private const double Delta = 0.0000001;

        [TestMethod]
        public void TestEmptyCollection()
        {
            var testDataCollection = new CovidDataCollection();


           Assert.ThrowsException<InvalidOperationException>(() => testDataCollection.FindNumberOfDaysWithCasesUnderLowerThreshold(1000));
        }

        [TestMethod]
        public void TestNoDaysWithZero()
        {
            var testDataCollection = new CovidDataCollection();
            var validData1 = new CovidData(new DateTime(2020, 10, 14), "GA", 5, 0, 10, 10);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 5, 0, 10, 10);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 10, 0, 10, 10);
            var validData4 = new CovidData(new DateTime(2020, 10, 17), "GA", 10, 0, 10, 10);

            testDataCollection.Add(validData1);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);
            testDataCollection.Add(validData4);

            Assert.AreEqual(2,testDataCollection.FindNumberOfDaysWithCasesUnderLowerThreshold(10));
        }

        [TestMethod]
        public void TestDaysWithZero()
        {
            var testDataCollection = new CovidDataCollection();
            var validData1 = new CovidData(new DateTime(2020, 10, 14), "GA", 0, 0, 10, 10);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 0, 0, 10, 10);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 5, 0, 10, 10);
            var validData4 = new CovidData(new DateTime(2020, 10, 17), "GA", 5, 0, 10, 10);
            var validData5 = new CovidData(new DateTime(2020, 10, 18), "GA", 5, 0, 10, 10);
            var validData6 = new CovidData(new DateTime(2020, 10, 18), "GA", 10, 0, 10, 10);

            testDataCollection.Add(validData1);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);
            testDataCollection.Add(validData4);
            testDataCollection.Add(validData5);
            testDataCollection.Add(validData6);

            Assert.AreEqual(3, testDataCollection.FindNumberOfDaysWithCasesUnderLowerThreshold(10));
        }

        [TestMethod]
        public void TestDaysAddedOutOfOrderWithZero()
        {
            var testDataCollection = new CovidDataCollection();
            var validData1 = new CovidData(new DateTime(2020, 10, 14), "GA", 0, 0, 10, 10);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 0, 0, 10, 10);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 5, 0, 10, 10);
            var validData4 = new CovidData(new DateTime(2020, 10, 17), "GA", 5, 0, 10, 10);
            var validData5 = new CovidData(new DateTime(2020, 10, 18), "GA", 5, 0, 10, 10);
            var validData6 = new CovidData(new DateTime(2020, 10, 18), "GA", 10, 0, 10, 10);

            testDataCollection.Add(validData5);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);
            testDataCollection.Add(validData6);
            testDataCollection.Add(validData4);
            testDataCollection.Add(validData1);


            Assert.AreEqual(3, testDataCollection.FindNumberOfDaysWithCasesUnderLowerThreshold(10));
        }
    }
}