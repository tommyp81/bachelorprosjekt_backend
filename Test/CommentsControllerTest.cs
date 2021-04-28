using API.Controllers;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.Domain_models;
using Model.DTO;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class CommentsControllerTest
    {
        [Fact]
        public async void GetComments_Ok()
        {
            // Arrange
            var commentDTOs = TestCommentListDTO();
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
        public async void GetComments_IsNull()
        {
            // Arrange
            var mockRepo = new Mock<ICommentBLL>();
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.GetComments();

            // Assert
            var objectResult = result.Result as ObjectResult;
            var okResult = Assert.IsType<OkObjectResult>(objectResult);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async void GetComment_Ok()
        {
            // Arrange
            int id = 1;
            var commentDTO = TestCommentDTO();
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
        public async void GetComment_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<ICommentBLL>();
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.GetComment(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CommentDTO>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async void AddComment_Ok()
        {
            // Arrange
            var comment = TestComment();
            var commentDTO = TestCommentDTO();
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
        public async void AddComment_BadRequest()
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
        public async void UpdateComment_Ok()
        {
            // Arrange
            int id = 1;
            var comment = TestComment();
            var commentDTO = TestCommentDTO();
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
        public async void UpdateComment_NotFound()
        {
            // Arrange
            int id = 1;
            var comment = TestComment();
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.UpdateComment(comment));
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateComment(id, comment);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CommentDTO>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async void UpdateComment_BadRequest()
        {
            // Arrange
            int id = 0;
            var comment = TestComment();
            var commentDTO = TestCommentDTO();
            var mockRepo = new Mock<ICommentBLL>();
            mockRepo.Setup(repo => repo.UpdateComment(comment)).ReturnsAsync(commentDTO);
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateComment(id, comment);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CommentDTO>>(result);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public async void DeleteComment_Ok()
        {
            // Arrange
            int id = 1;
            var commentDTO = TestCommentDTO();
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
        public async void DeleteComment_NotFound()
        {
            // Arrange
            int id = 1;
            //var commentDTO = TestCommentDTO();
            var mockRepo = new Mock<ICommentBLL>();
            //mockRepo.Setup(repo => repo.DeleteComment(id)).ReturnsAsync(commentDTO).Verifiable();
            var controller = new CommentsController(mockRepo.Object);

            // Act
            var result = await controller.DeleteComment(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CommentDTO>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async void AddDTO_Ok()
        {
            // Arrange
            int id = 1;
            var comment = TestComment();
            var commentDTO = TestCommentDTO();
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
            mockRepo.Setup(repo => repo.AddDTO(null));

            // Act
            var result = await mockRepo.Object.AddDTO(null);

            // Assert
            Assert.Null(result);
        }

        private static ICollection<CommentDTO> TestCommentListDTO()
        {
            var comments = new List<CommentDTO>
            {
                new CommentDTO()
                {
                    Id = 1,
                    Content = "testkommentar1",
                    Date = DateTime.UtcNow,
                    UserId = 1,
                    PostId = 1,
                    DocumentId = 1
                },
                new CommentDTO()
                {
                    Id = 2,
                    Content = "testkommentar2",
                    Date = DateTime.UtcNow,
                    UserId = 2,
                    PostId = 2,
                    DocumentId = 2
                },
                new CommentDTO()
                {
                    Id = 3,
                    Content = "testkommentar3",
                    Date = DateTime.UtcNow,
                    UserId = 3,
                    PostId = 3,
                    DocumentId = 3
                },
            };
            return comments;
        }

        private static CommentDTO TestCommentDTO()
        {
            var comment = new CommentDTO()
            {
                Id = 1,
                Content = "testkommentar1",
                Date = DateTime.UtcNow,
                UserId = 1,
                PostId = 1,
                DocumentId = 1
            };
            return comment;
        }

        private static Comment TestComment()
        {
            var comment = new Comment()
            {
                Id = 1,
                Content = "testkommentar1",
                Date = DateTime.UtcNow,
                UserId = 1,
                PostId = 1,
                DocumentId = 1
            };
            return comment;
        }
    }
}
