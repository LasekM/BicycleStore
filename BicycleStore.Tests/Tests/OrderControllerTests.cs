using BicycleStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Threading;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BicycleStore.Tests
{
    public class OrderControllerTests
    {
        private readonly Mock<MockHttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<BicycleStore.Controllers.OrderController>> _loggerMock;
        private readonly BicycleStore.Controllers.OrderController _controller;

        public OrderControllerTests()
        {
            _httpMessageHandlerMock = new Mock<MockHttpMessageHandler>() { CallBase = true };
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _loggerMock = new Mock<ILogger<BicycleStore.Controllers.OrderController>>();
            _controller = new BicycleStore.Controllers.OrderController(_httpClient, _loggerMock.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfOrders()
        {
            
            var mockOrders = new List<Order>
            {
                new Order { OrderId = 1, UserName = "user1", OrderDate = DateTime.Now },
                new Order { OrderId = 2, UserName = "user2", OrderDate = DateTime.Now }
            };
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(mockOrders))
            };

            _httpMessageHandlerMock
                .Setup(handler => handler.MockSendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse);

            
            var result = await _controller.Index();

            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Order>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenOrderIsNull()
        {
            
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("null")
            };

            _httpMessageHandlerMock
                .Setup(handler => handler.MockSendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse);

            
            var result = await _controller.Details(1);

            
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithOrder()
        {
            
            var mockOrder = new Order { OrderId = 1, UserName = "user1", OrderDate = DateTime.Now };
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(mockOrder))
            };

            _httpMessageHandlerMock
                .Setup(handler => handler.MockSendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse);

            
            var result = await _controller.Details(1);

            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Order>(viewResult.ViewData.Model);
            Assert.Equal(mockOrder.OrderId, model.OrderId);
        }

        [Fact]
        public async Task Create_ReturnsViewResult_WithBikesSelectList()
        {
            
            var mockBikes = new List<Bike>
            {
                new Bike { Id = 1, Model = "Bike1", IsReserved = false },
                new Bike { Id = 2, Model = "Bike2", IsReserved = false }
            };
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(mockBikes))
            };

            _httpMessageHandlerMock
                .Setup(handler => handler.MockSendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse);

            
            var result = await _controller.Create();

            
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(viewResult.ViewData.ContainsKey("Bikes"));
            var selectList = viewResult.ViewData["Bikes"] as SelectList;
            Assert.NotNull(selectList);
            Assert.Equal(2, selectList.Count());
        }

        [Fact]
        public async Task DetailsUser_ReturnsNotFound_WhenOrderIsNull()
        {
            
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("null")
            };

            _httpMessageHandlerMock
                .Setup(handler => handler.MockSendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse);

            
            var result = await _controller.DetailsUser(1);

            
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DetailsUser_ReturnsViewResult_WithOrder()
        {
            
            var mockOrder = new Order { OrderId = 1, UserName = "user1", OrderDate = DateTime.Now };
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(mockOrder))
            };

            _httpMessageHandlerMock
                .Setup(handler => handler.MockSendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse);

            
            var result = await _controller.DetailsUser(1);

            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Order>(viewResult.ViewData.Model);
            Assert.Equal(mockOrder.OrderId, model.OrderId);
        }
    }
}
