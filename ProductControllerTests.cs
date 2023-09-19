using backend.Controllers;
using backend.Models;
using backend.Repository;
using backend.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace backend.Tests.Controllers
{
    public class ProductsControllerTests
    {
        [Fact]
        public async Task GetProduct_ReturnsOkResult_WhenProductExists()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var loggerMock = new Mock<ILogger<ProductsController>>();

            var sampleProductId = 1; // Set the expected product ID
            var sampleProduct = new Product
            {
                ProductId = sampleProductId,
                // Initialize other product properties
                Name = "Name1",
                Description = "Description",
                Price = 12.5,
                Code = "123asc",
                Brand = "zahra"
            };

            productRepositoryMock
                .Setup(repo => repo.GetProductAsync(sampleProductId))
                .ReturnsAsync(sampleProduct);

            var controller = new ProductsController(productRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await controller.GetProduct(sampleProductId);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Check that it's an OK result
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value); // Check that the value is not null
            Assert.Equal(sampleProduct, okResult.Value); // Check that the returned product matches the sample product
        }

        [Fact]
        public async Task GetProduct_ReturnsNotFoundResult_WhenProductNotFound()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var loggerMock = new Mock<ILogger<ProductsController>>();

            var sampleProductId = 1; // Set the expected product ID

            productRepositoryMock
                .Setup(repo => repo.GetProductAsync(sampleProductId))
                .Throws<ProductNotFoundException>();

            var controller = new ProductsController(productRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await controller.GetProduct(sampleProductId);

            // Assert
            Assert.IsType<NotFoundResult>(result); // Check that it's a NotFound result
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreatedAtActionResult_WhenProductIsCreated()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var loggerMock = new Mock<ILogger<ProductsController>>();

            var sampleUserId = 1; // Set the user ID as needed
            var sampleProductDTO = new ProductDTO
            {
                // Initialize product DTO properties
            };

            productRepositoryMock
                .Setup(repo => repo.CreateProduct(sampleUserId, sampleProductDTO))
                .ReturnsAsync(new Product { /* Initialize created product properties */ });

            var controller = new ProductsController(productRepositoryMock.Object, loggerMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.PrimarySid, sampleUserId.ToString())
                    }, "test"))
                }
            };

            // Act
            var result = await controller.CreateProduct(sampleProductDTO);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result); // Check that it's a CreatedAtAction result
        }

       
        [Fact]
        public async Task UpdateProduct_ReturnsOkResult_WhenProductIsUpdated()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var loggerMock = new Mock<ILogger<ProductsController>>();

            var sampleUserId = 1; // Set the user ID as needed
            var sampleProductId = 1; // Set the expected product ID
            var sampleProductDTO = new ProductDTO
            {
                // Initialize product DTO properties for the update
            };

            var updatedProduct = new Product
            {
                ProductId = sampleProductId,
                // Initialize other product properties after the update
            };

            productRepositoryMock
                .Setup(repo => repo.UpdateProduct(sampleUserId, sampleProductId, sampleProductDTO))
                .ReturnsAsync(updatedProduct);

            var controller = new ProductsController(productRepositoryMock.Object, loggerMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.PrimarySid, sampleUserId.ToString())
                    }, "test"))
                }
            };

            // Act
            var result = await controller.UpdateProduct(sampleProductId, sampleProductDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Check that it's an OK result
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value); // Check that the value is not null
            Assert.Equal(updatedProduct, okResult.Value); // Check that the returned product matches the updated product
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNotFoundResult_WhenProductNotFound()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var loggerMock = new Mock<ILogger<ProductsController>>();

            var sampleUserId = 1; // Set the user ID as needed
            var sampleProductId = 1; // Set the expected product ID
            var sampleProductDTO = new ProductDTO
            {
                // Initialize product DTO properties for the update
            };

            productRepositoryMock
                .Setup(repo => repo.UpdateProduct(sampleUserId, sampleProductId, sampleProductDTO))
                .Throws<ProductNotFoundException>();

            var controller = new ProductsController(productRepositoryMock.Object, loggerMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.PrimarySid, sampleUserId.ToString())
                    }, "test"))
                }
            };

            // Act
            var result = await controller.UpdateProduct(sampleProductId, sampleProductDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result); // Check that it's a NotFound result
        }

        [Fact]
        public async Task DeleteProduct_ReturnsOkResult_WhenProductIsDeleted()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var loggerMock = new Mock<ILogger<ProductsController>>();

            var sampleUserId = 1; // Set the user ID as needed
            var sampleProductId = 1; // Set the expected product ID

            productRepositoryMock
                .Setup(repo => repo.DeleteProduct(sampleUserId, sampleProductId))
                .ReturnsAsync(true); // Simulate successful deletion

            var controller = new ProductsController(productRepositoryMock.Object, loggerMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.PrimarySid, sampleUserId.ToString())
                    }, "test"))
                }
            };

            // Act
            var result = await controller.DeleteProduct(sampleProductId);

            // Assert
            Assert.IsType<OkObjectResult>(result); // Check that it's an OK result
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("product deleted", okResult.Value); // Check the expected message
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNotFoundResult_WhenProductNotFound()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var loggerMock = new Mock<ILogger<ProductsController>>();

            var sampleUserId = 1; // Set the user ID as needed
            var sampleProductId = 1; // Set the expected product ID

            // Simulate that the product is not found
            productRepositoryMock
                .Setup(repo => repo.DeleteProduct(sampleUserId, sampleProductId))
                .Throws<ProductNotFoundException>();

            var controller = new ProductsController(productRepositoryMock.Object, loggerMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.PrimarySid, sampleUserId.ToString())
                    }, "test"))
                }
            };

            // Act
            var result = await controller.DeleteProduct(sampleProductId);

            // Assert
            Assert.IsType<NotFoundResult>(result); // Check that it's a NotFound result
        }


        [Fact]
        public async Task FetchAllProducts_ReturnsOkResult_WithProducts()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var loggerMock = new Mock<ILogger<ProductsController>>();

            var sampleProducts = new List<Product>
            {
                new Product { /* Initialize product properties */ },
                new Product { /* Initialize another product properties */ }
                // Add more sample products as needed
            };

            productRepositoryMock
                .Setup(repo => repo.GetAllProducts(-1, 1, 10))
                .ReturnsAsync(sampleProducts);

            var controller = new ProductsController(productRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await controller.FetchAllProducts();

            // Assert
            // var okResult = Assert.IsType<OkObjectResult>(result); // Check that it's an OK result
            Assert.NotNull(result); // Check that the value is not null
            var returnedProducts = Assert.IsAssignableFrom<List<Product>>(result);
            Assert.Equal(sampleProducts, returnedProducts); 
        }
    }
}
