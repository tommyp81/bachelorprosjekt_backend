using Xunit;
using API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Objects;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Moq;
using Model.DTO;
using DAL.Helpers;

namespace API.Controllers.Tests
{
    public class PostsControllerTests
    {
        [Fact]
        public async void GetPostsTest_Ok()
        {
            // Arrange
            var pagedResponse = PostObject.TestPagedResponse(null);
            var mockRepo = new Mock<IPostBLL>();
            mockRepo.Setup(repo => repo.PagedList(null, 1, 10, "Asc", "Date")).ReturnsAsync(pagedResponse);
            var controller = new PostsController(mockRepo.Object);

            // Act
            var result = await controller.GetPosts(null, 1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<PostDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsAssignableFrom<PageResponse<IEnumerable<PostDTO>>>(okResult.Value);
            int id = 1;
            foreach (var item in returnValue.Data)
            {
                Assert.Equal(id, item.Id);
                id++;
            }
            Assert.Equal(3, returnValue.Data.Count());
        }

        [Fact]
        public async void GetPostsTest_OkNull()
        {
            // Arrange
            var mockRepo = new Mock<IPostBLL>();
            mockRepo.Setup(repo => repo.PagedList(null, 1, 10, "Asc", "Date")).ReturnsAsync((PageResponse<IEnumerable<PostDTO>>)null);
            var controller = new PostsController(mockRepo.Object);

            // Act
            var result = await controller.GetPosts(null, 1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<PostDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async void GetPostsTest_InternalServerError()
        {
            // Arrange
            var mockRepo = new Mock<IPostBLL>();
            mockRepo.Setup(repo => repo.PagedList(null, 1, 10, "Asc", "Date")).ThrowsAsync(new InvalidOperationException());
            var controller = new PostsController(mockRepo.Object);

            // Act
            var result = await controller.GetPosts(null, 1, 10, "Asc", "Date");

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved henting av poster", objectResult.Value);
        }

        [Fact]
        public async void GetPostTest_Ok()
        {
            // Arrange
            int id = 1;
            var postDTO = PostObject.TestPostDTO();
            var mockRepo = new Mock<IPostBLL>();
            mockRepo.Setup(repo => repo.GetPost(id)).ReturnsAsync(postDTO);
            var controller = new PostsController(mockRepo.Object);

            // Act
            var result = await controller.GetPost(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PostDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<PostDTO>(okResult.Value);
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void GetPostTest_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IPostBLL>();
            mockRepo.Setup(repo => repo.GetPost(id)).ReturnsAsync((PostDTO)null);
            var controller = new PostsController(mockRepo.Object);

            // Act
            var result = await controller.GetPost(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PostDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Post med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void GetPostTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IPostBLL>();
            mockRepo.Setup(repo => repo.GetPost(id)).ThrowsAsync(new InvalidOperationException());
            var controller = new PostsController(mockRepo.Object);

            // Act
            var result = await controller.GetPost(id);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved henting av post", objectResult.Value);
        }

        [Fact]
        public async void AddPostTest_Ok()
        {
            // Arrange
            var post = PostObject.TestPost();
            var postDTO = PostObject.TestPostDTO();
            var mockRepo = new Mock<IPostBLL>();
            mockRepo.Setup(repo => repo.AddPost(null, post)).ReturnsAsync(postDTO).Verifiable();
            var controller = new PostsController(mockRepo.Object);

            // Act
            var result = await controller.AddPost(null, post);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PostDTO>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<PostDTO>(createdAtActionResult.Value);
            mockRepo.Verify();
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void AddPostTest_BadRequest()
        {
            // Arrange
            var mockRepo = new Mock<IPostBLL>();
            var controller = new PostsController(mockRepo.Object);

            // Act
            var result = await controller.AddPost(null, null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PostDTO>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Post ble ikke opprettet", badRequestResult.Value);
        }

        [Fact]
        public async void AddPostTest_InternalServerError()
        {
            // Arrange
            var post = PostObject.TestPost();
            var mockRepo = new Mock<IPostBLL>();
            mockRepo.Setup(repo => repo.AddPost(null, post)).ThrowsAsync(new InvalidOperationException());
            var controller = new PostsController(mockRepo.Object);

            // Act
            var result = await controller.AddPost(null, post);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved oppretting av post", objectResult.Value);
        }

        [Fact]
        public async void UpdatePostTest_Ok()
        {
            // Arrange
            int id = 1;
            var post = PostObject.TestPost();
            var postDTO = PostObject.TestPostDTO();
            var mockRepo = new Mock<IPostBLL>();
            mockRepo.Setup(repo => repo.UpdatePost(post)).ReturnsAsync(postDTO).Verifiable();
            var controller = new PostsController(mockRepo.Object);

            // Act
            var result = await controller.UpdatePost(id, post);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PostDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<PostDTO>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void UpdatePostTest_NotFound()
        {
            // Arrange
            int id = 1;
            var post = PostObject.TestPost();
            var mockRepo = new Mock<IPostBLL>();
            mockRepo.Setup(repo => repo.UpdatePost(post)).ReturnsAsync((PostDTO)null);
            var controller = new PostsController(mockRepo.Object);

            // Act
            var result = await controller.UpdatePost(id, post);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PostDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Post med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void UpdatePostTest_BadRequest()
        {
            // Arrange
            int id = 0;
            var post = PostObject.TestPost();
            var postDTO = PostObject.TestPostDTO();
            var mockRepo = new Mock<IPostBLL>();
            mockRepo.Setup(repo => repo.UpdatePost(post)).ReturnsAsync(postDTO);
            var controller = new PostsController(mockRepo.Object);

            // Act
            var result = await controller.UpdatePost(id, post);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PostDTO>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Post ID stemmer ikke", badRequestResult.Value);
        }

        [Fact]
        public async void UpdatePostTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var post = PostObject.TestPost();
            var mockRepo = new Mock<IPostBLL>();
            mockRepo.Setup(repo => repo.UpdatePost(post)).ThrowsAsync(new InvalidOperationException());
            var controller = new PostsController(mockRepo.Object);

            // Act
            var result = await controller.UpdatePost(id, post);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved oppdatering av post", objectResult.Value);
        }

        [Fact]
        public async void DeletePostTest_Ok()
        {
            // Arrange
            int id = 1;
            var postDTO = PostObject.TestPostDTO();
            var mockRepo = new Mock<IPostBLL>();
            mockRepo.Setup(repo => repo.DeletePost(id)).ReturnsAsync(postDTO).Verifiable();
            var controller = new PostsController(mockRepo.Object);

            // Act
            var result = await controller.DeletePost(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PostDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<PostDTO>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void DeletePostTest_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IPostBLL>();
            mockRepo.Setup(repo => repo.DeletePost(id)).ReturnsAsync((PostDTO)null);
            var controller = new PostsController(mockRepo.Object);

            // Act
            var result = await controller.DeletePost(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PostDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Post med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void DeletePostTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var postDTO = PostObject.TestPostDTO();
            var mockRepo = new Mock<IPostBLL>();
            mockRepo.Setup(repo => repo.DeletePost(id)).ThrowsAsync(new InvalidOperationException());
            var controller = new PostsController(mockRepo.Object);

            // Act
            var result = await controller.DeletePost(id);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved sletting av post", objectResult.Value);
        }

        [Fact]
        public async void SearchTest_Ok()
        {
            // Arrange
            string query = "testpost1";
            var pagedResponse = PostObject.TestPagedResponse(query);
            var mockRepo = new Mock<IPostBLL>();
            mockRepo.Setup(repo => repo.Search(query, null, 1, 10, "Asc", "Date")).ReturnsAsync(pagedResponse);
            var controller = new PostsController(mockRepo.Object);

            // Act
            var result = await controller.Search(query, null, 1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<PostDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsAssignableFrom<PageResponse<IEnumerable<PostDTO>>>(okResult.Value);
            foreach (var item in returnValue.Data)
            {
                Assert.Equal(query, item.Content);
            }
        }

        [Fact]
        public async void SearchTest_OkNull()
        {
            // Arrange
            var mockRepo = new Mock<IPostBLL>();
            mockRepo.Setup(repo => repo.Search(null, null, 1, 10, "Asc", "Date")).ReturnsAsync((PageResponse<IEnumerable<PostDTO>>)null);
            var controller = new PostsController(mockRepo.Object);

            // Act
            var result = await controller.Search(null, null, 1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<PostDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async void SearchTest_InternalServerError()
        {
            // Arrange
            var mockRepo = new Mock<IPostBLL>();
            mockRepo.Setup(repo => repo.Search(null, null, 1, 10, "Asc", "Date")).ThrowsAsync(new InvalidOperationException());
            var controller = new PostsController(mockRepo.Object);

            // Act
            var result = await controller.Search(null, null, 1, 10, "Asc", "Date");

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved søk i poster", objectResult.Value);
        }
    }
}