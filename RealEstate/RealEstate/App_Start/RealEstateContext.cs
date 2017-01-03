using MongoDB.Driver;
using RealEstate.Properties;
using RealEstate.Rentals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.App_Start
{
    public class RealEstateContext
    {
        public MongoDatabase Database { get; set; }

        public RealEstateContext()
        {
            var client = new MongoClient(Settings.Default.ReadEstateConnectionString);
            var server = client.GetServer();
            Database = server.GetDatabase(Settings.Default.RealEstateDatabaseName);
        }

        public MongoCollection<Rental> Rentals
        {
            get
            {
                return Database.GetCollection<Rental>("rentals");
            }
        }
    }
}