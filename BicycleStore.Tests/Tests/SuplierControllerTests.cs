using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using BicycleStore.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace BicycleStore.Tests
{
    public class SupplierControllerTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly SupplierController _controller;

        public SupplierControllerTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new System.Uri("https://localhost:7265/")
            };
            _controller = new SupplierController(_httpClient);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfSuppliers()
        {
            // Arrange
            var suppliers = new List<Supplier>
            {
                new Supplier { Id = 1, Name = "Supplier1" },
                new Supplier { Id = 2, Name = "Supplier2" }
            };
            var responseContent = new StringContent(JsonConvert.SerializeObject(suppliers), Encoding.UTF8, "application/json");

            _handlerMock.SetupRequest(HttpMethod.Get, "https://localhost:7265/api/Supplier", responseContent, HttpStatusCode.OK);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Supplier>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithSupplier()
        {
            // Arrange
            var supplier = new Supplier { Id = 1, Name = "Supplier1" };
            var responseContent = new StringContent(JsonConvert.SerializeObject(supplier), Encoding.UTF8, "application/json");

            _handlerMock.SetupRequest(HttpMethod.Get, "https://localhost:7265/api/Supplier/1", responseContent, HttpStatusCode.OK);

            // Act
            var result = await _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Supplier>(viewResult.ViewData.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public async Task Create_Post_ReturnsRedirectToActionResult()
        {
            // Arrange
            var supplier = new Supplier { Id = 1, Name = "New Supplier" };
            var responseContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            _handlerMock.SetupRequest(HttpMethod.Post, "https://localhost:7265/api/Supplier", responseContent, HttpStatusCode.Created);

            // Act
            var result = await _controller.Create(supplier);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Edit_Post_ReturnsRedirectToActionResult()
        {
            // Arrange
            var supplier = new Supplier { Id = 1, Name = "Updated Supplier" };
            var responseContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            _handlerMock.SetupRequest(HttpMethod.Put, "https://localhost:7265/api/Supplier/1", responseContent, HttpStatusCode.NoContent);

            // Act
            var result = await _controller.Edit(supplier);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsRedirectToActionResult()
        {
            // Arrange
            var responseContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            _handlerMock.SetupRequest(HttpMethod.Delete, "https://localhost:7265/api/Supplier/1", responseContent, HttpStatusCode.NoContent);

            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }

    public static class HttpMessageHandlerExtensions
    {
        public static void SetupRequest(this Mock<HttpMessageHandler> mockHandler, HttpMethod method, string requestUri, HttpContent content, HttpStatusCode statusCode)
        {
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == method &&
                        req.RequestUri.ToString() == requestUri
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = content
                });
        }
    }
}
