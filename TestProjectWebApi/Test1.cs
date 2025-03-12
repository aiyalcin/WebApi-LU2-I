using Microsoft.Extensions.Logging;
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
        private Mock<ILogger<EnvironmentController>> _mockLogger;
        private EnvironmentController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IEnvironmentRepo>();
            _mockLogger = new Mock<ILogger<EnvironmentController>>();
            _controller = new EnvironmentController(_mockRepo.Object, _mockLogger.Object);
        }

        /*
        * Acceptatiecriterium: Wanneer de SaveEnvironment methode wordt aangeroepen met een geldig EnvironmentItem, 
        * moet de SaveEnvironment methode van de IEnvironmentRepo precies één keer worden aangeroepen met hetzelfde EnvironmentItem.
        */
        [TestMethod]
        public async Task SaveEnvironment_CallsSaveEnvironmentOnRepo()
        {
            var environmentItem = new EnvironmentItem
            {
                WorldId = "1",
                WorldName = "TestWorld",
                Height = 100,
                Width = 100,
                Username = "test@example.com"
            };

            await _controller.SaveEnvironment(environmentItem);

            _mockRepo.Verify(repo => repo.SaveEnvironment(environmentItem), Times.Once);
        }


        /*
         * Acceptatiecriterium: Wanneer de CheckExits methode wordt aangeroepen met een bestaande
         * combinatie van email en worldName, moet de methode true retourneren en een informatie logbericht
         * genereren dat aangeeft dat de CheckExits methode is aangeroepen.
         */
        [TestMethod]
        public async Task CheckExits_ReturnsTrueIfExists()
        {
            var email = "test@example.com";
            var worldName = "TestWorld";
            var exists = true;

            _mockRepo.Setup(repo => repo.CheckExits(email, worldName)).ReturnsAsync(exists);

            var result = await _controller.CheckExits(email, worldName);

            Assert.IsTrue(result);
            _mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("CheckExits called")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }

        [TestMethod]
        public async Task CheckExits_ReturnsFalseIfNotExists()
        {
            var email = "test@example.com";
            var worldName = "NonExistentWorld";
            var exists = false;

            _mockRepo.Setup(repo => repo.CheckExits(email, worldName)).ReturnsAsync(exists);

            var result = await _controller.CheckExits(email, worldName);

            Assert.IsFalse(result);
            _mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("CheckExits called")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }
    }

    [TestClass]
    public class TilesControllerTests
    {
        /*
         * Wanneer de SaveTiles methode wordt aangeroepen met een geldige lijst van Tile2DItem objecten,
         * moet de SaveTiles methode van de ITilesRepo precies één keer worden aangeroepen met dezelfde lijst
         * van Tile2DItem objecten.
         */
        private Mock<ITilesRepo> _mockRepo;
        private TilesController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<ITilesRepo>();
            _controller = new TilesController(_mockRepo.Object);
        }

        [TestMethod]
        public async Task SaveTiles_CallsSaveTilesOnRepo()
        {
            var tileList = new TilesController.Tile2DItemList
            {
                Tiles = new List<Tile2DItem>
                {
                    new Tile2DItem { Id = "1", TileName = "Tile1", PositionX = 0, PositionY = 0, WorldId = "1" },
                    new Tile2DItem { Id = "2", TileName = "Tile2", PositionX = 1, PositionY = 1, WorldId = "1" }
                }
            };

            await _controller.SaveTiles(tileList);

            _mockRepo.Verify(repo => repo.SaveTiles(tileList.Tiles), Times.Once);
        }
    }
}
