using Xunit;
using API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Moq;
using DAL.Helpers;
using Model.DTO;
using Test.Objects;

namespace API.Controllers.Tests
{
    public class UsersControllerTests
    {
        [Fact]
        public async void GetUsersTest_Ok()
        {
            // Arrange
            var pagedResponse = UserObject.TestPagedResponse(null);
            var mockRepo = new Mock<IUserBLL>();
            mockRepo.Setup(repo => repo.PagedList(1, 10, "Asc", "Date")).ReturnsAsync(pagedResponse);
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.GetUsers(1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<UserDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsAssignableFrom<PageResponse<IEnumerable<UserDTO>>>(okResult.Value);
            int id = 1;
            foreach (var item in returnValue.Data)
            {
                Assert.Equal(id, item.Id);
                id++;
            }
            Assert.Equal(3, returnValue.Data.Count());
        }

        [Fact]
        public async void GetUsersTest_OkNull()
        {
            // Arrange
            var mockRepo = new Mock<IUserBLL>();
            mockRepo.Setup(repo => repo.PagedList(1, 10, "Asc", "Date")).ReturnsAsync((PageResponse<IEnumerable<UserDTO>>)null);
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.GetUsers(1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<UserDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async void GetUsersTest_InternalServerError()
        {
            // Arrange
            var mockRepo = new Mock<IUserBLL>();
            mockRepo.Setup(repo => repo.PagedList(1, 10, "Asc", "Date")).ThrowsAsync(new InvalidOperationException());
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.GetUsers(1, 10, "Asc", "Date");

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved henting av brukere", objectResult.Value);
        }

        [Fact]
        public async void GetUserTest_Ok()
        {
            // Arrange
            int id = 1;
            var userDTO = UserObject.TestUserDTO();
            var mockRepo = new Mock<IUserBLL>();
            mockRepo.Setup(repo => repo.GetUser(id)).ReturnsAsync(userDTO);
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.GetUser(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void GetUserTest_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IUserBLL>();
            mockRepo.Setup(repo => repo.GetUser(id)).ReturnsAsync((UserDTO)null);
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.GetUser(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Bruker med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void GetUserTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IUserBLL>();
            mockRepo.Setup(repo => repo.GetUser(id)).ThrowsAsync(new InvalidOperationException());
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.GetUser(id);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved henting av bruker", objectResult.Value);
        }

        [Fact]
        public async void AddUserTest_Ok()
        {
            // Arrange
            var user = UserObject.TestNewUser();
            var userDTO = UserObject.TestUserDTO();
            var mockRepo = new Mock<IUserBLL>();
            mockRepo.Setup(repo => repo.AddUser(user)).ReturnsAsync(userDTO).Verifiable();
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.AddUser(user);

            // Assert
            var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<UserDTO>(createdAtActionResult.Value);
            mockRepo.Verify();
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void AddUserTest_BadRequest()
        {
            // Arrange
            var mockRepo = new Mock<IUserBLL>();
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.AddUser(null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Brukernavn eller epost eksisterer allerede", badRequestResult.Value);
        }

        [Fact]
        public async void AddUserTest_InternalServerError()
        {
            // Arrange
            var user = UserObject.TestNewUser();
            var mockRepo = new Mock<IUserBLL>();
            mockRepo.Setup(repo => repo.AddUser(user)).ThrowsAsync(new InvalidOperationException());
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.AddUser(user);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved oppretting av bruker", objectResult.Value);
        }

        [Fact]
        public async void UpdateUserTest_Ok()
        {
            // Arrange
            int id = 1;
            var user = UserObject.TestNewUser();
            var userDTO = UserObject.TestUserDTO();
            var mockRepo = new Mock<IUserBLL>();
            mockRepo.Setup(repo => repo.UpdateUser(user)).ReturnsAsync(userDTO).Verifiable();
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.UpdateUser(id, user);

            // Assert
            var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<UserDTO>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void UpdateUserTest_BadRequest_Username()
        {
            // Arrange
            int id = 1;
            var user = UserObject.TestNewUser();
            var mockRepo = new Mock<IUserBLL>();
            mockRepo.Setup(repo => repo.UpdateUser(user)).ReturnsAsync((UserDTO)null);
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.UpdateUser(id, user);

            // Assert
            var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Brukernavn eller epost eksisterer allerede", badRequestResult.Value);
        }

        [Fact]
        public async void UpdateUserTest_BadRequest()
        {
            // Arrange
            int id = 0;
            var user = UserObject.TestNewUser();
            var userDTO = UserObject.TestUserDTO();
            var mockRepo = new Mock<IUserBLL>();
            mockRepo.Setup(repo => repo.UpdateUser(user)).ReturnsAsync(userDTO);
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.UpdateUser(id, user);

            // Assert
            var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Bruker ID stemmer ikke", badRequestResult.Value);
        }

        [Fact]
        public async void UpdateUserTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var user = UserObject.TestNewUser();
            var mockRepo = new Mock<IUserBLL>();
            mockRepo.Setup(repo => repo.UpdateUser(user)).ThrowsAsync(new InvalidOperationException());
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.UpdateUser(id, user);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved oppdatering av bruker", objectResult.Value);
        }

        [Fact]
        public async void DeleteUserTest_Ok()
        {
            // Arrange
            int id = 1;
            var userDTO = UserObject.TestUserDTO();
            var mockRepo = new Mock<IUserBLL>();
            mockRepo.Setup(repo => repo.DeleteUser(id)).ReturnsAsync(userDTO).Verifiable();
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.DeleteUser(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<UserDTO>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void DeleteUserTest_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IUserBLL>();
            mockRepo.Setup(repo => repo.DeleteUser(id)).ReturnsAsync((UserDTO)null);
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.DeleteUser(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Bruker med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void DeleteUserTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var userDTO = UserObject.TestUserDTO();
            var mockRepo = new Mock<IUserBLL>();
            mockRepo.Setup(repo => repo.DeleteUser(id)).ThrowsAsync(new InvalidOperationException());
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.DeleteUser(id);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved sletting av bruker", objectResult.Value);
        }

        [Fact]
        public async void SearchTest_Ok()
        {
            // Arrange
            string query = "sysadmin";
            var pagedResponse = UserObject.TestPagedResponse(query);
            var mockRepo = new Mock<IUserBLL>();
            mockRepo.Setup(repo => repo.Search(query, 1, 10, "Asc", "Date")).ReturnsAsync(pagedResponse);
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.Search(query, 1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<UserDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsAssignableFrom<PageResponse<IEnumerable<UserDTO>>>(okResult.Value);
            foreach (var item in returnValue.Data)
            {
                Assert.Equal(query, item.Username);
            }
        }

        [Fact]
        public async void SearchTest_OkNull()
        {
            // Arrange
            var mockRepo = new Mock<IUserBLL>();
            mockRepo.Setup(repo => repo.Search(null, 1, 10, "Asc", "Date")).ReturnsAsync((PageResponse<IEnumerable<UserDTO>>)null);
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.Search(null, 1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<UserDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async void SearchTest_InternalServerError()
        {
            // Arrange
            var mockRepo = new Mock<IUserBLL>();
            mockRepo.Setup(repo => repo.Search(null, 1, 10, "Asc", "Date")).ThrowsAsync(new InvalidOperationException());
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.Search(null, 1, 10, "Asc", "Date");

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved søk i brukere", objectResult.Value);
        }
    }
}