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
using Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Tests
{
    public class TopicsControllerTests
    {
        [Fact]
        public async void GetTopicsTest_Ok()
        {
            // Arrange
            var topicDTOs = TopicObject.TestTopicListDTO();
            var mockRepo = new Mock<ITopicBLL>();
            mockRepo.Setup(repo => repo.GetTopics()).ReturnsAsync(topicDTOs);
            var controller = new TopicsController(mockRepo.Object);

            // Act
            var result = await controller.GetTopics();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<TopicDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<TopicDTO>>(okResult.Value);
            int id = 1;
            foreach (var item in returnValue)
            {
                Assert.Equal(id, item.Id);
                id++;
            }
            Assert.Equal(3, returnValue.Count());
        }

        [Fact]
        public async void GetTopicsTest_OkNull()
        {
            // Arrange
            var mockRepo = new Mock<ITopicBLL>();
            mockRepo.Setup(repo => repo.GetTopics()).ReturnsAsync((IEnumerable<TopicDTO>)null);
            var controller = new TopicsController(mockRepo.Object);

            // Act
            var result = await controller.GetTopics();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<TopicDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async void GetTopicsTest_InternalServerError()
        {
            // Arrange
            var mockRepo = new Mock<ITopicBLL>();
            mockRepo.Setup(repo => repo.GetTopics()).ThrowsAsync(new InvalidOperationException());
            var controller = new TopicsController(mockRepo.Object);

            // Act
            var result = await controller.GetTopics();

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved henting av emne", objectResult.Value);
        }

        [Fact]
        public async void GetTopicTest_Ok()
        {
            // Arrange
            int id = 1;
            var topicDTO = TopicObject.TestTopicDTO();
            var mockRepo = new Mock<ITopicBLL>();
            mockRepo.Setup(repo => repo.GetTopic(id)).ReturnsAsync(topicDTO);
            var controller = new TopicsController(mockRepo.Object);

            // Act
            var result = await controller.GetTopic(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<TopicDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<TopicDTO>(okResult.Value);
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void GetTopicTest_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<ITopicBLL>();
            mockRepo.Setup(repo => repo.GetTopic(id)).ReturnsAsync((TopicDTO)null);
            var controller = new TopicsController(mockRepo.Object);

            // Act
            var result = await controller.GetTopic(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<TopicDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Emne med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void GetTopicTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<ITopicBLL>();
            mockRepo.Setup(repo => repo.GetTopic(id)).ThrowsAsync(new InvalidOperationException());
            var controller = new TopicsController(mockRepo.Object);

            // Act
            var result = await controller.GetTopic(id);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved henting av emne", objectResult.Value);
        }

        [Fact]
        public async void AddTopicTest_Ok()
        {
            // Arrange
            var topic = TopicObject.TestTopic();
            var topicDTO = TopicObject.TestTopicDTO();
            var mockRepo = new Mock<ITopicBLL>();
            mockRepo.Setup(repo => repo.AddTopic(topic)).ReturnsAsync(topicDTO).Verifiable();
            var controller = new TopicsController(mockRepo.Object);

            // Act
            var result = await controller.AddTopic(topic);

            // Assert
            var actionResult = Assert.IsType<ActionResult<TopicDTO>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<TopicDTO>(createdAtActionResult.Value);
            mockRepo.Verify();
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void AddTopicTest_BadRequest()
        {
            // Arrange
            var mockRepo = new Mock<ITopicBLL>();
            var controller = new TopicsController(mockRepo.Object);

            // Act
            var result = await controller.AddTopic(null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<TopicDTO>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Emne ble ikke opprettet", badRequestResult.Value);
        }

        [Fact]
        public async void AddTopicTest_InternalServerError()
        {
            // Arrange
            var topic = TopicObject.TestTopic();
            var mockRepo = new Mock<ITopicBLL>();
            mockRepo.Setup(repo => repo.AddTopic(topic)).ThrowsAsync(new InvalidOperationException());
            var controller = new TopicsController(mockRepo.Object);

            // Act
            var result = await controller.AddTopic(topic);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved oppretting av nytt emne", objectResult.Value);
        }

        [Fact]
        public async void UpdateTopicTest_Ok()
        {
            // Arrange
            int id = 1;
            var topic = TopicObject.TestTopic();
            var topicDTO = TopicObject.TestTopicDTO();
            var mockRepo = new Mock<ITopicBLL>();
            mockRepo.Setup(repo => repo.UpdateTopic(topic)).ReturnsAsync(topicDTO).Verifiable();
            var controller = new TopicsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateTopic(id, topic);

            // Assert
            var actionResult = Assert.IsType<ActionResult<TopicDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<TopicDTO>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void UpdateTopicTest_NotFound()
        {
            // Arrange
            int id = 1;
            var topic = TopicObject.TestTopic();
            var mockRepo = new Mock<ITopicBLL>();
            mockRepo.Setup(repo => repo.UpdateTopic(topic)).ReturnsAsync((TopicDTO)null);
            var controller = new TopicsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateTopic(id, topic);

            // Assert
            var actionResult = Assert.IsType<ActionResult<TopicDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Emne med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void UpdateTopicTest_BadRequest()
        {
            // Arrange
            int id = 0;
            var topic = TopicObject.TestTopic();
            var topicDTO = TopicObject.TestTopicDTO();
            var mockRepo = new Mock<ITopicBLL>();
            mockRepo.Setup(repo => repo.UpdateTopic(topic)).ReturnsAsync(topicDTO);
            var controller = new TopicsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateTopic(id, topic);

            // Assert
            var actionResult = Assert.IsType<ActionResult<TopicDTO>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Emne ID stemmer ikke", badRequestResult.Value);
        }

        [Fact]
        public async void UpdateTopicTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var topic = TopicObject.TestTopic();
            var mockRepo = new Mock<ITopicBLL>();
            mockRepo.Setup(repo => repo.UpdateTopic(topic)).ThrowsAsync(new InvalidOperationException());
            var controller = new TopicsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateTopic(id, topic);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved oppdatering av emne", objectResult.Value);
        }

        [Fact]
        public async void DeleteTopicTest_Ok()
        {
            // Arrange
            int id = 1;
            var topicDTO = TopicObject.TestTopicDTO();
            var mockRepo = new Mock<ITopicBLL>();
            mockRepo.Setup(repo => repo.DeleteTopic(id)).ReturnsAsync(topicDTO).Verifiable();
            var controller = new TopicsController(mockRepo.Object);

            // Act
            var result = await controller.DeleteTopic(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<TopicDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<TopicDTO>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void DeleteTopicTest_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<ITopicBLL>();
            mockRepo.Setup(repo => repo.DeleteTopic(id)).ReturnsAsync((TopicDTO)null);
            var controller = new TopicsController(mockRepo.Object);

            // Act
            var result = await controller.DeleteTopic(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<TopicDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Emne med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void DeleteTopicTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var DTO = TopicObject.TestTopicDTO();
            var mockRepo = new Mock<ITopicBLL>();
            mockRepo.Setup(repo => repo.DeleteTopic(id)).ThrowsAsync(new InvalidOperationException());
            var controller = new TopicsController(mockRepo.Object);

            // Act
            var result = await controller.DeleteTopic(id);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved sletting av emne", objectResult.Value);
        }
    }
}