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
    public class InfoTopicsControllerTests
    {
        [Fact]
        public async void GetInfoTopicsTest_Ok()
        {
            // Arrange
            var infotopicDTOs = InfoTopicObject.TestInfoTopicListDTO();
            var mockRepo = new Mock<IInfoTopicBLL>();
            mockRepo.Setup(repo => repo.GetInfoTopics()).ReturnsAsync(infotopicDTOs);
            var controller = new InfoTopicsController(mockRepo.Object);

            // Act
            var result = await controller.GetInfoTopics();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<InfoTopicDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<InfoTopicDTO>>(okResult.Value);
            int id = 1;
            foreach (var item in returnValue)
            {
                Assert.Equal(id, item.Id);
                id++;
            }
            Assert.Equal(3, returnValue.Count());
        }

        [Fact]
        public async void GetInfoTopicsTest_OkNull()
        {
            // Arrange
            var mockRepo = new Mock<IInfoTopicBLL>();
            mockRepo.Setup(repo => repo.GetInfoTopics()).ReturnsAsync((IEnumerable<InfoTopicDTO>)null);
            var controller = new InfoTopicsController(mockRepo.Object);

            // Act
            var result = await controller.GetInfoTopics();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<InfoTopicDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async void GetInfoTopicsTest_InternalServerError()
        {
            // Arrange
            var mockRepo = new Mock<IInfoTopicBLL>();
            mockRepo.Setup(repo => repo.GetInfoTopics()).ThrowsAsync(new InvalidOperationException());
            var controller = new InfoTopicsController(mockRepo.Object);

            // Act
            var result = await controller.GetInfoTopics();

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved henting av emner", objectResult.Value);
        }

        [Fact]
        public async void GetInfoTopicTest_Ok()
        {
            // Arrange
            int id = 1;
            var infotopicDTO = InfoTopicObject.TestInfoTopicDTO();
            var mockRepo = new Mock<IInfoTopicBLL>();
            mockRepo.Setup(repo => repo.GetInfoTopic(id)).ReturnsAsync(infotopicDTO);
            var controller = new InfoTopicsController(mockRepo.Object);

            // Act
            var result = await controller.GetInfoTopic(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<InfoTopicDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<InfoTopicDTO>(okResult.Value);
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void GetInfoTopicTest_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IInfoTopicBLL>();
            mockRepo.Setup(repo => repo.GetInfoTopic(id)).ReturnsAsync((InfoTopicDTO)null);
            var controller = new InfoTopicsController(mockRepo.Object);

            // Act
            var result = await controller.GetInfoTopic(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<InfoTopicDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Emne med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void GetInfoTopicTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IInfoTopicBLL>();
            mockRepo.Setup(repo => repo.GetInfoTopic(id)).ThrowsAsync(new InvalidOperationException());
            var controller = new InfoTopicsController(mockRepo.Object);

            // Act
            var result = await controller.GetInfoTopic(id);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved henting av emne", objectResult.Value);
        }

        [Fact]
        public async void AddInfoTopicTest_Ok()
        {
            // Arrange
            var infotopic = InfoTopicObject.TestInfoTopic();
            var infotopicDTO = InfoTopicObject.TestInfoTopicDTO();
            var mockRepo = new Mock<IInfoTopicBLL>();
            mockRepo.Setup(repo => repo.AddInfoTopic(infotopic)).ReturnsAsync(infotopicDTO).Verifiable();
            var controller = new InfoTopicsController(mockRepo.Object);

            // Act
            var result = await controller.AddInfoTopic(infotopic);

            // Assert
            var actionResult = Assert.IsType<ActionResult<InfoTopicDTO>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<InfoTopicDTO>(createdAtActionResult.Value);
            mockRepo.Verify();
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async void AddInfoTopicTest_BadRequest()
        {
            // Arrange
            var mockRepo = new Mock<IInfoTopicBLL>();
            var controller = new InfoTopicsController(mockRepo.Object);

            // Act
            var result = await controller.AddInfoTopic(null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<InfoTopicDTO>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Emne ble ikke opprettet", badRequestResult.Value);
        }

        [Fact]
        public async void AddInfoTopicTest_InternalServerError()
        {
            // Arrange
            var infotopic = InfoTopicObject.TestInfoTopic();
            var mockRepo = new Mock<IInfoTopicBLL>();
            mockRepo.Setup(repo => repo.AddInfoTopic(infotopic)).ThrowsAsync(new InvalidOperationException());
            var controller = new InfoTopicsController(mockRepo.Object);

            // Act
            var result = await controller.AddInfoTopic(infotopic);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved oppretting av nytt emne", objectResult.Value);
        }

        [Fact]
        public async void UpdateInfoTopicTest_Ok()
        {
            // Arrange
            int id = 1;
            var infotopic = InfoTopicObject.TestInfoTopic();
            var infotopicDTO = InfoTopicObject.TestInfoTopicDTO();
            var mockRepo = new Mock<IInfoTopicBLL>();
            mockRepo.Setup(repo => repo.UpdateInfoTopic(infotopic)).ReturnsAsync(infotopicDTO).Verifiable();
            var controller = new InfoTopicsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateInfoTopic(id, infotopic);

            // Assert
            var actionResult = Assert.IsType<ActionResult<InfoTopicDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<InfoTopicDTO>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void UpdateInfoTopicTest_NotFound()
        {
            // Arrange
            int id = 1;
            var infotopic = InfoTopicObject.TestInfoTopic();
            var mockRepo = new Mock<IInfoTopicBLL>();
            mockRepo.Setup(repo => repo.UpdateInfoTopic(infotopic)).ReturnsAsync((InfoTopicDTO)null);
            var controller = new InfoTopicsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateInfoTopic(id, infotopic);

            // Assert
            var actionResult = Assert.IsType<ActionResult<InfoTopicDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Emne med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void UpdateInfoTopicTest_BadRequest()
        {
            // Arrange
            int id = 0;
            var infotopic = InfoTopicObject.TestInfoTopic();
            var infotopicDTO = InfoTopicObject.TestInfoTopicDTO();
            var mockRepo = new Mock<IInfoTopicBLL>();
            mockRepo.Setup(repo => repo.UpdateInfoTopic(infotopic)).ReturnsAsync(infotopicDTO);
            var controller = new InfoTopicsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateInfoTopic(id, infotopic);

            // Assert
            var actionResult = Assert.IsType<ActionResult<InfoTopicDTO>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Emne ID stemmer ikke", badRequestResult.Value);
        }

        [Fact]
        public async void UpdateInfoTopicTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var infotopic = InfoTopicObject.TestInfoTopic();
            var mockRepo = new Mock<IInfoTopicBLL>();
            mockRepo.Setup(repo => repo.UpdateInfoTopic(infotopic)).ThrowsAsync(new InvalidOperationException());
            var controller = new InfoTopicsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateInfoTopic(id, infotopic);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved oppdatering av emne", objectResult.Value);
        }

        [Fact]
        public async void DeleteInfoTopicTest_Ok()
        {
            // Arrange
            int id = 1;
            var infotopicDTO = InfoTopicObject.TestInfoTopicDTO();
            var mockRepo = new Mock<IInfoTopicBLL>();
            mockRepo.Setup(repo => repo.DeleteInfoTopic(id)).ReturnsAsync(infotopicDTO).Verifiable();
            var controller = new InfoTopicsController(mockRepo.Object);

            // Act
            var result = await controller.DeleteInfoTopic(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<InfoTopicDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<InfoTopicDTO>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(id, returnValue.Id);
        }

        [Fact]
        public async void DeleteInfoTopicTest_NotFound()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IInfoTopicBLL>();
            mockRepo.Setup(repo => repo.DeleteInfoTopic(id)).ReturnsAsync((InfoTopicDTO)null);
            var controller = new InfoTopicsController(mockRepo.Object);

            // Act
            var result = await controller.DeleteInfoTopic(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<InfoTopicDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Emne med ID 1 ble ikke funnet", notFoundResult.Value);
        }

        [Fact]
        public async void DeleteInfoTopicTest_InternalServerError()
        {
            // Arrange
            int id = 1;
            var DTO = InfoTopicObject.TestInfoTopicDTO();
            var mockRepo = new Mock<IInfoTopicBLL>();
            mockRepo.Setup(repo => repo.DeleteInfoTopic(id)).ThrowsAsync(new InvalidOperationException());
            var controller = new InfoTopicsController(mockRepo.Object);

            // Act
            var result = await controller.DeleteInfoTopic(id);

            // Assert
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Feil ved sletting av emne", objectResult.Value);
        }
    }
}