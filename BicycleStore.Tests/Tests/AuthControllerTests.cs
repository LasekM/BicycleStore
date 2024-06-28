using BicycleStore.Controllers;
using BicycleStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Moq.Protected;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BicycleStore.Tests.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly AuthController _controller;
        private readonly Mock<HttpContext> _httpContextMock;
        private readonly Mock<ISession> _sessionMock;
        private readonly Mock<IAuthenticationService> _authServiceMock;
        private readonly Mock<ITempDataDictionary> _tempDataMock;
        private readonly Mock<ITempDataProvider> _tempDataProviderMock;
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<IUrlHelper> _urlHelperMock;
        private readonly Mock<IUrlHelperFactory> _urlHelperFactoryMock;

        public AuthControllerTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("https://localhost:7137/")
            };

            _controller = new AuthController(_httpClient);

            _httpContextMock = new Mock<HttpContext>();
            _sessionMock = new Mock<ISession>();
            _authServiceMock = new Mock<IAuthenticationService>();
            _tempDataMock = new Mock<ITempDataDictionary>();
            _tempDataProviderMock = new Mock<ITempDataProvider>();
            _serviceProviderMock = new Mock<IServiceProvider>();
            _urlHelperMock = new Mock<IUrlHelper>();
            _urlHelperFactoryMock = new Mock<IUrlHelperFactory>();

            _httpContextMock.Setup(s => s.Session).Returns(_sessionMock.Object);
            _httpContextMock.Setup(s => s.RequestServices.GetService(typeof(IAuthenticationService))).Returns(_authServiceMock.Object);
            _httpContextMock.Setup(s => s.RequestServices.GetService(typeof(ITempDataProvider))).Returns(_tempDataProviderMock.Object);
            _httpContextMock.Setup(s => s.RequestServices.GetService(typeof(IUrlHelperFactory))).Returns(_urlHelperFactoryMock.Object);
            _httpContextMock.Setup(s => s.RequestServices.GetService(typeof(IUrlHelper))).Returns(_urlHelperMock.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContextMock.Object
            };

            _controller.TempData = new TempDataDictionary(_httpContextMock.Object, _tempDataProviderMock.Object);
        }

        [Fact]
        public async Task Login_Post_ReturnsViewResult_WithInvalidModelState()
        {

            _controller.ModelState.AddModelError("Username", "Required");

   
            var result = await _controller.Login(new LoginModel());

          
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public async Task Login_Post_ReturnsViewResult_WithInvalidLoginAttempt()
        {
           
            var model = new LoginModel { Username = "user", Password = "password" };

            _handlerMock.SetupRequest(HttpMethod.Post, "https://localhost:7137/api/Auth/Login", new StringContent(string.Empty), HttpStatusCode.Unauthorized);

           
            var result = await _controller.Login(model);

           
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Invalid login attempt.", _controller.ViewBag.ErrorMessage);
        }

        [Fact]
        public async Task Register_Post_ReturnsViewResult_WithInvalidModelState()
        {
            
            _controller.ModelState.AddModelError("Username", "Required");

           
            var result = await _controller.Register(new RegisterModel());

            
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public async Task Register_Post_ReturnsRedirectToAction_WithValidRegistration()
        {
            
            var model = new RegisterModel { Username = "user", Password = "password" };

            _handlerMock.SetupRequest(HttpMethod.Post, "https://localhost:7137/api/Auth/Register", new StringContent(string.Empty), HttpStatusCode.OK);

            
            var result = await _controller.Register(model);

           
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Register_Post_ReturnsViewResult_WithRegistrationFailed()
        {
            
            var model = new RegisterModel { Username = "user", Password = "password" };

            _handlerMock.SetupRequest(HttpMethod.Post, "https://localhost:7137/api/Auth/Register", new StringContent(string.Empty), HttpStatusCode.BadRequest);

            
            var result = await _controller.Register(model);

            
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Registration failed.", _controller.ModelState[string.Empty].Errors[0].ErrorMessage);
        }

        [Fact]
        public async Task Logout_Post_RedirectsToIndex()
        {
           
            _authServiceMock.Setup(s => s.SignOutAsync(_httpContextMock.Object, CookieAuthenticationDefaults.AuthenticationScheme, null))
                            .Returns(Task.CompletedTask);

           
            var result = await _controller.Logout();

            
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
