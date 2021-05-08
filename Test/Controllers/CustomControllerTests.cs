using Xunit;
using API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Objects;
using Moq;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Microsoft.AspNetCore.Http;
using DAL.Helpers;
using Model.Auth;
using API.Auth;

namespace API.Controllers.Tests
{
    public class CustomControllerTests
    {
        [Fact]
        public async void GetDocumentsTest_Ok()
        {
            // Arrange
            var pagedResponse = DocumentObject.TestPagedResponse(null);
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.PagedList(null, 1, 10, "Asc", "Date")).ReturnsAsync(pagedResponse);
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.GetDocuments(null, 1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<DocumentDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsAssignableFrom<PageResponse<IEnumerable<DocumentDTO>>>(okResult.Value);
            int id = 1;
            foreach (var item in returnValue.Data)
            {
                Assert.Equal(id, item.Id);
                id++;
            }
            Assert.Equal(3, returnValue.Data.Count());
        }

        [Fact]
        public async void GetDocumentsTest_NotFound()
        {
            // Arrange
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.PagedList(null, 1, 10, "Asc", "Date")).ReturnsAsync((PageResponse<IEnumerable<DocumentDTO>>)null);
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.GetDocuments(null, 1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<DocumentDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async void GetDocumentsTest_InternalServerError()
        {
            // Arrange
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.PagedList(null, 1, 10, "Asc", "Date")).ThrowsAsync(new InvalidOperationException());
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.GetDocuments(null, 1, 10, "Asc", "Date");

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved henting av dokumenter", objectResult.Value);
        }

        [Fact]
        public async void UploadDocumentTest_Ok()
        {
            // Arrange
            int userId = 1;
            int postId = 1;
            var file = DocumentObject.TestFile();
            var iFormFile = new FormFile(file.FileStream, file.FileStream.Position, file.FileStream.Length, file.ContentType, file.FileDownloadName);
            var documentDTO = DocumentObject.TestDocumentDTO();
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.UploadDocument(iFormFile, userId, postId, null, null)).ReturnsAsync(documentDTO).Verifiable();
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.UploadDocument(iFormFile, userId, postId, null, null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<DocumentDTO>(createdAtActionResult.Value);
            mockRepo.Verify();
            Assert.Equal(documentDTO.FileName, returnValue.FileName);
        }

        [Fact]
        public async void UploadDocumentTest_BadRequest()
        {
            // Arrange
            var mockRepo = new Mock<ICustomBLL>();
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.UploadDocument(null, null, null, null, null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Det må legges ved en fil", badRequestResult.Value);
        }

        [Fact]
        public async void UploadDocumentTest_InternalServerError()
        {
            var file = DocumentObject.TestFile();
            var iFormFile = new FormFile(file.FileStream, file.FileStream.Position, file.FileStream.Length, file.ContentType, file.FileDownloadName);
            int userId = 1;
            int postId = 1;
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.UploadDocument(iFormFile, userId, postId, null, null)).ThrowsAsync(new InvalidOperationException());
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.UploadDocument(iFormFile, userId, postId, null, null);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved opplasting av nytt dokument", objectResult.Value);
        }

        [Fact]
        public async void GetDocumentInfoTest_Ok()
        {
            // Arrange
            int id = 1;
            var documentDTO = DocumentObject.TestDocumentDTO();
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.GetDocumentInfo(id)).ReturnsAsync(documentDTO);
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.GetDocumentInfo(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<DocumentDTO>(okResult.Value);
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void GetDocumentInfoTest_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.GetDocumentInfo(id)).ReturnsAsync((DocumentDTO)null);
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.GetDocumentInfo(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Dokument med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void GetDocumentInfoTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.GetDocumentInfo(id)).ThrowsAsync(new InvalidOperationException());
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.GetDocumentInfo(id);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved henting av dokument", objectResult.Value);
        }

        [Fact]
        public async void GetDocumentTest_Ok()
        {
            // Arrange
            int id = 1;
            var file = DocumentObject.TestFile();
            var documentDTO = DocumentObject.TestDocumentDTO();
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.GetDocument(id)).ReturnsAsync(file);
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.GetDocument(id);

            // Assert
            var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
            var fileResult = Assert.IsType<FileStreamResult>(actionResult);
            Assert.Equal("Testfil1.txt", fileResult.FileDownloadName);
        }

        [Fact]
        public async void GetDocumentTest_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.GetDocument(id)).ReturnsAsync((FileStreamResult)null);
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.GetDocument(id);

            // Assert
            var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult);
            Assert.Equal("Dokumentet med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void GetDocumentTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.GetDocument(id)).ThrowsAsync(new InvalidOperationException());
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.GetDocument(id);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved henting av dokument", objectResult.Value);
        }

        [Fact]
        public async void DeleteDocumentTest_Ok()
        {
            // Arrange
            int id = 1;
            var documentDTO = DocumentObject.TestDocumentDTO();
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.DeleteDocument(id)).ReturnsAsync(documentDTO).Verifiable();
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.DeleteDocument(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<DocumentDTO>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void DeleteDocumentTest_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.DeleteDocument(id));
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.DeleteDocument(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Dokument med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void DeleteDocumentTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.DeleteDocument(id)).ThrowsAsync(new InvalidOperationException());
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.DeleteDocument(id);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved sletting av dokument", objectResult.Value);
        }

        [Fact]
        public async void LoginTest_Ok()
        {
            // Arrange
            var authRequest = new AuthRequest()
            {
                Username = "sysadmin",
                Password = "password"
            };
            var authResponse = UserObject.TestAuthResponse();
            var mockRepo = new Mock<ICustomBLL>();
            var mockTokenRepo = new Mock<ITokenService>();
            mockRepo.Setup(repo => repo.Login(authRequest)).ReturnsAsync(authResponse);
            var controller = new CustomController(mockRepo.Object, mockTokenRepo.Object);

            // Act
            var result = await controller.Login(authRequest);

            // Assert
            var actionResult = Assert.IsType<ActionResult<AuthResponse>>(result);
            var objectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<AuthResponse>(objectResult.Value);
            Assert.Equal(authRequest.Username, returnValue.Username);
        }

        [Fact]
        public async void LoginTest_Unauthorized()
        {
            var mockRepo = new Mock<ICustomBLL>();
            var mockTokenRepo = new Mock<ITokenService>();
            mockRepo.Setup(repo => repo.Login(null)).ReturnsAsync((AuthResponse)null);
            var controller = new CustomController(mockRepo.Object, mockTokenRepo.Object);

            // Act
            var result = await controller.Login(null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<AuthResponse>>(result);
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(actionResult.Result);
            Assert.Equal("Feil ved brukernavn, epost eller passord", unauthorizedResult.Value);
        }

        [Fact]
        public async void LoginTest_InternalServerError()
        {
            // Arrange
            var mockRepo = new Mock<ICustomBLL>();
            var mockTokenRepo = new Mock<ITokenService>();
            mockRepo.Setup(repo => repo.Login(null)).ThrowsAsync(new InvalidOperationException());
            var controller = new CustomController(mockRepo.Object, mockTokenRepo.Object);

            // Act
            var result = await controller.Login(null);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved login", objectResult.Value);
        }

        [Fact]
        public async void SetAdminTest_Ok()
        {
            // Arrange
            int id = 1;
            bool admin = true;
            var userDTO = UserObject.TestUserDTO();
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.SetAdmin(id, admin)).ReturnsAsync(userDTO);
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.SetAdmin(id, admin);

            // Assert
            var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void SetAdminTest_NotFound()
        {
            // Arrange
            int id = 1;
            bool admin = true;
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.SetAdmin(id, admin)).ReturnsAsync((UserDTO)null);
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.SetAdmin(id, admin);

            // Assert
            var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Bruker med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void SetAdminTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            bool admin = true;
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.SetAdmin(id, admin)).ThrowsAsync(new InvalidOperationException());
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.SetAdmin(id, admin);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved endring av admin", objectResult.Value);
        }

        [Fact]
        public async void SearchTest_Ok()
        {
            // Arrange
            string query = "Testfil1.txt";
            var pagedResponse = DocumentObject.TestPagedResponse(query);
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.Search(query, null, 1, 10, "Asc", "Date")).ReturnsAsync(pagedResponse);
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.SearchDocuments(query, null, 1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<DocumentDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsAssignableFrom<PageResponse<IEnumerable<DocumentDTO>>>(okResult.Value);
            foreach (var item in returnValue.Data)
            {
                Assert.Equal(query, item.FileName);
            }
        }

        [Fact]
        public async void SearchTest_OkNull()
        {
            // Arrange
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.Search(null, null, 1, 10, "Asc", "Date")).ReturnsAsync((PageResponse<IEnumerable<DocumentDTO>>)null);
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.SearchDocuments(null, null, 1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<DocumentDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async void SearchTest_InternalServerError()
        {
            // Arrange
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.Search(null, null, 1, 10, "Asc", "Date")).ThrowsAsync(new InvalidOperationException());
            var controller = new CustomController(mockRepo.Object, null);

            // Act
            var result = await controller.SearchDocuments(null, null, 1, 10, "Asc", "Date");

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved søk i kommentarer", objectResult.Value);
        }
    }
}