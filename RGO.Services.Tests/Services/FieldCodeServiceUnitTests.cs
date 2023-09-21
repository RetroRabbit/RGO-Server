using Microsoft.Extensions.Options;
using Moq;
using RGO.Models;
using RGO.Models.Enums;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using Xunit;

namespace RGO.Tests.Services
{
    public class FieldCodeServiceUnitTests
    {
        private readonly Mock<IUnitOfWork> _dbMock;
        private readonly Mock<IFieldCodeOptionsService> _fieldCodeOptionsService;
        private readonly FieldCodeService _fieldCodeService;
        private readonly FieldCodeDto _fieldCodeDto;
        private readonly FieldCodeDto _fieldCodeDto2;
        private readonly FieldCodeDto _fieldCodeDto3;
        private readonly FieldCodeDto _fieldCodeDto4;
        private readonly FieldCodeOptionsDto _fieldCodeOptionsDto;
        private readonly FieldCodeOptionsDto _fieldCodeOptionsDto2;

        public FieldCodeServiceUnitTests()
        {
            _dbMock = new Mock<IUnitOfWork>();
            _fieldCodeOptionsService = new Mock<IFieldCodeOptionsService>();
            _fieldCodeDto = new FieldCodeDto(
                Id: 1,
                Code: "AAA000",
                Name: "string",
                Description: "string",
                Regex: "string",
                Type: FieldCodeType.String,
                Status: ItemStatus.Active,
                Internal: false,
                InternalTable: ""
                );
            _fieldCodeDto2 = new FieldCodeDto(
               Id: 2,
               Code: "AAA000",
               Name: "string2",
               Description: "string",
               Regex: "string",
               Type: FieldCodeType.String,
               Status: ItemStatus.Archive,
               Internal: false,
               InternalTable: ""
               );
            _fieldCodeDto3 = new FieldCodeDto(
               Id: 0,
               Code: "CCC222",
               Name: "string3",
               Description: "string",
               Regex: "string",
               Type: FieldCodeType.String,
               Status: ItemStatus.Active,
               Internal: false,
               InternalTable: ""
               );
            _fieldCodeDto4 = new FieldCodeDto(
                Id: 1,
                Code: "AAA000",
                Name: "string",
                Description: "string",
                Regex: "string",
                Type: FieldCodeType.String,
                Status: ItemStatus.Archive,
                Internal: false,
                InternalTable: ""
                );
            _fieldCodeOptionsDto = new FieldCodeOptionsDto(Id: 1, FieldCodeId: 1, Option: "string");
            _fieldCodeOptionsDto2 = new FieldCodeOptionsDto(Id: 2, FieldCodeId: 1, Option: "string");
            _fieldCodeService = new FieldCodeService(_dbMock.Object, _fieldCodeOptionsService.Object);
        }

        [Fact]
        public async Task GetAllFieldCodesTest()
        {
            List<FieldCodeDto> fields = new List<FieldCodeDto>() { _fieldCodeDto, _fieldCodeDto2 };
            List<FieldCodeDto> field = new List<FieldCodeDto>() { _fieldCodeDto };
            List<FieldCodeOptionsDto> options = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto };

            _fieldCodeOptionsService.Setup(x => x.GetFieldCodeOptions(It.IsAny<int>()))
                .Returns(Task.FromResult(options));

            _dbMock.Setup(x => x.FieldCode.GetAll(null)).Returns(Task.FromResult(fields));
            var result = await _fieldCodeService.GetAllFieldCodes();

            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.Equal(field, result);
        }

        [Fact]
        public async Task SaveFieldCodeTest()
        {
            List<FieldCodeDto> fields = new List<FieldCodeDto>() { _fieldCodeDto, _fieldCodeDto2 };
            List<FieldCodeOptionsDto> options = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto };
            _fieldCodeDto3.Options = options;

            _dbMock.Setup(x => x.FieldCode.GetAll(null)).Returns(Task.FromResult(fields));

            _dbMock.Setup(x => x.FieldCode.Add(It.IsAny<FieldCode>()))
                .Returns(Task.FromResult(_fieldCodeDto3));
            _fieldCodeOptionsService.Setup(x => x.SaveFieldCodeOptions(It.IsAny<FieldCodeOptionsDto>()))
                            .Returns(Task.FromResult(_fieldCodeOptionsDto));
            _fieldCodeOptionsService.Setup(x => x.GetFieldCodeOptions(It.IsAny<int>()))
                .Returns(Task.FromResult(options));

            var result = await _fieldCodeService.SaveFieldCode(_fieldCodeDto3);
           
            Assert.NotNull(result);
            Assert.Equal(_fieldCodeDto3, result);
            _dbMock.Verify(x => x.FieldCode.Add(It.IsAny<FieldCode>()), Times.Once);
        }

        [Fact]
        public async Task UpdateFieldCodeTest()
        {
            List<FieldCodeDto> fields = new List<FieldCodeDto>() { _fieldCodeDto, _fieldCodeDto2 };
            List<FieldCodeOptionsDto> options = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto, _fieldCodeOptionsDto2 };
            List<FieldCodeOptionsDto> optionsList2 = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto };
            _dbMock.Setup(x => x.FieldCode.Update(It.IsAny<FieldCode>()))
                .Returns(Task.FromResult(_fieldCodeDto));
            _dbMock.Setup(x => x.FieldCode.GetAll(null)).Returns(Task.FromResult(fields));
            _fieldCodeOptionsService.Setup(x => x.UpdateFieldCodeOptions(It.IsAny<List<FieldCodeOptionsDto>>()))
                .Returns(Task.FromResult(optionsList2));
            _fieldCodeOptionsService.Setup(x => x.GetAllFieldCodeOptions()).Returns(Task.FromResult(options));
            _fieldCodeOptionsService.Setup(x => x.GetFieldCodeOptions(It.IsAny<int>()))
                .Returns(Task.FromResult(options));
            _dbMock.Setup(x => x.FieldCodeOptions.Delete(It.IsAny<int>())).Returns(Task.FromResult(_fieldCodeOptionsDto2));
            _fieldCodeDto.Options = options;
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
            
            _dbMock.Setup(x => x.FieldCode.Update(It.IsAny<FieldCode>()))
                .Returns(Task.FromResult(_fieldCodeDto4));

            var result = await _fieldCodeService.DeleteFieldCode(_fieldCodeDto);
            Assert.NotNull(result);
            Assert.Equal(_fieldCodeDto4, result);
            _dbMock.Verify(r => r.FieldCode.Update(It.IsAny<FieldCode>()), Times.Once);
        }


    }
}
