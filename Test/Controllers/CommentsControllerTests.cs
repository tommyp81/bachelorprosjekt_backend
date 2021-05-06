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
using DAL.Helpers;

namespace API.Controllers.Tests
{
    public class CommentsControllerTests
    {
        [Fact]
        public async void GetCommentsTest_Ok()
        {
            // Arrange
            var pagedResponse = CommentObject.TestPagedResponse(null);
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.PagedList(null, 1, 10, "Asc", "Date")).ReturnsAsync(pagedResponse);
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.GetComments(null, 1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<CommentDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsAssignableFrom<PageResponse<IEnumerable<CommentDTO>>>(okResult.Value);
            int id = 1;
            foreach (var item in returnValue.Data)
            {
                Assert.Equal(id, item.Id);
                id++;
            }
        }

        [Fact]
        public async void GetCommentsTest_OkNull()
        {
            // Arrange
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.PagedList(null, 1, 10, "Asc", "Date")).ReturnsAsync((PageResponse<IEnumerable<CommentDTO>>)null);
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.GetComments(null, 1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<CommentDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async void GetCommentsTest_InternalServerError()
        {
            // Arrange
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.PagedList(null, 1, 10, "Asc", "Date")).ThrowsAsync(new InvalidOperationException());
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.GetComments(null, 1, 10, "Asc", "Date");

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
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
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
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Kommentar med ID 1 ble ikke funnet", notFoundResult.Value);
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
            Assert.Equal("Feil ved henting av kommentar", objectResult.Value);
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
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Kommentar ble ikke opprettet", badRequestResult.Value);
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
            Assert.Equal("Feil ved oppretting av kommentar", objectResult.Value);
        }

        [Fact]
        public async void UpdateCommentTest_Ok()
        {
            // Arrange
            int id = 1;
            var comment = CommentObject.TestComment();
            var commentDTO = CommentObject.TestCommentDTO();
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.UpdateComment(comment)).ReturnsAsync(commentDTO).Verifiable();
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateComment(id, comment);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CommentDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
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
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Kommentar med ID 1 ble ikke funnet", notFoundResult.Value);
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
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Kommentar ID stemmer ikke", badRequestResult.Value);
        }

        [Fact]
        public async void UpdateCommentTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var comment = CommentObject.TestComment();
            var mockRepo = new Mock<ICommentBLL>();
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
            mockRepo.Setup(repo => repo.DeleteComment(id)).ReturnsAsync(commentDTO).Verifiable();
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.DeleteComment(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CommentDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
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
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Kommentar med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void DeleteCommentTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var commentDTO = CommentObject.TestCommentDTO();
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.DeleteComment(id)).ThrowsAsync(new InvalidOperationException());
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.DeleteComment(id);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved sletting av kommentar", objectResult.Value);
        }

        [Fact]
        public async void SearchTest_Ok()
        {
            // Arrange
            string query = "testkommentar1";
            var pagedResponse = CommentObject.TestPagedResponse(query);
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.Search(query, null, 1, 10, "Asc", "Date")).ReturnsAsync(pagedResponse);
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.Search(query, null, 1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<CommentDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsAssignableFrom<PageResponse<IEnumerable<CommentDTO>>>(okResult.Value);
            foreach (var item in returnValue.Data)
            {
                Assert.Equal(query, item.Content);
            }
        }

        [Fact]
        public async void SearchTest_OkNull()
        {
            // Arrange
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.Search(null, null, 1, 10, "Asc", "Date")).ReturnsAsync((PageResponse<IEnumerable<CommentDTO>>)null);
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.Search(null, null, 1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<CommentDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async void SearchTest_InternalServerError()
        {
            // Arrange
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.Search(null, null, 1, 10, "Asc", "Date")).ThrowsAsync(new InvalidOperationException());
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.Search(null, null, 1, 10, "Asc", "Date");

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved søk i kommentarer", objectResult.Value);
        }
    }
}