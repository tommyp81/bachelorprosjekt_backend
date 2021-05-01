//using Xunit;
//using API.Controllers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Test.Objects;
//using Moq;
//using BLL.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using Model.DTO;
//using Microsoft.AspNetCore.Http;

//namespace API.Controllers.Tests
//{
//    public class CustomControllerTests
//    {
//        //[Fact]
//        //public async void CustomControllerTest()
//        //{
//        //    Assert.True(false, "This test needs an implementation");
//        //}

//        [Fact]
//        public async void GetDocumentsTest_Ok()
//        {
//            // Arrange
//            var documentDTOs = DocumentObject.TestDocumentListDTO();
//            var mockRepo = new Mock<ICustomBLL>();
//            mockRepo.Setup(repo => repo.GetDocuments()).ReturnsAsync(documentDTOs);
//            var controller = new CustomController(mockRepo.Object, null, null);

//            // Act
//            var result = await controller.GetDocuments();

//            // Assert
//            var actionResult = Assert.IsType<ActionResult<ICollection<DocumentDTO>>>(result);
//            var objectResult = result.Result as ObjectResult;
//            var okResult = Assert.IsType<OkObjectResult>(objectResult);
//            var returnValue = Assert.IsAssignableFrom<ICollection<DocumentDTO>>(okResult.Value);
//            Assert.Equal(3, returnValue.Count);
//            var document = returnValue.FirstOrDefault();
//            Assert.Equal(1, document.Id);
//        }

//        [Fact]
//        public async void GetDocumentsTest_IsNull()
//        {
//            // Arrange
//            var mockRepo = new Mock<ICustomBLL>();
//            mockRepo.Setup(repo => repo.GetDocuments()).ReturnsAsync((ICollection<DocumentDTO>)null);
//            var controller = new CustomController(mockRepo.Object, null, null);

//            // Act
//            var result = await controller.GetDocuments();

//            // Assert
//            var objectResult = result.Result as ObjectResult;
//            var okResult = Assert.IsType<OkObjectResult>(objectResult);
//            Assert.Null(okResult.Value);
//        }

//        [Fact]
//        public async void GetDocumentsTest_InternalServerError()
//        {
//            // Arrange
//            var mockRepo = new Mock<ICustomBLL>();
//            mockRepo.Setup(repo => repo.GetDocuments()).ThrowsAsync(new InvalidOperationException());
//            var controller = new CustomController(mockRepo.Object, null, null);

//            // Act
//            var result = await controller.GetDocuments();

//            // Assert
//            var objectResult = result.Result as ObjectResult;
//            Assert.Equal(500, objectResult.StatusCode);
//        }

//        [Fact]
//        public async void UploadDocumentTest_Ok()
//        {
//            // Arrange
//            var file = DocumentObject.TestDocument();
//            var iFormFile = new FormFile(file.FileStream, file.FileStream.Position, file.FileStream.Length, file.ContentType, file.FileDownloadName);
//            int userId = 1;
//            int postId = 1;
//            var documentDTO = DocumentObject.TestDocumentDTO();
//            var mockRepo = new Mock<ICustomBLL>();
//            mockRepo.Setup(repo => repo.UploadDocument(iFormFile, userId, postId, null, null)).ReturnsAsync(documentDTO).Verifiable();
//            var controller = new CustomController(mockRepo.Object, null, null);

//            // Act
//            var result = await controller.UploadDocument(iFormFile, userId, postId, null, null);

//            // Assert
//            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
//            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
//            var returnValue = Assert.IsType<DocumentDTO>(createdAtActionResult.Value);
//            mockRepo.Verify();
//            Assert.Equal(documentDTO.FileName, returnValue.FileName);
//        }

//        [Fact]
//        public async void UploadDocumentTest_BadRequest()
//        {
//            // Arrange
//            var mockRepo = new Mock<ICustomBLL>();
//            var controller = new CustomController(mockRepo.Object, null, null);

//            // Act
//            var result = await controller.UploadDocument(null, null, null, null, null);

//            // Assert
//            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
//            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
//        }

//        [Fact]
//        public async void UploadDocumentTest_InternalServerError()
//        {
//            var file = DocumentObject.TestDocument();
//            var iFormFile = new FormFile(file.FileStream, file.FileStream.Position, file.FileStream.Length, file.ContentType, file.FileDownloadName);
//            int userId = 1;
//            int postId = 1;
//            var mockRepo = new Mock<ICustomBLL>();
//            mockRepo.Setup(repo => repo.UploadDocument(iFormFile, userId, postId, null, null)).ThrowsAsync(new InvalidOperationException());
//            var controller = new CustomController(mockRepo.Object, null, null);

//            // Act
//            var result = await controller.UploadDocument(iFormFile, userId, postId, null, null);

//            // Assert
//            var objectResult = result.Result as ObjectResult;
//            Assert.Equal(500, objectResult.StatusCode);
//        }

//        [Fact]
//        public async void GetDocumentInfoTest_Ok()
//        {
//            // Arrange
//            int id = 1;
//            var documentDTO = DocumentObject.TestDocumentDTO();
//            var mockRepo = new Mock<ICustomBLL>();
//            mockRepo.Setup(repo => repo.GetDocumentInfo(id)).ReturnsAsync(documentDTO);
//            var controller = new CustomController(mockRepo.Object, null, null);

//            // Act
//            var result = await controller.GetDocumentInfo(id);

//            // Assert
//            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
//            var objectResult = result.Result as ObjectResult;
//            var okResult = Assert.IsType<OkObjectResult>(objectResult);
//            var returnValue = Assert.IsType<DocumentDTO>(okResult.Value);
//            Assert.Equal(id, returnValue.Id);
//        }

//        [Fact]
//        public async void GetDocumentInfoTest_NotFound()
//        {
//            // Arrange
//            int id = 1;
//            var mockRepo = new Mock<ICustomBLL>();
//            mockRepo.Setup(repo => repo.GetDocumentInfo(id)).ReturnsAsync((DocumentDTO)null);
//            var controller = new CustomController(mockRepo.Object, null, null);

//            // Act
//            var result = await controller.GetDocumentInfo(id);

//            // Assert
//            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
//            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
//        }

//        [Fact]
//        public async void GetDocumentInfoTest_InternalServerError()
//        {
//            // Arrange
//            int id = 1;
//            var mockRepo = new Mock<ICustomBLL>();
//            mockRepo.Setup(repo => repo.GetDocumentInfo(id)).ThrowsAsync(new InvalidOperationException());
//            var controller = new CustomController(mockRepo.Object, null, null);

//            // Act
//            var result = await controller.GetDocumentInfo(id);

//            // Assert
//            var objectResult = result.Result as ObjectResult;
//            Assert.Equal(500, objectResult.StatusCode);
//        }

//        [Fact]
//        public async void GetDocumentTest_Ok()
//        {
//            // Arrange
//            int id = 1;
//            var documentDTO = DocumentObject.TestDocumentDTO();
//            var file = DocumentObject.TestDocument();
//            var mockRepo = new Mock<ICustomBLL>();
//            mockRepo.Setup(repo => repo.GetDocumentInfo(id)).ReturnsAsync(documentDTO);
//            mockRepo.Setup(repo => repo.GetDocument(id)).ReturnsAsync(file);
//            var controller = new CustomController(mockRepo.Object, null, null);

//            // Act
//            var result = await controller.GetDocument(id);

//            // Assert
//            //var actionResult = Assert.IsType<FileStreamResult>(result);
//            var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
//            Assert.NotNull(result);
//            //var objectResult = actionResult as OkObjectResult;
//            //var objectResult = result as ObjectResult;
//            //Assert.Equal(500, objectResult.StatusCode);
//            //var okResult = Assert.IsType<OkObjectResult>(actionResult.);
//            //var returnValue = Assert.IsType<DocumentDTO>(okResult.Value);
//            //Assert.Equal(documentDTO.FileName, returnValue.FileName);

//            Assert.True(false, "This test needs an implementation");
//        }

//        [Fact]
//        public async void GetDocumentTest_NotFoundDatabase()
//        {
//            // Arrange
//            int id = 1;
//            var mockRepo = new Mock<ICustomBLL>();
//            mockRepo.Setup(repo => repo.GetDocumentInfo(id)).ReturnsAsync((DocumentDTO)null);
//            mockRepo.Setup(repo => repo.GetDocument(id)).ReturnsAsync((FileStreamResult)null);
//            var controller = new CustomController(mockRepo.Object, null, null);

//            // Act
//            var result = await controller.GetDocument(id);

//            // Assert
//            var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
//            var objectResult = Assert.IsType<NotFoundObjectResult>(actionResult);
//            Assert.Equal("Dokumentet med ID 1 finnes ikke i databasen", objectResult.Value);
//        }

//        [Fact]
//        public async void GetDocumentTest_NotFoundAzure()
//        {
//            // Arrange
//            int id = 1;
//            var documentDTO = DocumentObject.TestDocumentDTO();
//            var mockRepo = new Mock<ICustomBLL>();
//            mockRepo.Setup(repo => repo.GetDocumentInfo(id)).ReturnsAsync(documentDTO);
//            mockRepo.Setup(repo => repo.GetDocument(id)).ReturnsAsync((FileStreamResult)null);
//            var controller = new CustomController(mockRepo.Object, null, null);

//            // Act
//            var result = await controller.GetDocument(id);

//            // Assert
//            var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
//            var objectResult = Assert.IsType<NotFoundObjectResult>(actionResult);
//            Assert.Equal("Dokumentet med ID 1 finnes ikke i Azure Storage", objectResult.Value);
//        }

//        //[Fact]
//        //public async void GetDocumentTest_InternalServerError()
//        //{
//        //    Assert.True(false, "This test needs an implementation");
//        //}

//        [Fact]
//        public async void DeleteDocumentTest_Ok()
//        {
//            // Arrange
//            int id = 1;
//            var documentDTO = DocumentObject.TestDocumentDTO();
//            var mockRepo = new Mock<ICustomBLL>();
//            mockRepo.Setup(repo => repo.GetDocumentInfo(id)).ReturnsAsync(documentDTO);
//            mockRepo.Setup(repo => repo.DeleteDocument(id)).ReturnsAsync(documentDTO).Verifiable();
//            var controller = new CustomController(mockRepo.Object, null, null);

//            // Act
//            var result = await controller.DeleteDocument(id);

//            // Assert
//            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
//            var objectResult = result.Result as ObjectResult;
//            var okResult = Assert.IsType<OkObjectResult>(objectResult);
//            var returnValue = Assert.IsType<DocumentDTO>(okResult.Value);
//            mockRepo.Verify();
//            Assert.Equal(id, returnValue.Id);
//        }

//        [Fact]
//        public async void DeleteDocumentTest_NotFound()
//        {
//            // Arrange
//            int id = 1;
//            var mockRepo = new Mock<ICustomBLL>();
//            mockRepo.Setup(repo => repo.DeleteDocument(id));
//            var controller = new CustomController(mockRepo.Object, null, null);

//            // Act
//            var result = await controller.DeleteDocument(id);

//            // Assert
//            var actionResult = Assert.IsType<ActionResult<DocumentDTO>>(result);
//            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
//        }

//        [Fact]
//        public async void DeleteDocumentTest_InternalServerError()
//        {
//            // Arrange
//            int id = 1;
//            var documentDTO = DocumentObject.TestDocumentDTO();
//            var mockRepo = new Mock<ICustomBLL>();
//            mockRepo.Setup(repo => repo.GetDocumentInfo(id)).ReturnsAsync(documentDTO);
//            mockRepo.Setup(repo => repo.DeleteDocument(id)).ThrowsAsync(new InvalidOperationException());
//            var controller = new CustomController(mockRepo.Object, null, null);

//            // Act
//            var result = await controller.DeleteDocument(id);

//            // Assert
//            var objectResult = result.Result as ObjectResult;
//            Assert.Equal(500, objectResult.StatusCode);
//        }

//        [Fact]
//        public async void LoginTest_Ok()
//        {
//            // Arrange
//            string username = "sysadmin";
//            string password = "password";
//            var userDTO = UserObject.TestUserDTO();
//            var mockRepo = new Mock<ICustomBLL>();
//            mockRepo.Setup(repo => repo.Login(username, null, password)).ReturnsAsync(userDTO);
//            var controller = new CustomController(mockRepo.Object, null, null);

//            // Act
//            var result = await controller.Login(username, null, password);

//            // Assert
//            var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
//            var objectResult = result.Result as ObjectResult;
//            var okResult = Assert.IsType<OkObjectResult>(objectResult);
//            var returnValue = Assert.IsType<UserDTO>(okResult.Value);
//            Assert.Equal(username, returnValue.Username);
//        }

//        [Fact]
//        public async void LoginTest_Unauthorized()
//        {
//            // Arrange
//            string username = "feil";
//            string password = "feil";
//            var mockRepo = new Mock<ICustomBLL>();
//            mockRepo.Setup(repo => repo.Login(username, null, password)).ReturnsAsync((UserDTO)null);
//            var controller = new CustomController(mockRepo.Object, null, null);

//            // Act
//            var result = await controller.Login(username, null, password);

//            // Assert
//            var objectResult = result.Result as ObjectResult;
//            Assert.IsType<UnauthorizedObjectResult>(objectResult);
//        }

//        [Fact]
//        public async void LoginTest_InternalServerError()
//        {
//            // Arrange
//            var mockRepo = new Mock<ICustomBLL>();
//            mockRepo.Setup(repo => repo.Login(null, null, null)).ThrowsAsync(new InvalidOperationException());
//            var controller = new CustomController(mockRepo.Object, null, null);

//            // Act
//            var result = await controller.Login(null, null, null);

//            // Assert
//            var objectResult = result.Result as ObjectResult;
//            Assert.Equal(500, objectResult.StatusCode);
//        }

//        //[Fact]
//        //public async void SetAdminTest()
//        //{
//        //    Assert.True(false, "This test needs an implementation");
//        //}

//        //// Tester for metoder i BLL

//        //[Fact]
//        //public async void GetCommentCountTest()
//        //{
//        //    Assert.True(false, "This test needs an implementation");
//        //}

//        //[Fact]
//        //public async void GetLikeCountTest()
//        //{
//        //    Assert.True(false, "This test needs an implementation");
//        //}

//        //[Fact]
//        //public async void AddDTOTest()
//        //{
//        //    Assert.True(false, "This test needs an implementation");
//        //}
//    }
//}