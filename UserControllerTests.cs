using backend.Controllers;
using backend.Models;
using backend.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;


namespace backend.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public async Task LoginUser_ReturnsOkResult_WithToken()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var loggerMock = new Mock<ILogger<UserController>>();

            var sampleUserAuthDTO = new UserAuthDTO
            {
                Email = "test@example.com",
                Password = "password"
            };

            var sampleToken = "sample_token"; // Replace with an actual token value

            userRepositoryMock
                .Setup(repo => repo.AuthenticateUser(sampleUserAuthDTO))
                .ReturnsAsync(sampleToken);

            var controller = new UserController(userRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await controller.LoginUser(sampleUserAuthDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Check that it's an OK result
            Assert.NotNull(okResult.Value); // Check that the value is not null
            var token = Assert.IsType<string>(okResult.Value);
            Assert.Equal(sampleToken, token); // Check that the returned token matches the sample token
        }

        [Fact]
        public async Task LoginUser_ReturnsBadRequestResult_WhenAuthenticationFails()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var loggerMock = new Mock<ILogger<UserController>>();

            var sampleUserAuthDTO = new UserAuthDTO
            {
                Email = "test@example.com",
                Password = "password"
            };

            userRepositoryMock
                .Setup(repo => repo.AuthenticateUser(sampleUserAuthDTO))
                .Throws<Exception>();

            var controller = new UserController(userRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await controller.LoginUser(sampleUserAuthDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result); // Check that it's a BadRequest result
            Assert.NotNull(badRequestResult.Value); // Check that the value is not null
            var message = Assert.IsType<string>(badRequestResult.Value);
            // Assert.Equal("", message); // Check the expected error message
        }

        [Fact]
        public async Task CreateUser_ReturnsOkResult_WhenAccountIsCreated()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var loggerMock = new Mock<ILogger<UserController>>();

            var sampleUserAuthDTO = new UserAuthDTO
            {
                Email = "test@example.com",
                Password = "password"
            };

            userRepositoryMock
                .Setup(repo => repo.CreateUser(sampleUserAuthDTO))
                .ReturnsAsync(true); // Simulate successful account creation

            var controller = new UserController(userRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await controller.CreateUser(sampleUserAuthDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Check that it's an OK result
            Assert.NotNull(okResult.Value); // Check that the value is not null
            var message = Assert.IsType<string>(okResult.Value);
            Assert.Equal("account created", message); // Check the expected message
        }

        [Fact]
        public async Task CreateUser_ReturnsBadRequestResult_WhenAccountAlreadyExists()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var loggerMock = new Mock<ILogger<UserController>>();

            var sampleUserAuthDTO = new UserAuthDTO
            {
                Email = "test@example.com",
                Password = "password"
            };

            userRepositoryMock
                .Setup(repo => repo.CreateUser(sampleUserAuthDTO))
                .ReturnsAsync(false); // Simulate that the account already exists

            var controller = new UserController(userRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await controller.CreateUser(sampleUserAuthDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result); // Check that it's a BadRequest result
            Assert.NotNull(badRequestResult.Value); // Check that the value is not null
            var message = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("an account with similar details already exists", message); // Check the expected error message
        }
    }
}
