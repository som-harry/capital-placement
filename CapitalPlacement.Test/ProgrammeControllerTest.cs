using CapitalPlacement.Application.DTOS;
using CapitalPlacement.Application.Interfaces.Application;
using CapitalReplacement.Api.Controllers;
using CapitalReplacement.Application.Features.Constants;
using CapitalReplacement.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CapitalPlacement.Test
{
    
    public class ProgrammeControllerTest
    {
        private readonly Mock<ILogger<ProgrammeController>> _loggerMock;
        private readonly Mock<IProgrammeService> _programmeServiceMock;
        private readonly ProgrammeController _controller;

        public ProgrammeControllerTest()
        {
            _loggerMock = new Mock<ILogger<ProgrammeController>>();
            _programmeServiceMock = new Mock<IProgrammeService>();
            _controller = new ProgrammeController(_loggerMock.Object, _programmeServiceMock.Object);
        }
        [Fact]
        public async Task SubmitProgramme_ValidInput_ReturnsOk()
        {
            // Arrange
            var programmeDto = new ProgrammeDto(); 
            var response = new BaseResponse<Guid> { ResponseCode = ResponseCodes.CREATED };
            _programmeServiceMock.Setup(service => service.CreateProgramme(programmeDto)).ReturnsAsync(response);

            // Act
            var result = await _controller.SubmitProgramme(programmeDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResponse = Assert.IsType<BaseResponse<Guid>>(okResult.Value);
            Assert.Equal(ResponseCodes.CREATED, actualResponse.ResponseCode);
        }
        [Fact]
        public async Task SubmitProgramme_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Sample error message");
            var programmeDto = new ProgrammeDto(); // Provide invalid input

            // Act
            var result = await _controller.SubmitProgramme(programmeDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task SubmitProgramme_serviceEncouterError_ReturnsInternalServerError()
        {
            // Arrange
            var response = new BaseResponse<Guid> { ResponseCode = ResponseCodes.SERVER_ERROR };
            var programmeDto = new ProgrammeDto();
            _programmeServiceMock.Setup(service => service.CreateProgramme(programmeDto)).ReturnsAsync(response);

            // Act
            var result = await _controller.SubmitProgramme(programmeDto);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            Assert.Equal(500, objectResult.StatusCode);
            
        }

        [Fact]
        public async Task GetAllProgrammes_Success_ReturnsOk()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 50;
            var returnResponse = new List<UpdatedProgrammmeDto>();
            var response = new BaseResponse<IEnumerable<UpdatedProgrammmeDto>>
            {

                Data = returnResponse,
                ResponseCode = ResponseCodes.SUCCESS
            };
            _programmeServiceMock.Setup(service => service.GetAllProgrammes(pageNumber, pageSize)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAllProgrammes(pageNumber, pageSize);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResponse = Assert.IsType<BaseResponse<IEnumerable<UpdatedProgrammmeDto>>>(okResult.Value);
            Assert.Equal(ResponseCodes.SUCCESS, actualResponse.ResponseCode);
        }

        [Fact]
        public async Task GetAllProgrammes_NotFound_ReturnsNotFound()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 50;
            var response = new BaseResponse<IEnumerable<UpdatedProgrammmeDto>>
            {
                ResponseCode = ResponseCodes.NOT_FOUND,
                Message = "Programmes not found"
            };
            _programmeServiceMock.Setup(service => service.GetAllProgrammes(pageNumber, pageSize)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAllProgrammes(pageNumber, pageSize);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAllProgrammes_InternalServerError_ReturnsInternalServerError()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 50;
            var response = new BaseResponse<IEnumerable<UpdatedProgrammmeDto>>
            {
                ResponseCode = ResponseCodes.SERVER_ERROR,
                Message = "Internal server error occurred"
            };
            _programmeServiceMock.Setup(service => service.GetAllProgrammes(pageNumber, pageSize)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAllProgrammes(pageNumber, pageSize);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            Assert.Equal(500, objectResult.StatusCode); 
        }

    }
}
