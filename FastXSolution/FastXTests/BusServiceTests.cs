using Castle.Core.Logging;
using FastX.Contexts;
using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Repositories;
using FastX.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace FastXTests
{
    public class Tests
    {
        private BusService _busService;
        private Mock<IRepository<int, Bus>> _mockBusRepo;
        private Mock<IRepository<int, BusOperator>> _mockBusOperatorRepo;
        private Mock<ILogger<BusService>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FastXContext>().UseInMemoryDatabase("dummyDatabase").Options;

            // Create a mock repository
            _mockBusRepo = new Mock<IRepository<int, Bus>>();
            _mockBusOperatorRepo = new Mock<IRepository<int, BusOperator>>();
            _mockLogger = new Mock<ILogger<BusService>>();

            // Pass the mock repository to the BusService
            _busService = new BusService(_mockBusRepo.Object, _mockBusOperatorRepo.Object, _mockLogger.Object);
        }

        [Test]
        public async Task AddBusTests()
        {
            //Arrange
            // Arrange
            string busName = "pinkBus";
            string busType = "ac seater";
            int totalSeats = 20;
           int busOperatorId = 1;

            // Arrange: Insert a valid BusOperator into the in-memory database
            var validBusOperator = new BusOperator
            {
                BusOperatorId = 1,
                Username = "busop1",


            };

            // Set up mock behavior for BusOperator repository
            _mockBusOperatorRepo.Setup(repo => repo.GetAsync(busOperatorId)).ReturnsAsync(validBusOperator);


            // Set up mock behavior for Bus repository
            _mockBusRepo.Setup(repo => repo.Add(It.IsAny<Bus>())).ReturnsAsync((Bus addedBus) =>
            {
                // Simulate the repository behavior: return the added bus
                return addedBus;
            });

            // Act
            var addedBus = await _busService.AddBus(busName, busType, totalSeats, busOperatorId);

            // Assert
            // Assert
            Assert.That(addedBus.BusName, Is.EqualTo(busName));

            

           
        }


        [Test]
        public async Task AddBusTests_BusOperatorNotFound()
        {
            // Arrange
            string busName = "pinkBus";
            string busType = "ac seater";
            int totalSeats = 20;
            int busOperatorId = 1;

            // Arrange: Simulate that BusOperator with the given id is not found
            _mockBusOperatorRepo.Setup(repo => repo.GetAsync(busOperatorId)).ReturnsAsync((BusOperator)null);

            // Act and Assert
            Assert.ThrowsAsync<BusOperatorNotFoundException>(async () =>
            {
                var addedBus = await _busService.AddBus(busName, busType, totalSeats, busOperatorId);
            });
        }

    }
}
