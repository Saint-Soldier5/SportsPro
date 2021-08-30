using System;
using Xunit;
using SportsPro.Areas.Admin.Controllers;
using SportsProTest.FakeClassesNotUsed;
using SportsPro.Controllers;
using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using Moq;

namespace SportsProTest
{
    public class ProductControllerTests
    {
        [Fact]
        public void IndexActionMethod_ReturnsAViewResult()
        {
            //FakeProductRepository - not used
            //arange
            /*var rep = new FakeProductRepository();
            var controller = new ProductController(rep);
            */

            // Moq
            // arrange
            var rep = new Mock<IRepository<Product>>();
            var controller = new ProductController(rep.Object);

            //act
            var result = controller.Index();

            //assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
