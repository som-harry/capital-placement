using CapitalPlacement.Application.DTOS;
using CapitalPlacement.Application.Services;
using CapitalPlacement.Domain.Entities;
using CapitalReplacement.Application.Features.Constants;
using CapitalReplacement.Application.Interfaces.Persistence;
using Mapster;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CapitalPlacement.Test
{
    public class ProgrammerServiceTest
    {
        private readonly Mock<ILogger<ProgrammeService>> _loggerMock;
        private readonly Mock<IAsyncRepository<Programme>> _asyncRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ProgrammeService _service;

        public ProgrammerServiceTest()
        {
            _loggerMock = new Mock<ILogger<ProgrammeService>>();
            _asyncRepositoryMock = new Mock<IAsyncRepository<Programme>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _service = new ProgrammeService(_loggerMock.Object,_asyncRepositoryMock.Object,_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task ShouldNotCreateProgramme_IfThereIsAnExistingProgram_ReturnsDuplicateResource()    
        {
            var programmeDto = new ProgrammeDto { ProgramTitle = "ExistingProgram" };
            var existingProgram = new Programme { Id = Guid.NewGuid(), ProgramTitle = "ExistingProgram" };

            _asyncRepositoryMock.Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Programme, bool>>>()))
                .ReturnsAsync(existingProgram);

            _unitOfWorkMock.Setup(unitOfWork => unitOfWork.CommitChangesAsync()).ReturnsAsync(0); 

            var loggerMock = new Mock<ILogger<ProgrammeService>>();

            var result = await _service.CreateProgramme(programmeDto);

            Assert.Equal(ResponseCodes.DUPLICATE_RESOURCE, result.ResponseCode);
            Assert.Equal("Program with title already exists, please choose a different program title", result.Message);
        }
        [Fact]
        public async Task ShouldNotCreateProgramme_IfNewProgramCannotBeSave_ReturnsServerError()
        {
            var programmeDto = new ProgrammeDto { ProgramTitle = "NewProgram" };

            _asyncRepositoryMock.Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Programme, bool>>>()))
                .ReturnsAsync((Programme)null);

            _unitOfWorkMock.Setup(unitOfWork => unitOfWork.CommitChangesAsync()).ReturnsAsync(0); 

            var result = await _service.CreateProgramme(programmeDto);

            Assert.Equal(ResponseCodes.SERVER_ERROR, result.ResponseCode);
            Assert.Equal("Failed to save program response", result.Message);
        }
        [Fact]
        public async Task UpdateProgramme_ProgramDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var programmeDto = new UpdatedProgrammmeDto();
            var mappedRequest = programmeDto.Adapt<Programme>();

            _asyncRepositoryMock.Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Programme, bool>>>()))
              .ReturnsAsync((Programme)null);

            // Act
            var result = await _service.UpdateProgramme(programmeDto);

            // Assert
            _asyncRepositoryMock.Verify(repo => repo.Update(It.IsAny<Programme>()), Times.Never); // Verify that Update method is never called
            Assert.Equal(ResponseCodes.NOT_FOUND, result.ResponseCode);
            Assert.Equal("No record was found in the database", result.Message);
        }
        [Fact]
        public async Task DeleteProgramme_ProgramDoesNotExist_ReturnsNotFound()
        {
            var programmeId = new Guid();

            _asyncRepositoryMock.Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<Expression<Func<Programme, bool>>>()))
              .ReturnsAsync((Programme)null);

            var result = await _service.DeleteProgramme(programmeId);

            _asyncRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Programme>()), Times.Never); 
            Assert.Equal(ResponseCodes.NOT_FOUND, result.ResponseCode);
            Assert.Equal("No record was found in the database", result.Message);
        }
    }
}
