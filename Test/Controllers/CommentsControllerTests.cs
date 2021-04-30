using Xunit;
using API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Objects;
using BLL.Interfaces;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;

namespace API.Controllers.Tests
{
    public class CommentsControllerTests
    {
        //[Fact]
        //public async void CommentsControllerTest()
        //{
        //    Assert.True(false, "This test needs an implementation");
        //}

        [Fact]
        public async void GetCommentsTest_Ok()
        {
            // Arrange
            var commentDTOs = CommentObject.TestCommentListDTO();
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.GetComments()).ReturnsAsync(commentDTOs);
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.GetComments();

            // Assert
            var actionResult = Assert.IsType<ActionResult<ICollection<CommentDTO>>>(result);
            var objectResult = result.Result as ObjectResult;
            var okResult = Assert.IsType<OkObjectResult>(objectResult);
            var returnValue = Assert.IsAssignableFrom<ICollection<CommentDTO>>(okResult.Value);
            Assert.Equal(3, returnValue.Count);
            var comment = returnValue.FirstOrDefault();
            Assert.Equal(1, comment.Id);
        }

        [Fact]
        public async void GetCommentsTest_IsNull()
        {
            // Arrange
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.GetComments()).ReturnsAsync((ICollection<CommentDTO>)null);
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.GetComments();

            // Assert
            var actionResult = Assert.IsType<ActionResult<ICollection<CommentDTO>>>(result);
            var objectResult = result.Result as ObjectResult;
            var okResult = Assert.IsType<OkObjectResult>(objectResult);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async void GetCommentsTest_InternalServerError()
        {
            // Arrange
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.GetComments()).ThrowsAsync(new InvalidOperationException());
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.GetComments();

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved henting av kommentarer", objectResult.Value);
        }

        [Fact]
        public async void GetCommentTest_Ok()
        {
            // Arrange
            int id = 1;
            var commentDTO = CommentObject.TestCommentDTO();
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.GetComment(id)).ReturnsAsync(commentDTO);
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.GetComment(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CommentDTO>>(result);
            var objectResult = result.Result as ObjectResult;
            var okResult = Assert.IsType<OkObjectResult>(objectResult);
            var returnValue = Assert.IsType<CommentDTO>(okResult.Value);
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void GetCommentTest_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.GetComment(id)).ReturnsAsync((CommentDTO)null);
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.GetComment(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CommentDTO>>(result);
            var objectResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Kommentar med ID 1 ble ikke funnet", objectResult.Value);
        }

        [Fact]
        public async void GetCommentTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.GetComment(id)).ThrowsAsync(new InvalidOperationException());
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.GetComment(id);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved henting av kommentarer", objectResult.Value);
        }

        [Fact]
        public async void AddCommentTest_Ok()
        {
            // Arrange
            var comment = CommentObject.TestComment();
            var commentDTO = CommentObject.TestCommentDTO();
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.AddComment(null, comment)).ReturnsAsync(commentDTO).Verifiable();
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.AddComment(null, comment);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CommentDTO>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<CommentDTO>(createdAtActionResult.Value);
            mockRepo.Verify();
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void AddCommentTest_BadRequest()
        {
            // Arrange
            var mockRepo = new Mock<ICommentBLL>();
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.AddComment(null, null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CommentDTO>>(result);
            Assert.IsType<BadRequestResult>(actionResult.Result);
        }

        [Fact]
        public async void AddCommentTest_InternalServerError()
        {
            // Arrange
            var comment = CommentObject.TestComment();
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.AddComment(null, comment)).ThrowsAsync(new InvalidOperationException());
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.AddComment(null, comment);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved oppretting av ny kommentar", objectResult.Value);
        }

        [Fact]
        public async void UpdateCommentTest_Ok()
        {
            // Arrange
            int id = 1;
            var comment = CommentObject.TestComment();
            var commentDTO = CommentObject.TestCommentDTO();
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.GetComment(id)).ReturnsAsync(commentDTO);
            mockRepo.Setup(repo => repo.UpdateComment(comment)).ReturnsAsync(commentDTO).Verifiable();
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateComment(id, comment);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CommentDTO>>(result);
            var objectResult = result.Result as ObjectResult;
            var okResult = Assert.IsType<OkObjectResult>(objectResult);
            var returnValue = Assert.IsType<CommentDTO>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void UpdateCommentTest_NotFound()
        {
            // Arrange
            int id = 1;
            var comment = CommentObject.TestComment();
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.UpdateComment(comment)).ReturnsAsync((CommentDTO)null);
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateComment(id, comment);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CommentDTO>>(result);
            var objectResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Kommentar med ID 1 ble ikke funnet", objectResult.Value);
        }

        [Fact]
        public async void UpdateCommentTest_BadRequest()
        {
            // Arrange
            int id = 0;
            var comment = CommentObject.TestComment();
            var commentDTO = CommentObject.TestCommentDTO();
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.UpdateComment(comment)).ReturnsAsync(commentDTO);
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateComment(id, comment);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CommentDTO>>(result);
            var objectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Kommentar ID stemmer ikke", objectResult.Value);
        }

        [Fact]
        public async void UpdateCommentTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var comment = CommentObject.TestComment();
            var commentDTO = CommentObject.TestCommentDTO();
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.GetComment(id)).ReturnsAsync(commentDTO);
            mockRepo.Setup(repo => repo.UpdateComment(comment)).ThrowsAsync(new InvalidOperationException());
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateComment(id, comment);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved oppdatering av kommentar", objectResult.Value);
        }

        [Fact]
        public async void DeleteCommentTest_Ok()
        {
            // Arrange
            int id = 1;
            var commentDTO = CommentObject.TestCommentDTO();
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.GetComment(id)).ReturnsAsync(commentDTO);
            mockRepo.Setup(repo => repo.DeleteComment(id)).ReturnsAsync(commentDTO).Verifiable();
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.DeleteComment(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CommentDTO>>(result);
            var objectResult = result.Result as ObjectResult;
            var okResult = Assert.IsType<OkObjectResult>(objectResult);
            var returnValue = Assert.IsType<CommentDTO>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void DeleteCommentTest_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.DeleteComment(id)).ReturnsAsync((CommentDTO)null);
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.DeleteComment(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CommentDTO>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async void DeleteCommentTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var commentDTO = CommentObject.TestCommentDTO();
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.GetComment(id)).ReturnsAsync(commentDTO);
            mockRepo.Setup(repo => repo.DeleteComment(id)).ThrowsAsync(new InvalidOperationException());
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.DeleteComment(id);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
        }

        // Tester for metoder i BLL

        [Fact]
        public async void AddDTO_Ok()
        {
            // Arrange
            int id = 1;
            var comment = CommentObject.TestComment();
            var commentDTO = CommentObject.TestCommentDTO();
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.AddDTO(comment)).ReturnsAsync(commentDTO);

            // Act
            var result = await mockRepo.Object.AddDTO(comment);

            // Assert
            var returnValue = Assert.IsType<CommentDTO>(result);
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void AddDTO_IsNull()
        {
            // Arrange
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.AddDTO(null)).ReturnsAsync((CommentDTO)null);

            // Act
            var result = await mockRepo.Object.AddDTO(null);

            // Assert
            Assert.Null(result);
        }
    }
}