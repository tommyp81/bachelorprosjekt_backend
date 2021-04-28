using API.Controllers;
using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class CustomControllerTest
    {
        [Fact]
        public async Task GetDocuments_Ok()
        {
            // Arrange
            var documentDTOs = TestDocumentListDTO();
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.GetDocuments()).ReturnsAsync(documentDTOs);
            var controller = new CustomController(mockRepo.Object, null, null);

            // Act
            var result = await controller.GetDocuments();

            // Assert
            var actionResult = Assert.IsType<ActionResult<ICollection<DocumentDTO>>>(result);
            var objectResult = result.Result as ObjectResult;
            var okResult = Assert.IsType<OkObjectResult>(objectResult);
            var returnValue = Assert.IsAssignableFrom<ICollection<DocumentDTO>>(okResult.Value);
            Assert.Equal(3, returnValue.Count);
            var document = returnValue.FirstOrDefault();
            Assert.Equal(1, document.Id);
        }

        [Fact]
        public async Task GetDocuments_IsNull()
        {
            // Arrange
            var mockRepo = new Mock<ICustomBLL>();
            var controller = new CustomController(mockRepo.Object, null, null);

            // Act
            var result = await controller.GetDocuments();

            // Assert
            var objectResult = result.Result as ObjectResult;
            var okResult = Assert.IsType<OkObjectResult>(objectResult);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async void UploadDocument_Ok()
        {
            // Arrange
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Dette er også kun en test.")), 0, 0, "Data", "Testfil1.txt");
            int userId = 1;
            int postId = 1;
            var documentDTO = TestDocumentDTO();
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.UploadDocument(file, userId, postId, null, null)).ReturnsAsync(documentDTO).Verifiable();
            var controller = new CustomController(mockRepo.Object, null, null);

            // Act
            var result = await controller.UploadDocument(file, userId, postId, null, null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<DocumentDTO>(createdAtActionResult.Value);
            mockRepo.Verify();
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void UploadDocument_BadRequest()
        {
            // Arrange
            var mockRepo = new Mock<ICustomBLL>();
            var controller = new CustomController(mockRepo.Object, null, null);

            // Act
            var result = await controller.UploadDocument(null, null, null, null, null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public async void GetDocumentInfo_Ok()
        {
            // Arrange
            int id = 1;
            var documentDTO = TestDocumentDTO();
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.GetDocumentInfo(id)).ReturnsAsync(documentDTO);
            var controller = new CustomController(mockRepo.Object, null, null);

            // Act
            var result = await controller.GetDocumentInfo(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
            var objectResult = result.Result as ObjectResult;
            var okResult = Assert.IsType<OkObjectResult>(objectResult);
            var returnValue = Assert.IsType<DocumentDTO>(okResult.Value);
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void GetDocumentInfo_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<ICustomBLL>();
            var controller = new CustomController(mockRepo.Object, null, null);

            // Act
            var result = await controller.GetDocumentInfo(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async void GetDocument_Ok()
        {
            // Arrange
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Dette er også kun en test.")), 0, 0, "Data", "Testfil1.txt");
            int id = 1;
            var documentDTO = TestDocumentDTO();
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.GetDocumentInfo(id)).ReturnsAsync(documentDTO);
            var controller = new CustomController(mockRepo.Object, null, null);

            // Act
            var result = await controller.GetDocument(id);

            // Assert
            //var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
            var objectResult = result.Result as ObjectResult;
            Assert.IsAssignableFrom<IFormFile>(objectResult.Value = file);
        }

        [Fact]
        public async void GetDocument_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<ICustomBLL>();
            var controller = new CustomController(mockRepo.Object, null, null);

            // Act
            var result = await controller.GetDocument(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async void DeleteDocument_Ok()
        {
            // Arrange
            int id = 1;
            var documentDTO = TestDocumentDTO();
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.GetDocumentInfo(id)).ReturnsAsync(documentDTO);
            mockRepo.Setup(repo => repo.DeleteDocument(id)).ReturnsAsync(documentDTO).Verifiable();
            var controller = new CustomController(mockRepo.Object, null, null);

            // Act
            var result = await controller.DeleteDocument(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
            var objectResult = result.Result as ObjectResult;
            var okResult = Assert.IsType<OkObjectResult>(objectResult);
            var returnValue = Assert.IsType<DocumentDTO>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void DeleteComment_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.DeleteDocument(id));
            var controller = new CustomController(mockRepo.Object, null, null);

            // Act
            var result = await controller.DeleteDocument(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async void Login_Ok()
        {
            // Arrange
            string username = "sysadmin";
            string password = "password";
            var userDTO = TestUserDTO();
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.Login(username, null, password)).ReturnsAsync(userDTO);
            var controller = new CustomController(mockRepo.Object, null, null);

            // Act
            var result = await controller.Login(username, null, password);

            // Assert
            var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
            var objectResult = result.Result as ObjectResult;
            var okResult = Assert.IsType<OkObjectResult>(objectResult);
            var returnValue = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(username, returnValue.Username);
        }

        [Fact]
        public async void Login_Unauthorized()
        {
            // Arrange
            string username = "feil";
            string password = "feil";
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.Login(username, null, password)).ReturnsAsync((UserDTO)null);
            var controller = new CustomController(mockRepo.Object, null, null);

            // Act
            var result = await controller.Login(username, null, password);

            // Assert
            //var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
            var objectResult = result.Result as ObjectResult;
            Assert.IsType<UnauthorizedObjectResult>(objectResult);
        }

        [Fact]
        public async Task Login_InternalServerError()
        {
            // Arrange
            var mockRepo = new Mock<ICustomBLL>();
            mockRepo.Setup(repo => repo.Login(null, null, null)).ThrowsAsync(new InvalidOperationException());
            var controller = new CustomController(mockRepo.Object, null, null);

            // Act
            var result = await controller.Login(null, null, null);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
        }

        private static ICollection<DocumentDTO> TestDocumentListDTO()
        {
            var documents = new List<DocumentDTO>
            {
                new DocumentDTO()
                {
                    Id = 1,
                    FileName = "Testfil1.txt",
                    FileType = ".txt",
                    FileSize = "29 byte",
                    Uploaded = DateTime.UtcNow,
                    UniqueName = "Testfil1.txt (test)",
                    Container = "sysadmin",
                    UserId = 1,
                    PostId = 1,
                    CommentId = null,
                    InfoTopicId = null
                },
                new DocumentDTO()
                {
                    Id = 2,
                    FileName = "Testfil2.txt",
                    FileType = ".txt",
                    FileSize = "29 byte",
                    Uploaded = DateTime.UtcNow,
                    UniqueName = "Testfil2.txt (test)",
                    Container = "sysadmin",
                    UserId = 1,
                    PostId = null,
                    CommentId = 1,
                    InfoTopicId = null
                },
                new DocumentDTO()
                {
                    Id = 3,
                    FileName = "Testfil3.txt",
                    FileType = ".txt",
                    FileSize = "29 byte",
                    Uploaded = DateTime.UtcNow,
                    UniqueName = "Testfil3.txt (test)",
                    Container = "sysadmin",
                    UserId = 1,
                    PostId = null,
                    CommentId = null,
                    InfoTopicId = 1
                },
            };
            return documents;
        }

        private static DocumentDTO TestDocumentDTO()
        {
            var document = new DocumentDTO()
            {
                Id = 1,
                FileName = "Testfil1.txt",
                FileType = ".txt",
                FileSize = "29 byte",
                Uploaded = DateTime.UtcNow,
                UniqueName = "Testfil1.txt (test)",
                Container = "sysadmin",
                UserId = 1,
                PostId = 1,
                CommentId = null,
                InfoTopicId = null
            };
            return document;
        }

        private static UserDTO TestUserDTO()
        {
            var user = new UserDTO()
            {
                Id = 1,
                Username = "sysadmin",
                FirstName = "Superbruker",
                LastName = "NFB",
                Email = "admin@badminton.no",
                Admin = true
            };
            return user;
        }
    }
}
