using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Project.Controllers;
using MVC.Project.Models;
using MVC.Project.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RealWorldUnitTest
{
    public class ProductControllerTestWithInMemoryData : ProductControllerTest
    {
        public ProductControllerTestWithInMemoryData()
        {
            SetContextOptions(new DbContextOptionsBuilder<NorthwindContext>().UseInMemoryDatabase("NorthwindInMemoryDB").Options);
            SeedData();
        }

        [Fact]
        public async Task Create_ModelValidProduct_ReturnRedirectToActionWithSaveProduct()
        {
            using (NorthwindContext northwindContext = new NorthwindContext(Options))
            {
                var category = northwindContext.Categories.First();

                var product = new Product { ProductId = 6, ProductName = "Product Create", UnitPrice = 5, CategoryId = 1, UnitsInStock = 7 };
                var controller = new ProductsController(new Repository<Product>(northwindContext), new Repository<Category>(northwindContext), new Repository<Supplier>(northwindContext));

                var result = await controller.Create(product);
                var redirect = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirect.ActionName);
            }
        }
    }
}
