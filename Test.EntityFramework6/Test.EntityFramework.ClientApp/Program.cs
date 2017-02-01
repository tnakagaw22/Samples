using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.EntityFramework6.Data;
using Test.EntityFramework6.Data.Models;

namespace Test.EntityFramework.ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("starting");
            using (var context = new TestEntityFrameworkContext())
            {
                context.Database.Log = Console.WriteLine;
                var book = context.Books.Find(4);
                //var book = context.Books.Single(p => p.Id == 2);
                Console.WriteLine("book is retrived " + book.Title);
                var bookFromMemeory = context.Books.Find(2);
                //var bookFromMemeory = context.Books.Single(p => p.Id == 2);
                Console.WriteLine("book is retrived again " + book.Title);
                //context.Books.Add(new Book()
                //{
                //    Title = "First Book",
                //    PublishedDate =DateTime.Now
                //});

                //context.SaveChanges();
            }

            Console.WriteLine("end");
            Console.ReadLine();
        }
    }
}
