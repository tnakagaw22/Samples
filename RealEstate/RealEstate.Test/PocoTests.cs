using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Test
{
    [TestClass]
    public class PocoTests
    {
        public PocoTests()
        {
            JsonWriterSettings.Defaults.Indent = true;
        }

        public class Person
        {
            [BsonId] // default is _id
            public int PersonId { get; set; }
            public string FirstName { get; set; }
            public int Age { get; set; }

            public List<string> Address = new List<string>();

            [BsonIgnoreIfNull] // not store
            public Contact Contact = new Contact();

            [BsonIgnore]
            public string IgnoreMe { get; set; }
            [BsonElement("New")]
            public string Old { get; set; }

        }

        public class Contact
        {
            public string Email { get; set; }
            public string Phone { get; set; }
        }

        [TestMethod]
        public void Automatic()
        {
            var person = new Person()
            {
                Age = 54,
                FirstName = "bob"
            };
            person.Address.Add("101 some road");
            person.Address.Add("Unit 401");
            person.Contact.Email = "email@email.com";
            person.Contact.Phone = "123-456-7899";

            Console.WriteLine(person.ToJson());
        }

        [TestMethod]
        public void SerializationAttributes()
        {
            var person = new Person();

            Console.WriteLine(person.ToJson()); ;
        }
    }
}
