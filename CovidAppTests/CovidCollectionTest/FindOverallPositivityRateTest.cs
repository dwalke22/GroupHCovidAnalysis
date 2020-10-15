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

            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 10, 10, 10, 10);

            dataCollection.Add(validData);

            Assert.AreEqual(validData.OverallPositivePercentage, dataCollection.FindOverallPositivityRate(), Delta);
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

           var expected = (validData.OverallPositivePercentage + validData2.OverallPositivePercentage +
                           validData3.OverallPositivePercentage) / 3;

           Assert.AreEqual(expected, dataCollection.FindOverallPositivityRate(), Delta);
       }

    }
}
