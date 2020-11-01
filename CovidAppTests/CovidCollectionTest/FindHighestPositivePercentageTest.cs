using System;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAppTests.CovidCollectionTest
{
    /// <summary>
    ///     Input                   Expected Output
    ///     EmptyList               InvalidOperationException
    ///     SingleItem              0.5
    ///     Multipule(beg)          .75
    ///     Multipule(mid)          .75
    ///     Multipule(end)          .75
    /// </summary>
    [TestClass]
    public class FindHighestPositivePercentageTest
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

            Assert.AreEqual(validData.OverallPositivePercentage, dataCollection.FindHighestPositivePercentage().OverallPositivePercentage, Delta);
       }

       [TestMethod]
       public void TestDateMultipleHighestAtBegin()
       {
           var dataCollection = new CovidDataCollection();

           var validData = new CovidData(new DateTime(2020, 10, 14),"GA", 30, 10, 10, 10, 0);

           var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 10, 10, 10, 10, 0);

           var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 10, 10, 10, 10, 0);

            dataCollection.Add(validData);
            dataCollection.Add(validData2);
           dataCollection.Add(validData3);

            var expected = validData.OverallPositivePercentage;

           Assert.AreEqual(expected, dataCollection.FindHighestPositivePercentage().OverallPositivePercentage, Delta);
       }

        [TestMethod]
        public void TestDateMultipleHighestInMiddle()
        {
            var dataCollection = new CovidDataCollection();

            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 30, 10, 10, 10, 0);

            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 10, 10, 10, 10, 0);

            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 10, 10, 10, 10, 0);

            dataCollection.Add(validData2);
            dataCollection.Add(validData);
            dataCollection.Add(validData3);

            var expected = validData.OverallPositivePercentage;

            Assert.AreEqual(expected, dataCollection.FindHighestPositivePercentage().OverallPositivePercentage, Delta);
        }

        [TestMethod]
        public void TestDateMultipleHighestAtEnd()
        {
            var dataCollection = new CovidDataCollection();

            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 30, 10, 10, 10, 0);

            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 10, 10, 10, 10, 0);

            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 10, 10, 10, 10, 0);

            dataCollection.Add(validData2);
            dataCollection.Add(validData3);
            dataCollection.Add(validData);

            var expected = validData.OverallPositivePercentage;

            Assert.AreEqual(expected, dataCollection.FindHighestPositivePercentage().OverallPositivePercentage, Delta);
        }
    }
}
