using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using MVC.Project.Controllers;
using MVC.Project.Models;
using MVC.Project.Repository;
using MVC.Project.Test.App_Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MVC.Project.Test
{
    public class ProductsControllerTest
    {
        private readonly Mock<IRepository<Product>> _mock;
        private readonly Mock<IRepository<Category>> _mockCategory;
        private readonly Mock<IRepository<Supplier>> _mockSupplier;
        private readonly ProductsController _productsController;
        private readonly List<Product> _products;
        public ProductsControllerTest()
        {
            _mock = new Mock<IRepository<Product>>();
            _mockCategory = new Mock<IRepository<Category>>();
            _mockSupplier = new Mock<IRepository<Supplier>>();
            _productsController = new ProductsController(_mock.Object, _mockCategory.Object, _mockSupplier.Object);
            _products = new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    ProductName = "Book",
                    CategoryId = 2,
                    SupplierId = 1,
                    UnitPrice = 5,
                    UnitsInStock = 25,
                    Discontinued = true
                },
                new Product
                {
                    ProductId = 2,
                    ProductName = "CopyBook",
                    CategoryId = 2,
                    SupplierId = 1,
                    UnitPrice = 2,
                    UnitsInStock = 17,
                    Discontinued = true
                },
                new Product
                {
                    ProductId = 3,
                    ProductName = "Phone",
                    CategoryId = 1,
                    SupplierId = 1,
                    UnitPrice = 95,
                    UnitsInStock = 15,
                    Discontinued = true
                }
            };
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnView()
        {
            var result = await _productsController.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnProductList()
        {
            _mock.Setup(x => x.GetAll()).ReturnsAsync(_products);
            dynamic result = await _productsController.Index();
            
            var viewResult = Assert.IsType<ViewResult>(result);
            var productsList = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);
            Assert.Equal<int>(3, productsList.Count);
        }

        [Fact]
        public async void Details_IdIsNull_ReturnRedirectToIndexAction()
        {
            var result = await _productsController.Details(null);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async void Details_InvalidId_ReturnNotFound()
        {
            Product product = null;
            _mock.Setup(x => x.GetById(0)).ReturnsAsync(product);
            var result = await _productsController.Details(0);
            var notFound = Assert.IsType<NotFoundResult>(result);
            Assert.Equal<int>(404, notFound.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        public async void Details_ValidId_ReturnDetailsViewWithProductData(int id)
        {
            Product product = _products.First(x => x.ProductId == id);
            _mock.Setup(x => x.GetById(id)).ReturnsAsync(product);

            var result = await _productsController.Details(id);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            var viewModel = Assert.IsAssignableFrom<Product>(viewResult.Model);
            Assert.Equal<int>(product.ProductId, viewModel.ProductId);
            Assert.Equal(product.ProductName, viewModel.ProductName);
        }

        [Fact]
        public void Create_GetActionExecutes_ReturnView()
        {
            List<Category> categories = new List<Category>
            {
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Test",
                    Description = "Test",
                }
            };
            List<Supplier> suppliers = new List<Supplier>
            {
                new Supplier
                {
                    SupplierId = 1,
                    CompanyName = "Test",
                    ContactName = "Test",
                }
            };

            DbSet<Category> myDbSetCategory = Tools.GetQueryableMockDbSet<Category>(categories);
            DbSet<Supplier> myDbSetSupplier = Tools.GetQueryableMockDbSet<Supplier>(suppliers);

            _mockCategory.Setup(x => x.GetDbSet()).Returns(myDbSetCategory);
            _mockSupplier.Setup(x => x.GetDbSet()).Returns(myDbSetSupplier);
           var result = _productsController.Create();
           var viewResult = Assert.IsType<ViewResult>(result);
        }
    
        [Fact]
        public async void Create_PostActionWithInvalidData_ReturnView()
        {
            List<Category> categories = new List<Category>
            {
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Test",
                    Description = "Test",
                }
            };
            List<Supplier> suppliers = new List<Supplier>
            {
                new Supplier
                {
                    SupplierId = 1,
                    CompanyName = "Test",
                    ContactName = "Test",
                }
            };

            DbSet<Category> myDbSetCategory = Tools.GetQueryableMockDbSet<Category>(categories);
            DbSet<Supplier> myDbSetSupplier = Tools.GetQueryableMockDbSet<Supplier>(suppliers);

            _mockCategory.Setup(x => x.GetDbSet()).Returns(myDbSetCategory);
            _mockSupplier.Setup(x => x.GetDbSet()).Returns(myDbSetSupplier);

            Product product = _products.First();
            _productsController.ModelState.AddModelError("Name", "Name is invalid");
            var result = await _productsController.Create(product);
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewResultData = Assert.IsAssignableFrom<Product>(viewResult.Model);
            Assert.Equal(product.ProductId, viewResultData.ProductId);
        }
    
        [Fact]
        public async void Create_PostActionWithValidData_ReturnRedirectToIndexAction()
        {
            Product product = new Product();
            _mock.Setup(x => x.Create(It.IsAny<Product>())).Callback<Product>(x => product = x);

            var result = await _productsController.Create(_products.First());
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }
    }
}
