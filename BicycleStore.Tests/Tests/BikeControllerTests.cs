using BicycleStore.Controllers;
using BicycleStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;

namespace BicycleStore.Tests.Tests
{
    public class BikeControllerTests
    {
        private readonly Mock<MockHttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<BikeController>> _loggerMock;
        private readonly BikeController _controller;

        public BikeControllerTests()
        {
            _httpMessageHandlerMock = new Mock<MockHttpMessageHandler>() { CallBase = true };
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _loggerMock = new Mock<ILogger<BikeController>>();
            _controller = new BikeController(_httpClient, _loggerMock.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfBikes()
        {
            // Arrange
            var mockBikes = new List<Bike>
            {
                new Bike { Id = 1, Model = "Bike1", Category = "Road", Price = 1000, SupplierID = 1 },
                new Bike { Id = 2, Model = "Bike2", Category = "Mountain", Price = 1500, SupplierID = 2 }
            };
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(mockBikes))
            };

            _httpMessageHandlerMock
                .Setup(handler => handler.MockSendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Bike>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenBikeIsNull()
        {
            // Arrange
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("null")
            };

            _httpMessageHandlerMock
                .Setup(handler => handler.MockSendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithBike()
        {
            // Arrange
            var mockBike = new Bike { Id = 1, Model = "Bike1", Category = "Road", Price = 1000, SupplierID = 1 };
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(mockBike))
            };

            _httpMessageHandlerMock
                .Setup(handler => handler.MockSendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Bike>(viewResult.ViewData.Model);
            Assert.Equal(mockBike.Id, model.Id);
        }
    }
}
