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

namespace API.Controllers.Tests
{
    public class LikesControllerTests
    {
        [Fact]
        public async void GetLikeTest_Ok()
        {
            // Arrange
            var like = LikeObject.TestLike();
            var likeDTO = LikeObject.TestLikeDTO();
            var mockRepo = new Mock<ILikeBLL>();
            mockRepo.Setup(repo => repo.GetLike(like)).ReturnsAsync(likeDTO).Verifiable();
            var controller = new LikesController(mockRepo.Object);

            // Act
            var result = await controller.GetLike(like);

            // Assert
            var actionResult = Assert.IsType<ActionResult<LikeDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<LikeDTO>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void GetLikeTest_BadRequest()
        {
            // Arrange
            var like = LikeObject.TestLike2();
            var likeDTO = LikeObject.TestLikeDTO();
            var mockRepo = new Mock<ILikeBLL>();
            mockRepo.Setup(repo => repo.GetLike(like)).ReturnsAsync(likeDTO).Verifiable();
            var controller = new LikesController(mockRepo.Object);

            // Act
            var result = await controller.GetLike(like);

            // Assert
            var actionResult = Assert.IsType<ActionResult<LikeDTO>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Det må legges ved en bruker ID", badRequestResult.Value);
        }

        [Fact]
        public async void GetLikeTest_InternalServerError()
        {
            // Arrange
            var like = LikeObject.TestLike();
            var mockRepo = new Mock<ILikeBLL>();
            mockRepo.Setup(repo => repo.GetLike(like)).ThrowsAsync(new InvalidOperationException());
            var controller = new LikesController(mockRepo.Object);

            // Act
            var result = await controller.GetLike(like);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved henting av like", objectResult.Value);
        }

        [Fact]
        public async void AddLikeTest_Ok()
        {
            // Arrange
            var like = LikeObject.TestLike();
            var likeDTO = LikeObject.TestLikeDTO();
            var mockRepo = new Mock<ILikeBLL>();
            mockRepo.Setup(repo => repo.AddLike(like)).ReturnsAsync(likeDTO).Verifiable();
            var controller = new LikesController(mockRepo.Object);

            // Act
            var result = await controller.AddLike(like);

            // Assert
            var actionResult = Assert.IsType<ActionResult<LikeDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<LikeDTO>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void AddLikeTest_BadRequest()
        {
            // Arrange
            var like = LikeObject.TestLike2();
            var likeDTO = LikeObject.TestLikeDTO();
            var mockRepo = new Mock<ILikeBLL>();
            mockRepo.Setup(repo => repo.AddLike(like)).ReturnsAsync(likeDTO).Verifiable();
            var controller = new LikesController(mockRepo.Object);

            // Act
            var result = await controller.AddLike(like);

            // Assert
            var actionResult = Assert.IsType<ActionResult<LikeDTO>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Det må legges ved en bruker ID", badRequestResult.Value);
        }

        [Fact]
        public async void AddLikeTest_InternalServerError()
        {
            // Arrange
            var like = LikeObject.TestLike();
            var mockRepo = new Mock<ILikeBLL>();
            mockRepo.Setup(repo => repo.AddLike(like)).ThrowsAsync(new InvalidOperationException());
            var controller = new LikesController(mockRepo.Object);

            // Act
            var result = await controller.AddLike(like);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved oppretting av like", objectResult.Value);
        }

        [Fact]
        public async void DeleteLikeTest_Ok()
        {
            // Arrange
            var like = LikeObject.TestLike();
            var likeDTO = LikeObject.TestLikeDTO();
            var mockRepo = new Mock<ILikeBLL>();
            mockRepo.Setup(repo => repo.DeleteLike(like)).ReturnsAsync(likeDTO).Verifiable();
            var controller = new LikesController(mockRepo.Object);

            // Act
            var result = await controller.DeleteLike(like);

            // Assert
            var actionResult = Assert.IsType<ActionResult<LikeDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<LikeDTO>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void DeleteLikeTest_BadRequest()
        {
            // Arrange
            var like = LikeObject.TestLike2();
            var likeDTO = LikeObject.TestLikeDTO();
            var mockRepo = new Mock<ILikeBLL>();
            mockRepo.Setup(repo => repo.DeleteLike(like)).ReturnsAsync(likeDTO).Verifiable();
            var controller = new LikesController(mockRepo.Object);

            // Act
            var result = await controller.DeleteLike(like);

            // Assert
            var actionResult = Assert.IsType<ActionResult<LikeDTO>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Det må legges ved en bruker ID", badRequestResult.Value);
        }

        [Fact]
        public async void DeleteLikeTest_InternalServerError()
        {
            // Arrange
            var like = LikeObject.TestLike();
            var mockRepo = new Mock<ILikeBLL>();
            mockRepo.Setup(repo => repo.DeleteLike(like)).ThrowsAsync(new InvalidOperationException());
            var controller = new LikesController(mockRepo.Object);

            // Act
            var result = await controller.DeleteLike(like);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved sletting av like", objectResult.Value);
        }
    }
}