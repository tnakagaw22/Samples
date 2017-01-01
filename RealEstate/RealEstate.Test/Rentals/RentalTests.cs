using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using RealEstate.Rentals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Test.Rentals
{
    [TestClass]
    public class RentalTests
    {
        [TestMethod]
        public void ToDocument_RentalWithPrice_PriceRepresentedAsDouble()
        {
            var rental = new Rental();
            rental.Price = 1;

            var document = rental.ToBsonDocument();

            Assert.AreEqual(document["Price"].BsonType, BsonType.Double);
        }

        [TestMethod]
        public void ToDocument_RentalWithAnId_IdIsRepresentedAsAnObjectId()
        {
            var rental = new Rental();
            rental.Id = ObjectId.GenerateNewId().ToString();

            var document = rental.ToBsonDocument();

            Assert.AreEqual(document["_id"].BsonType, BsonType.ObjectId);
        }
    }
}
