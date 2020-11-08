using System;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAppTests.CovidCollectionTest
{
    /// <summary>
    ///     TestIntegerAverageNoZeroes                          7
    ///     TestDoubleAverageNoZeroes                           7.75
    ///     TestAverageWithLeadingZero                          9
    ///     TestAverageWithMiddleZeroes                         8.5
    ///     TestEmptyCollection                                 Exception
    /// </summary>
    [TestClass]
    public class FindAveragePositiveCasesSinceFirstPositiveTests
    {
        [TestMethod]
        public void TestIntegerAverageNoZeroes()
        {
            var testDataCollection = new CovidDataCollection();
            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 6, 10, 10, 10, 0);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 7, 10, 10, 10, 0);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 8, 10, 10, 10, 0);

            testDataCollection.Add(validData);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);

            Assert.AreEqual(7, testDataCollection.FindAveragePositiveCasesSinceFirstPositive());
        }

        [TestMethod]
        public void TestDoubleAverageNoZeroes()
        {
            var testDataCollection = new CovidDataCollection();
            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 4, 10, 10, 10, 0);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 7, 10, 10, 10, 0);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 8, 10, 10, 10, 0);
            var validData4 = new CovidData(new DateTime(2020, 10, 17), "GA", 12, 10, 10, 10, 0);

            testDataCollection.Add(validData);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);
            testDataCollection.Add(validData4);

            Assert.AreEqual(7.75, testDataCollection.FindAveragePositiveCasesSinceFirstPositive());
        }

        [TestMethod]
        public void TestAverageWithLeadingZero()
        {
            var testDataCollection = new CovidDataCollection();
            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 0, 10, 10, 10, 0);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 7, 10, 10, 10, 0);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 8, 10, 10, 10, 0);
            var validData4 = new CovidData(new DateTime(2020, 10, 17), "GA", 12, 10, 10, 10, 0);

            testDataCollection.Add(validData);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);
            testDataCollection.Add(validData4);

            Assert.AreEqual(9, testDataCollection.FindAveragePositiveCasesSinceFirstPositive());
        }

        [TestMethod]
        public void TestAverageWithMiddleZeroes()
        {
            var testDataCollection = new CovidDataCollection();
            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 0, 10, 10, 10, 0);
            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 7, 10, 10, 10, 0);
            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 0, 10, 10, 10, 0);
            var validData4 = new CovidData(new DateTime(2020, 10, 17), "GA", 12, 10, 10, 10, 0);
            var validData5 = new CovidData(new DateTime(2020, 10, 18), "GA", 15, 10, 10, 10, 0);

            testDataCollection.Add(validData);
            testDataCollection.Add(validData2);
            testDataCollection.Add(validData3);
            testDataCollection.Add(validData4);
            testDataCollection.Add(validData5);

            Assert.AreEqual(8.5, testDataCollection.FindAveragePositiveCasesSinceFirstPositive());
        }

        [TestMethod]
        public void TestEmptyCollection()
        {
            var testDataCollection = new CovidDataCollection();

            Assert.ThrowsException<InvalidOperationException>(() => testDataCollection.FindAverageNumberOfTestPerDay());
        }
    }
}