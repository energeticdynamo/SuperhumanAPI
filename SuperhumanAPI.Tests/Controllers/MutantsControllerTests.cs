using Microsoft.AspNetCore.Mvc;
using Moq;
using SuperhumanAPI.Controllers;
using SuperhumanAPI.Models;
using SuperhumanAPI.Repositories.Interfaces;

namespace SuperhumanAPI.Tests.Controllers
{
    public class MutantsControllerTests
    {
        private readonly Mock<IMutantRepository> _mockRepo;
        private readonly MutantsController _controller;

        public MutantsControllerTests()
        {
            _mockRepo = new Mock<IMutantRepository>();
            _controller = new MutantsController(_mockRepo.Object);
        }

        // Get by Id

        [Fact]
        public async Task GetMutantById_ReturnsOk_WhenMutantExists()
        {
            // Arrange
            var mutant = new Mutant { MutantId = 1, PrimaryPower = "Telepathy" };
            _mockRepo.Setup(repo => repo.GetMutantByIdAsync(1)).ReturnsAsync(mutant);

            // Act
            var result = await _controller.GetMutantById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(mutant, okResult.Value);
        }

        [Fact]
        public async Task GetMutantById_ReturnsNotFound_WhenMutantIsNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetMutantByIdAsync(99)).ReturnsAsync((Mutant)null!);

            // Act
            var result = await _controller.GetMutantById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        // ── GET all (paged) ────────────────────────────

        [Fact]
        public async Task GetAllMutants_ReturnsOk_WithPagedResult()
        {
            // Arrange
            var paged = new PagedResult<Mutant>
            {
                Items = [new Mutant { MutantId = 1, PrimaryPower = "Flight" }],
                TotalCount = 1
            };
            _mockRepo.Setup(r => r.GetAllMutantsAsync(1, 10)).ReturnsAsync(paged);

            // Act
            var result = await _controller.GetAllMutantsAsync(1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(paged, okResult.Value);
        }

        // ── POST (create) ──────────────────────────────
        [Fact]
        public async Task CreateMutant_ReturnsCreatedAtAction_WhenValid()
        {
            // Arrange
            var mutant = new Mutant { MutantId = 5, PrimaryPower = "Healing" };

            // Act
            var result = await _controller.CreateMutant(mutant);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(mutant, created.Value);
            _mockRepo.Verify(r => r.AddMutantAsync(mutant), Times.Once);
        }

        [Fact]
        public async Task CreateMutant_ReturnsBadRequest_WhenMutantIsNull()
        {
            // Act
            var result = await _controller.CreateMutant(null!);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        // ── DELETE ──────────────────────────────────────

        [Fact]
        public async Task DeleteMutant_ReturnsNoContent_WhenDeleted()
        {
            // Arrange
            _mockRepo.Setup(r => r.DeleteMutantByMutantIdAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteMutant(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteMutant_ReturnsNotFound_WhenNotExists()
        {
            // Arrange
            _mockRepo.Setup(r => r.DeleteMutantByMutantIdAsync(99)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteMutant(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // ── PUT (update) ───────────────────────────────

        [Fact]
        public async Task UpdateMutant_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var mutant = new Mutant { MutantId = 2, PrimaryPower = "Speed" };

            // Act
            var result = await _controller.UpdateMutant(999, mutant);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateMutant_ReturnsNotFound_WhenMutantDoesNotExist()
        {
            // Arrange
            var mutant = new Mutant { MutantId = 1, PrimaryPower = "Speed" };
            _mockRepo.Setup(r => r.GetMutantByIdAsync(1)).ReturnsAsync((Mutant)null!);

            // Act
            var result = await _controller.UpdateMutant(1, mutant);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateMutant_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var mutant = new Mutant { MutantId = 1, PrimaryPower = "Speed" };
            _mockRepo.Setup(r => r.GetMutantByIdAsync(1)).ReturnsAsync(mutant);

            // Act
            var result = await _controller.UpdateMutant(1, mutant);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockRepo.Verify(r => r.UpdateMutantAsync(mutant), Times.Once);
        }
    }
}
