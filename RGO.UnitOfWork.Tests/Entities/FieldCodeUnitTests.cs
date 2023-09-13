using Moq;
using RGO.Models;
using RGO.Models.Enums;
using RGO.Services.Services;
using RGO.UnitOfWork.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities
{
    public class FieldCodeUnitTests
    {
        //TODO: Update
        private readonly Mock<IUnitOfWork> _dbMock;
        private readonly FieldCodeService _fieldCodeService;
        private readonly FieldCodeOptionsService _fieldCodeOptionsService;
        private readonly FieldCodeDto _fieldCodeDto;
        private readonly FieldCodeDto _fieldCodeDto2;

        public FieldCodeUnitTests()
        {
            _dbMock = new Mock<IUnitOfWork>();
            _fieldCodeService = new FieldCodeService(_dbMock.Object, _fieldCodeOptionsService);
            _fieldCodeDto = new FieldCodeDto(
                Id: 0,
                Code: "AAA000",
                Name: "string",
                Description: "string",
                Regex: "string",
                Type: FieldCodeType.String,
                Status: 0,
                Internal: false,
                InternalTable: ""
                );
            _fieldCodeDto2 = new FieldCodeDto(
               Id: 0,
               Code: "AAA000",
               Name: "string2",
               Description: "string",
               Regex: "string",
               Type: FieldCodeType.String,
               Status: 0,
               Internal: false,
               InternalTable: ""
               );
        }

        [Fact]
        public async Task FieldCodeTest()
        {
            var fieldCode = new FieldCode();
            Assert.IsType<FieldCode>(fieldCode);
            Assert.NotNull(fieldCode);
        }

        [Fact]
        public async Task GetAllFieldCodesTest()
        {
            List<FieldCodeDto> fields = new List<FieldCodeDto>() { _fieldCodeDto};
  
            _dbMock.Setup(x => x.FieldCode.GetAll(null)).Returns(Task.FromResult(fields));
            var result = await _fieldCodeService.GetAllFieldCodes();

            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.Equal(fields, result);
        }

        [Fact]
        public async Task GetFieldCodeTest()
        {
            List<FieldCodeDto> fields = new List<FieldCodeDto>() { _fieldCodeDto };
            _dbMock.Setup(x => x.FieldCode.GetAll(null)).Returns(Task.FromResult(fields));

            var result = await _fieldCodeService.GetFieldCode(_fieldCodeDto.Name);
            Assert.NotNull(result);
            Assert.Equal(_fieldCodeDto, result);
            _dbMock.Verify(x => x.FieldCode.GetAll(null), Times.Once);
        }

        [Fact]
        public async Task SaveFieldCodeTest()
        {
            List<FieldCodeDto> fields = new List<FieldCodeDto>() { _fieldCodeDto };
            _dbMock.Setup(x => x.FieldCode.GetAll(null)).Returns(Task.FromResult(fields));

            _dbMock.Setup(x => x.FieldCode.Add(It.IsAny<FieldCode>()))
                .Returns(Task.FromResult(_fieldCodeDto));

            var result = await _fieldCodeService.SaveFieldCode(_fieldCodeDto2);
            Assert.NotNull(result);
            Assert.Equal(_fieldCodeDto, result);
            _dbMock.Verify(x => x.FieldCode.Add(It.IsAny<FieldCode>()), Times.Once);
        }

        [Fact]
        public async Task UpdateFieldCodeTest()
        {
            _dbMock.Setup(x => x.FieldCode.Update(It.IsAny<FieldCode>()))
                .Returns(Task.FromResult(_fieldCodeDto));

            var result = await _fieldCodeService.UpdateFieldCode(_fieldCodeDto);
            Assert.NotNull(result);
            Assert.Equal(_fieldCodeDto, result);
            _dbMock.Verify(x => x.FieldCode.Update(It.IsAny<FieldCode>()), Times.Once);
        }

        [Fact]
        public async Task DeleteFieldCodeTest()
        {
            List<FieldCodeDto> fields = new List<FieldCodeDto>() { _fieldCodeDto };
            _dbMock.Setup(x => x.FieldCode.GetAll(null)).Returns(Task.FromResult(fields));

            _dbMock.Setup(x => x.FieldCode.Delete(It.IsAny<int>()))
                .Returns(Task.FromResult(_fieldCodeDto));

            var result = await _fieldCodeService.DeleteFieldCode(_fieldCodeDto);
            Assert.NotNull(result);
            Assert.Equal(_fieldCodeDto, result);
            _dbMock.Verify(r => r.FieldCode.Delete(It.IsAny<int>()), Times.Once);
        }
    }
}
