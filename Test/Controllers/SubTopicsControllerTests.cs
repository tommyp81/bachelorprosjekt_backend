using Xunit;
using API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using BLL.Interfaces;
using Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Test.Objects;

namespace API.Controllers.Tests
{
    public class SubTopicsControllerTests
    {
        [Fact]
        public async void GetSubTopicsTest_Ok()
        {
            // Arrange
            var subtopicDTOs = SubTopicObject.TestSubTopicListDTO();
            var mockRepo = new Mock<ISubTopicBLL>();
            mockRepo.Setup(repo => repo.GetSubTopics()).ReturnsAsync(subtopicDTOs);
            var controller = new SubTopicsController(mockRepo.Object);

            // Act
            var result = await controller.GetSubTopics();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<SubTopicDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<SubTopicDTO>>(okResult.Value);
            int id = 1;
            foreach (var item in returnValue)
            {
                Assert.Equal(id, item.Id);
                id++;
            }
            Assert.Equal(3, returnValue.Count());
        }

        [Fact]
        public async void GetSubTopicsTest_OkNull()
        {
            // Arrange
            var mockRepo = new Mock<ISubTopicBLL>();
            mockRepo.Setup(repo => repo.GetSubTopics()).ReturnsAsync((IEnumerable<SubTopicDTO>)null);
            var controller = new SubTopicsController(mockRepo.Object);

            // Act
            var result = await controller.GetSubTopics();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<SubTopicDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async void GetSubTopicsTest_InternalServerError()
        {
            // Arrange
            var mockRepo = new Mock<ISubTopicBLL>();
            mockRepo.Setup(repo => repo.GetSubTopics()).ThrowsAsync(new InvalidOperationException());
            var controller = new SubTopicsController(mockRepo.Object);

            // Act
            var result = await controller.GetSubTopics();

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved henting av underemne", objectResult.Value);
        }

        [Fact]
        public async void GetSubTopicTest_Ok()
        {
            // Arrange
            int id = 1;
            var subtopicDTO = SubTopicObject.TestSubTopicDTO();
            var mockRepo = new Mock<ISubTopicBLL>();
            mockRepo.Setup(repo => repo.GetSubTopic(id)).ReturnsAsync(subtopicDTO);
            var controller = new SubTopicsController(mockRepo.Object);

            // Act
            var result = await controller.GetSubTopic(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<SubTopicDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<SubTopicDTO>(okResult.Value);
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void GetSubTopicTest_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<ISubTopicBLL>();
            mockRepo.Setup(repo => repo.GetSubTopic(id)).ReturnsAsync((SubTopicDTO)null);
            var controller = new SubTopicsController(mockRepo.Object);

            // Act
            var result = await controller.GetSubTopic(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<SubTopicDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Underemne med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void GetSubTopicTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<ISubTopicBLL>();
            mockRepo.Setup(repo => repo.GetSubTopic(id)).ThrowsAsync(new InvalidOperationException());
            var controller = new SubTopicsController(mockRepo.Object);

            // Act
            var result = await controller.GetSubTopic(id);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved henting av underemne", objectResult.Value);
        }

        [Fact]
        public async void AddSubTopicTest_Ok()
        {
            // Arrange
            var subtopic = SubTopicObject.TestSubTopic();
            var subtopicDTO = SubTopicObject.TestSubTopicDTO();
            var mockRepo = new Mock<ISubTopicBLL>();
            mockRepo.Setup(repo => repo.AddSubTopic(subtopic)).ReturnsAsync(subtopicDTO).Verifiable();
            var controller = new SubTopicsController(mockRepo.Object);

            // Act
            var result = await controller.AddSubTopic(subtopic);

            // Assert
            var actionResult = Assert.IsType<ActionResult<SubTopicDTO>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<SubTopicDTO>(createdAtActionResult.Value);
            mockRepo.Verify();
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void AddSubTopicTest_BadRequest()
        {
            // Arrange
            var mockRepo = new Mock<ISubTopicBLL>();
            var controller = new SubTopicsController(mockRepo.Object);

            // Act
            var result = await controller.AddSubTopic(null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<SubTopicDTO>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Underemne ble ikke opprettet", badRequestResult.Value);
        }

        [Fact]
        public async void AddSubTopicTest_InternalServerError()
        {
            // Arrange
            var subtopic = SubTopicObject.TestSubTopic();
            var mockRepo = new Mock<ISubTopicBLL>();
            mockRepo.Setup(repo => repo.AddSubTopic(subtopic)).ThrowsAsync(new InvalidOperationException());
            var controller = new SubTopicsController(mockRepo.Object);

            // Act
            var result = await controller.AddSubTopic(subtopic);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved oppretting av nytt underemne", objectResult.Value);
        }

        [Fact]
        public async void UpdateSubTopicTest_Ok()
        {
            // Arrange
            int id = 1;
            var subtopic = SubTopicObject.TestSubTopic();
            var subtopicDTO = SubTopicObject.TestSubTopicDTO();
            var mockRepo = new Mock<ISubTopicBLL>();
            mockRepo.Setup(repo => repo.UpdateSubTopic(subtopic)).ReturnsAsync(subtopicDTO).Verifiable();
            var controller = new SubTopicsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateSubTopic(id, subtopic);

            // Assert
            var actionResult = Assert.IsType<ActionResult<SubTopicDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<SubTopicDTO>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void UpdateSubTopicTest_NotFound()
        {
            // Arrange
            int id = 1;
            var subtopic = SubTopicObject.TestSubTopic();
            var mockRepo = new Mock<ISubTopicBLL>();
            mockRepo.Setup(repo => repo.UpdateSubTopic(subtopic)).ReturnsAsync((SubTopicDTO)null);
            var controller = new SubTopicsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateSubTopic(id, subtopic);

            // Assert
            var actionResult = Assert.IsType<ActionResult<SubTopicDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Underemne med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void UpdateSubTopicTest_BadRequest()
        {
            // Arrange
            int id = 0;
            var subtopic = SubTopicObject.TestSubTopic();
            var subtopicDTO = SubTopicObject.TestSubTopicDTO();
            var mockRepo = new Mock<ISubTopicBLL>();
            mockRepo.Setup(repo => repo.UpdateSubTopic(subtopic)).ReturnsAsync(subtopicDTO);
            var controller = new SubTopicsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateSubTopic(id, subtopic);

            // Assert
            var actionResult = Assert.IsType<ActionResult<SubTopicDTO>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Underemne ID stemmer ikke", badRequestResult.Value);
        }

        [Fact]
        public async void UpdateSubTopicTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var subtopic = SubTopicObject.TestSubTopic();
            var mockRepo = new Mock<ISubTopicBLL>();
            mockRepo.Setup(repo => repo.UpdateSubTopic(subtopic)).ThrowsAsync(new InvalidOperationException());
            var controller = new SubTopicsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateSubTopic(id, subtopic);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved oppdatering av underemne", objectResult.Value);
        }

        [Fact]
        public async void DeleteSubTopicTest_Ok()
        {
            // Arrange
            int id = 1;
            var subtopicDTO = SubTopicObject.TestSubTopicDTO();
            var mockRepo = new Mock<ISubTopicBLL>();
            mockRepo.Setup(repo => repo.DeleteSubTopic(id)).ReturnsAsync(subtopicDTO).Verifiable();
            var controller = new SubTopicsController(mockRepo.Object);

            // Act
            var result = await controller.DeleteSubTopic(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<SubTopicDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<SubTopicDTO>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void DeleteSubTopicTest_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<ISubTopicBLL>();
            mockRepo.Setup(repo => repo.DeleteSubTopic(id)).ReturnsAsync((SubTopicDTO)null);
            var controller = new SubTopicsController(mockRepo.Object);

            // Act
            var result = await controller.DeleteSubTopic(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<SubTopicDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Underemne med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void DeleteSubTopicTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var DTO = SubTopicObject.TestSubTopicDTO();
            var mockRepo = new Mock<ISubTopicBLL>();
            mockRepo.Setup(repo => repo.DeleteSubTopic(id)).ThrowsAsync(new InvalidOperationException());
            var controller = new SubTopicsController(mockRepo.Object);

            // Act
            var result = await controller.DeleteSubTopic(id);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved sletting av underemne", objectResult.Value);
        }
    }
}