using System;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAppTests.CovidCollectionTest
{
    /// <summary>
    ///     Input           Expected Output
    ///     null            ArgumentNullException
    ///     validData       1
    ///     validData(3)    3
    /// </summary>
    [TestClass]
    public class AddTest
    {
        [TestMethod]
        public void TestValidDataEmptyList()
        {
            var dataCollection = new CovidDataCollection();

            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 10, 10, 10, 10,0);

            dataCollection.Add(validData);

            Assert.AreEqual(1, dataCollection.Count);
        }

        [TestMethod]
        public void TestNullData()
        {
            var dataCollection = new CovidDataCollection();
            Assert.ThrowsException<ArgumentNullException>(() => dataCollection.Add(null));
        }

        [TestMethod]
        public void TestMultipleAdds()
        {
            var dataCollection = new CovidDataCollection();

            var validData = new CovidData(new DateTime(2020, 10, 14), "GA", 10, 10, 10, 10,0);

            var validData2 = new CovidData(new DateTime(2020, 10, 15), "GA", 10, 10, 10, 10,0);

            var validData3 = new CovidData(new DateTime(2020, 10, 16), "GA", 10, 10, 10, 10,0);

            dataCollection.Add(validData);
            dataCollection.Add(validData2);
            dataCollection.Add(validData3);

            Assert.AreEqual(3, dataCollection.Count);
        }
    }
}