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
    public class VideosControllerTests
    {
        [Fact]
        public async void GetVideosTest_Ok()
        {
            // Arrange
            var pagedResponse = VideoObject.TestPagedResponse(null);
            var mockRepo = new Mock<IVideoBLL>();
            mockRepo.Setup(repo => repo.PagedList(null, 1, 10, "Asc", "Date")).ReturnsAsync(pagedResponse);
            var controller = new VideosController(mockRepo.Object);

            // Act
            var result = await controller.GetVideos(null, 1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<VideoDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsAssignableFrom<PageResponse<IEnumerable<VideoDTO>>>(okResult.Value);
            int id = 1;
            foreach (var item in returnValue.Data)
            {
                Assert.Equal(id, item.Id);
                id++;
            }
            Assert.Equal(3, returnValue.Data.Count());
        }

        [Fact]
        public async void GetVideosTest_OkNull()
        {
            // Arrange
            var mockRepo = new Mock<IVideoBLL>();
            mockRepo.Setup(repo => repo.PagedList(null, 1, 10, "Asc", "Date")).ReturnsAsync((PageResponse<IEnumerable<VideoDTO>>)null);
            var controller = new VideosController(mockRepo.Object);

            // Act
            var result = await controller.GetVideos(null, 1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<VideoDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async void GetVideosTest_InternalServerError()
        {
            // Arrange
            var mockRepo = new Mock<IVideoBLL>();
            mockRepo.Setup(repo => repo.PagedList(null, 1, 10, "Asc", "Date")).ThrowsAsync(new InvalidOperationException());
            var controller = new VideosController(mockRepo.Object);

            // Act
            var result = await controller.GetVideos(null, 1, 10, "Asc", "Date");

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved henting av videoer", objectResult.Value);
        }

        [Fact]
        public async void GetVideoTest_Ok()
        {
            // Arrange
            int id = 1;
            var videoDTO = VideoObject.TestVideoDTO();
            var mockRepo = new Mock<IVideoBLL>();
            mockRepo.Setup(repo => repo.GetVideo(id)).ReturnsAsync(videoDTO);
            var controller = new VideosController(mockRepo.Object);

            // Act
            var result = await controller.GetVideo(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<VideoDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<VideoDTO>(okResult.Value);
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void GetVideoTest_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IVideoBLL>();
            mockRepo.Setup(repo => repo.GetVideo(id)).ReturnsAsync((VideoDTO)null);
            var controller = new VideosController(mockRepo.Object);

            // Act
            var result = await controller.GetVideo(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<VideoDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Video med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void GetVideoTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IVideoBLL>();
            mockRepo.Setup(repo => repo.GetVideo(id)).ThrowsAsync(new InvalidOperationException());
            var controller = new VideosController(mockRepo.Object);

            // Act
            var result = await controller.GetVideo(id);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved henting av video", objectResult.Value);
        }

        [Fact]
        public async void AddVideoTest_Ok()
        {
            // Arrange
            var video = VideoObject.TestVideo();
            var videoDTO = VideoObject.TestVideoDTO();
            var mockRepo = new Mock<IVideoBLL>();
            mockRepo.Setup(repo => repo.AddVideo(video)).ReturnsAsync(videoDTO).Verifiable();
            var controller = new VideosController(mockRepo.Object);

            // Act
            var result = await controller.AddVideo(video);

            // Assert
            var actionResult = Assert.IsType<ActionResult<VideoDTO>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<VideoDTO>(createdAtActionResult.Value);
            mockRepo.Verify();
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void AddVideoTest_BadRequest()
        {
            // Arrange
            var mockRepo = new Mock<IVideoBLL>();
            var controller = new VideosController(mockRepo.Object);

            // Act
            var result = await controller.AddVideo(null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<VideoDTO>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Video ble ikke opprettet", badRequestResult.Value);
        }

        [Fact]
        public async void AddVideoTest_InternalServerError()
        {
            // Arrange
            var video = VideoObject.TestVideo();
            var mockRepo = new Mock<IVideoBLL>();
            mockRepo.Setup(repo => repo.AddVideo(video)).ThrowsAsync(new InvalidOperationException());
            var controller = new VideosController(mockRepo.Object);

            // Act
            var result = await controller.AddVideo(video);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved oppretting av video", objectResult.Value);
        }

        [Fact]
        public async void UpdateVideoTest_Ok()
        {
            // Arrange
            int id = 1;
            var video = VideoObject.TestVideo();
            var videoDTO = VideoObject.TestVideoDTO();
            var mockRepo = new Mock<IVideoBLL>();
            mockRepo.Setup(repo => repo.UpdateVideo(video)).ReturnsAsync(videoDTO).Verifiable();
            var controller = new VideosController(mockRepo.Object);

            // Act
            var result = await controller.UpdateVideo(id, video);

            // Assert
            var actionResult = Assert.IsType<ActionResult<VideoDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<VideoDTO>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void UpdateVideoTest_NotFound()
        {
            // Arrange
            int id = 1;
            var video = VideoObject.TestVideo();
            var mockRepo = new Mock<IVideoBLL>();
            mockRepo.Setup(repo => repo.UpdateVideo(video)).ReturnsAsync((VideoDTO)null);
            var controller = new VideosController(mockRepo.Object);

            // Act
            var result = await controller.UpdateVideo(id, video);

            // Assert
            var actionResult = Assert.IsType<ActionResult<VideoDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Video med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void UpdateVideoTest_BadRequest()
        {
            // Arrange
            int id = 0;
            var video = VideoObject.TestVideo();
            var videoDTO = VideoObject.TestVideoDTO();
            var mockRepo = new Mock<IVideoBLL>();
            mockRepo.Setup(repo => repo.UpdateVideo(video)).ReturnsAsync(videoDTO);
            var controller = new VideosController(mockRepo.Object);

            // Act
            var result = await controller.UpdateVideo(id, video);

            // Assert
            var actionResult = Assert.IsType<ActionResult<VideoDTO>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Video ID stemmer ikke", badRequestResult.Value);
        }

        [Fact]
        public async void UpdateVideoTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var video = VideoObject.TestVideo();
            var mockRepo = new Mock<IVideoBLL>();
            mockRepo.Setup(repo => repo.UpdateVideo(video)).ThrowsAsync(new InvalidOperationException());
            var controller = new VideosController(mockRepo.Object);

            // Act
            var result = await controller.UpdateVideo(id, video);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved oppdatering av video", objectResult.Value);
        }

        [Fact]
        public async void DeleteVideoTest_Ok()
        {
            // Arrange
            int id = 1;
            var videoDTO = VideoObject.TestVideoDTO();
            var mockRepo = new Mock<IVideoBLL>();
            mockRepo.Setup(repo => repo.DeleteVideo(id)).ReturnsAsync(videoDTO).Verifiable();
            var controller = new VideosController(mockRepo.Object);

            // Act
            var result = await controller.DeleteVideo(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<VideoDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<VideoDTO>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void DeleteVideoTest_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IVideoBLL>();
            mockRepo.Setup(repo => repo.DeleteVideo(id)).ReturnsAsync((VideoDTO)null);
            var controller = new VideosController(mockRepo.Object);

            // Act
            var result = await controller.DeleteVideo(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<VideoDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Video med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void DeleteVideoTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var videoDTO = VideoObject.TestVideoDTO();
            var mockRepo = new Mock<IVideoBLL>();
            mockRepo.Setup(repo => repo.DeleteVideo(id)).ThrowsAsync(new InvalidOperationException());
            var controller = new VideosController(mockRepo.Object);

            // Act
            var result = await controller.DeleteVideo(id);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved sletting av video", objectResult.Value);
        }

        [Fact]
        public async void SearchTest_Ok()
        {
            // Arrange
            string query = "Video Test 1";
            var pagedResponse = VideoObject.TestPagedResponse(query);
            var mockRepo = new Mock<IVideoBLL>();
            mockRepo.Setup(repo => repo.Search(query, null, 1, 10, "Asc", "Date")).ReturnsAsync(pagedResponse);
            var controller = new VideosController(mockRepo.Object);

            // Act
            var result = await controller.Search(query, null, 1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<VideoDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsAssignableFrom<PageResponse<IEnumerable<VideoDTO>>>(okResult.Value);
            foreach (var item in returnValue.Data)
            {
                Assert.Equal(query, item.Title);
            }
        }

        [Fact]
        public async void SearchTest_OkNull()
        {
            // Arrange
            var mockRepo = new Mock<IVideoBLL>();
            mockRepo.Setup(repo => repo.Search(null, null, 1, 10, "Asc", "Date")).ReturnsAsync((PageResponse<IEnumerable<VideoDTO>>)null);
            var controller = new VideosController(mockRepo.Object);

            // Act
            var result = await controller.Search(null, null, 1, 10, "Asc", "Date");

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<VideoDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async void SearchTest_InternalServerError()
        {
            // Arrange
            var mockRepo = new Mock<IVideoBLL>();
            mockRepo.Setup(repo => repo.Search(null, null, 1, 10, "Asc", "Date")).ThrowsAsync(new InvalidOperationException());
            var controller = new VideosController(mockRepo.Object);

            // Act
            var result = await controller.Search(null, null, 1, 10, "Asc", "Date");

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved søk i videoer", objectResult.Value);
        }
    }
}