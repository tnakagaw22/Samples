using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace RealEstate.Test
{
    [TestClass]
    public class BsonDocumentTests
    {

        public BsonDocumentTests()
        {
            JsonWriterSettings.Defaults.Indent = true;
        }

        [TestMethod]
        public void EmptyDocument()
        {
            var document = new BsonDocument();

            Console.WriteLine(document);
        }

        [TestMethod]
        public void AddElements()
        {
            var person = new BsonDocument()
            {
                {"age", new BsonInt32(54) },
                {"isAlive", true }
            };
            person.Add("firstName", new BsonString("bob"));

            Console.WriteLine(person);
        }

        [TestMethod]
        public void AddingArrays()
        {
            var person = new BsonDocument();
            person.Add("address",
                new BsonArray(new[] { "101 Some Road", "Unit 401" }));

            Console.WriteLine(person);
        }

        [TestMethod]
        public void EmbeddedDocument()
        {
            var person = new BsonDocument()
            {
                {
                    "contact", new BsonDocument
                    {
                        {"phone", "123-456-7890" },
                        {"email", "whatever@email.com" }
                    }
                }
            };

            Console.WriteLine(person);
        }

        [TestMethod]
        public void BsonValueConversions()
        {
            var person = new BsonDocument()
            {
                {"age", 54}
            };

            Console.WriteLine(person["age"].AsInt32 + 10);
            Console.WriteLine(person["age"].IsInt32);
            Console.WriteLine(person["age"].IsString);
        }

        [TestMethod]
        public void ToBson()
        {
            var person = new BsonDocument()
            {
                {"firstName", "bob"}
            };

            var bson = person.ToBson();

            Console.WriteLine(BitConverter.ToString(bson));

            var deserializedPerson = BsonSerializer.Deserialize<BsonDocument>(bson);
            Console.WriteLine(deserializedPerson);
        }
    }
}
