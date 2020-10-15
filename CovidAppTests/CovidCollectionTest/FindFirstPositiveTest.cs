using System;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAppTests.CovidCollectionTest
{
    /// <summary>
    ///     Input                   Expected Output
    ///     EmptyList               InvalidOperationException
    ///     One Item (10/14/2020)   10/14/2020
    ///     DateInMiddle            10/14/2020
    ///     DateAtEnd               10/14/2020
    /// </summary>
    [TestClass]
    public class FindFirstPositiveTest
    {
        [TestMethod]
        public void TestEmptyList()
        {
            var dataCollection = new CovidDataCollection();

            Assert.ThrowsException<InvalidOperationException>(() => dataCollection.FindFirstPositiveTest());
        }

       [TestMethod]
       public void TestSingleDataInList()
       {
            var dataCollection = new CovidDataCollection();

            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 10, 10, 10, 10);

            dataCollection.Add(validData);

            Assert.AreEqual(validData.Date, dataCollection.FindFirstPositiveTest());
       }

       [TestMethod]
       public void TestDateInMiddleOfList()
       {
           var dataCollection = new CovidDataCollection();

           var validData = new CovidData(new DateTime(2020, 10, 14),"GA", 10, 10, 10, 10);

           var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 10, 10, 10, 10);

           var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 10, 10, 10, 10);

            dataCollection.Add(validData2);
            dataCollection.Add(validData);
            dataCollection.Add(validData3);

           Assert.AreEqual(validData.Date, dataCollection.FindFirstPositiveTest());
       }

       [TestMethod]
       public void TestDateAtEndOfList()
       {
           var dataCollection = new CovidDataCollection();

           var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 10, 10, 10, 10);

           var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 10, 10, 10, 10);

           var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 10, 10, 10, 10);

           dataCollection.Add(validData2);
           dataCollection.Add(validData3);
           dataCollection.Add(validData);

           Assert.AreEqual(validData.Date, dataCollection.FindFirstPositiveTest());
        }
    }
}
