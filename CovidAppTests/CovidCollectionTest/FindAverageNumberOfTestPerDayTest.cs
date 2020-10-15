using System;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAppTests.CovidCollectionTest
{
    /// <summary>
    ///     Input                   Expected Output
    ///     EmptyList               InvalidOperationException
    ///     One Item (20.0)         20.0
    ///     Multiple (20.0)         20.0
    /// </summary>
    [TestClass]
    public class FindAverageNumberOfTestPerDayTest
    {
        private const double Delta = 0.0000001;

        [TestMethod]
        public void TestEmptyList()
        {
            var dataCollection = new CovidDataCollection();

            Assert.ThrowsException<InvalidOperationException>(() => dataCollection.FindAverageNumberOfTestPerDay());
        }

       [TestMethod]
       public void TestSingleDataInList()
       {
            var dataCollection = new CovidDataCollection();

            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 10, 10, 10, 10);

            dataCollection.Add(validData);

            var expected = Convert.ToDouble(validData.TotalTest);

            Assert.AreEqual(expected, dataCollection.FindAverageNumberOfTestPerDay(), Delta);
       }

       [TestMethod]
       public void TestDateMultiple()
       {
           var dataCollection = new CovidDataCollection();

           var validData = new CovidData(new DateTime(2020, 10, 14),"GA", 10, 10, 10, 10);

           var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 10, 10, 10, 10);

           var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 10, 10, 10, 10);

           dataCollection.Add(validData2);
           dataCollection.Add(validData);
           dataCollection.Add(validData3);

           double expected = (validData.TotalTest + validData2.TotalTest +
                           validData3.TotalTest) / 3.0;

           Assert.AreEqual(expected, dataCollection.FindAverageNumberOfTestPerDay(), Delta);
       }

    }
}
