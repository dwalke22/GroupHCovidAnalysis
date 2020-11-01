using System;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAppTests.CovidCollectionTest
{
    /// <summary>
    ///     Input                   Expected Output
    ///     EmptyList               InvalidOperationException
    ///     One Item (10.0)         0.5
    ///     Multiple (10.0)         0.5
    ///     100Percent              1
    ///     0Percent                0
    /// </summary>
    [TestClass]
    public class FindOverallPositivityRateTest
    {
        private const double Delta = 0.0000001;

        [TestMethod]
        public void TestEmptyList()
        {
            var dataCollection = new CovidDataCollection();

            Assert.ThrowsException<InvalidOperationException>(() => dataCollection.FindOverallPositivityRate());
        }

        [TestMethod]
        public void TestSingleDataInList()
        {
            var dataCollection = new CovidDataCollection();

            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 10, 10, 10, 10, 0);

            dataCollection.Add(validData);

            Assert.AreEqual(validData.OverallPositivePercentage, dataCollection.FindOverallPositivityRate(), Delta);
        }

        [TestMethod]
        public void TestDateMultiple()
        {
            var dataCollection = new CovidDataCollection();

            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 10, 10, 10, 10, 0);

            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 10, 10, 10, 10, 0);

            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 10, 10, 10, 10, 0);

            dataCollection.Add(validData2);
            dataCollection.Add(validData);
            dataCollection.Add(validData3);

            Assert.AreEqual(0.5, dataCollection.FindOverallPositivityRate(), Delta);
        }

        [TestMethod]
        public void Test100Percent()
        {
            var dataCollection = new CovidDataCollection();

            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 5, 0, 10, 10, 0);

            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 5, 0, 10, 10, 0);

            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 5, 0, 10, 10, 0);

            dataCollection.Add(validData2);
            dataCollection.Add(validData);
            dataCollection.Add(validData3);

            Assert.AreEqual(1, dataCollection.FindOverallPositivityRate(), Delta);
        }

        [TestMethod]
        public void Test0Percent()
        {
            var dataCollection = new CovidDataCollection();

            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 0, 5, 10, 10, 0);

            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 0, 5, 10, 10, 0);

            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 0, 5, 10, 10, 0);

            dataCollection.Add(validData2);
            dataCollection.Add(validData);
            dataCollection.Add(validData3);

            Assert.AreEqual(0, dataCollection.FindOverallPositivityRate(), Delta);
        }
    }
}