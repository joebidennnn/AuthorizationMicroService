using AuthorizationService.Controllers;
using AuthorizationService.Models;
using AuthorizationService.Repositories;
using AuthorizationService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;

namespace AuthorizationServiceNUnitTest
{
    public class Tests
    {
        private AuthenticationController _controller;
        private AuthenticationService _service;
        private Mock<IConfiguration> _config;
        private Mock<IAuthenticationService> _mockService;
        private IUserRepository _userRepo;
        private AuthenticationController _controller2;
        User ValidUser;
        User InvalidUser;
        [SetUp]
        public void Setup()
        {
            _config = new Mock<IConfiguration>();
            _mockService = new Mock<IAuthenticationService>();
            _userRepo = new UserRepository();
            _config.Setup(c => c["Jwt:Key"]).Returns("mysuperdupersecret");
            _service = new AuthenticationService(_userRepo, _config.Object);
            _controller = new AuthenticationController(_service);
            ValidUser = new User() { UserName = "joe", Password = "biden" };
            InvalidUser = new User() { UserName = "user", Password = "pass" };
            _mockService.Setup(c => c.Authenticate(InvalidUser)).Throws(new Exception());
            _controller2 = new AuthenticationController(_mockService.Object);
        }
        //Controller test
        [Test]
        public void AuthenticateUser_ValidData_ReturnsOkResultWithToken()
        {
            var response = _controller.AuthenticateUser(ValidUser) as OkObjectResult;
            Assert.AreEqual(200, response.StatusCode);
        }
        [Test]
        public void AuthenticateUser_ValidData_ReturnsUserNotFound()
        {
            var response = _controller.AuthenticateUser(InvalidUser) as NotFoundObjectResult;
            Assert.AreEqual(404, response.StatusCode);
        }
        [Test]
        public void AuthenticateUser_InvalidData_ReturnsErrorResponse()
        {
            var response = _controller2.AuthenticateUser(InvalidUser) as StatusCodeResult;
            Assert.AreEqual(500, response.StatusCode);
        }
        //Services test
        [Test]
        public void AuthenticationService_ValidData_ReturnsToken()
        {
            var response = _service.Authenticate(ValidUser);
            Assert.IsNotNull(response);
        }
        [Test]
        public void AuthenticationService_InValidData_ReturnsNull()
        {
            var response = _service.Authenticate(InvalidUser);
            Assert.IsNull(response);
        }
        [Test]
        public void AuthenticationService_InValidData_OnErrorThrowsException()
        {
            Assert.Throws<Exception>(() => _mockService.Object.Authenticate(InvalidUser));
        }
        //Repository test
        [Test]
        public void UserRepository_ValidData_ReturnsUserFound()
        {
            var response = _userRepo.GetUser(ValidUser);
            Assert.IsNotNull(response);
        }
        [Test]
        public void UserRepository_ValidData_ReturnsNull()
        {
            var response = _userRepo.GetUser(InvalidUser);
            Assert.IsNull(response);
        }
    }
}