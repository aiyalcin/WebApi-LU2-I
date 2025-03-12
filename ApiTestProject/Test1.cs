using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Controllers;
using WebApi.DataBase;
using WebApi.Items;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

[TestClass]
namespace ApiTestProject
{
    [TestClass]
    public class TilesControllerTests
    {
        [TestMethod]
        public async Task ReadTilesAsync_ReturnsTiles()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<EnvironmentRepo>>();
            var mockTilesRepo = new Mock<ITilesRepo>();
            var worldId = "testWorldId";
            var expectedTiles = new List<Tile2DItem?> { new Tile2DItem { Id = "1", TileName = "Tile1", PositionX = 0, PositionY = 0, WorldId = worldId } };

            mockTilesRepo.Setup(repo => repo.ReadTilesAsync(worldId)).ReturnsAsync(expectedTiles);

            var controller = new TilesController(mockConfiguration.Object, mockLogger.Object, mockTilesRepo.Object);

            // Act
            var result = await controller.ReadTilesAsync(worldId);

            // Assert
            CollectionAssert.AreEqual(expectedTiles, result);
        }

        [TestMethod]
        public async Task SaveTiles_CallsSaveTilesOnRepo()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<EnvironmentRepo>>();
            var mockTilesRepo = new Mock<ITilesRepo>();
            var tileList = new TilesController.Tile2DItemList
            {
                Tiles = new List<Tile2DItem> { new Tile2DItem { Id = "1", TileName = "Tile1", PositionX = 0, PositionY = 0, WorldId = "testWorldId" } }
            };

            var controller = new TilesController(mockConfiguration.Object, mockLogger.Object, mockTilesRepo.Object);

            // Act
            await controller.SaveTiles(tileList);

            // Assert
            mockTilesRepo.Verify(repo => repo.SaveTiles(tileList.Tiles), Times.Once);
        }

        [TestMethod]
        public async Task ReadTilesAsync_ReturnsEmptyListWhenNoTiles()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<EnvironmentRepo>>();
            var mockTilesRepo = new Mock<ITilesRepo>();
            var worldId = "testWorldId";
            var expectedTiles = new List<Tile2DItem?>();

            mockTilesRepo.Setup(repo => repo.ReadTilesAsync(worldId)).ReturnsAsync(expectedTiles);

            var controller = new TilesController(mockConfiguration.Object, mockLogger.Object, mockTilesRepo.Object);

            // Act
            var result = await controller.ReadTilesAsync(worldId);

            // Assert
            Assert.AreEqual(0, result.Count);
        }
    }
}
