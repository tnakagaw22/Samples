using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.EntityFramework6.Data.Models;

namespace Test.EntityFramework6.Data
{
    public class TestEntityFrameworkContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
    }
}
