using Microsoft.EntityFrameworkCore;
using MVC.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealWorldUnitTest
{
    public class ProductControllerTest
    {
        protected DbContextOptions<NorthwindContext> Options { get; private set; }

        public void SetContextOptions(DbContextOptions<NorthwindContext> options)
        {
            Options = options;
        }

        public void SeedData()
        {
            using (NorthwindContext northwindContext = new NorthwindContext(Options))
            {
                northwindContext.Database.EnsureDeleted();
                northwindContext.Database.EnsureCreated();

                northwindContext.Categories.Add(new Category { CategoryId = 1, CategoryName = "Test 1", Description = "test 1 desc" });
                northwindContext.Categories.Add(new Category { CategoryId = 2, CategoryName = "Test 2", Description = "test 2 desc" });
                northwindContext.SaveChanges();

                northwindContext.Products.Add(new Product { ProductId = 1, ProductName = "Product 1", UnitPrice = 5, CategoryId = 1, UnitsInStock = 7 });
                northwindContext.Products.Add(new Product { ProductId = 2, ProductName = "Product 2", UnitPrice = 5, CategoryId = 1, UnitsInStock = 7 });
                northwindContext.Products.Add(new Product { ProductId = 3, ProductName = "Product 3", UnitPrice = 5, CategoryId = 2, UnitsInStock = 73 });
                northwindContext.Products.Add(new Product { ProductId = 4, ProductName = "Product 4", UnitPrice = 5, CategoryId = 2, UnitsInStock = 27 });
                northwindContext.SaveChanges();
            }
        }
    }
}
