using Microsoft.Testing.Platform.Logging;
using Moq;
using WebApi.Controllers;
using WebApi.DataBase;
using WebApi.Items;

namespace TestProjectWebApi
{
    [TestClass]
    public class EnvironmentControllerTests
    {
        private Mock<IEnvironmentRepo> _mockRepo;
        private EnvironmentController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IEnvironmentRepo>();
            _controller = new EnvironmentController(_mockRepo.Object);
        }

        [TestMethod]
        public async Task SaveEnvironment_CallsSaveEnvironmentOnRepo()
        {
            // Arrange
            var environmentItem = new EnvironmentItem
            {
                WorldId = "1",
                WorldName = "TestWorld",
                Height = 100,
                Width = 100,
                Username = "test@example.com"
            };

            // Act
            await _controller.SaveEnvironment(environmentItem);

            // Assert
            _mockRepo.Verify(repo => repo.SaveEnvironment(environmentItem), Times.Once);
        }

        [TestMethod]
        public async Task CheckExits_ReturnsTrueIfExists()
        {
            // Arrange
            var email = "test@example.com";
            var worldName = "TestWorld";
            var exists = true;

            _mockRepo.Setup(repo => repo.CheckExits(email, worldName)).ReturnsAsync(exists);

            // Act
            var result = await _controller.CheckExits(email, worldName);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task CheckExits_ReturnsFalseIfNotExists()
        {
            // Arrange
            var email = "test@example.com";
            var worldName = "NonExistentWorld";
            var exists = false;

            _mockRepo.Setup(repo => repo.CheckExits(email, worldName)).ReturnsAsync(exists);

            // Act
            var result = await _controller.CheckExits(email, worldName);

            // Assert
            Assert.IsFalse(result);
        }

    }

}
