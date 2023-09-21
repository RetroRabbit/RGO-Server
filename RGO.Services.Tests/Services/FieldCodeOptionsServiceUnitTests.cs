using Moq;
using RGO.Models;
using RGO.Services.Services;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.Linq.Expressions;
using Xunit;
using static Npgsql.PostgresTypes.PostgresCompositeType;

namespace RGO.Tests.Services
{
    public class FieldCodeOptionsServiceUnitTests
    {
        private readonly Mock<IUnitOfWork> _dbMock;
        private readonly FieldCodeOptionsService _fieldCodeOptionsService;
        private readonly FieldCodeOptionsDto _fieldCodeOptionsDto;
        private readonly FieldCodeOptionsDto _fieldCodeOptionsDto2;

        public FieldCodeOptionsServiceUnitTests()
        {
            _dbMock = new Mock<IUnitOfWork>();
            _fieldCodeOptionsService = new FieldCodeOptionsService(_dbMock.Object);
            _fieldCodeOptionsDto = new FieldCodeOptionsDto(Id: 1, FieldCodeId: 1  , Option: "string");
            _fieldCodeOptionsDto2 = new FieldCodeOptionsDto(Id: 0, FieldCodeId: 1, Option: "string2");
        }

        [Fact]
        public async Task GetAllFieldCodeOptionsTest()
        {
            List<FieldCodeOptionsDto> fields = new List<FieldCodeOptionsDto>() { _fieldCodeOptionsDto };

            _dbMock.Setup(x => x.FieldCodeOptions.GetAll(null)).Returns(Task.FromResult(fields));
            var result = await _fieldCodeOptionsService.GetAllFieldCodeOptions();

            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.Equal(fields, result);
        }

        [Fact]
        public async Task GetFieldCodeOptionsTest()
        {
            List<FieldCodeOptionsDto> fields = new List<FieldCodeOptionsDto>() { _fieldCodeOptionsDto };
            _dbMock.Setup(x => x.FieldCodeOptions.GetAll(null)).Returns(Task.FromResult(fields));

            var result = await _fieldCodeOptionsService.GetFieldCodeOptions(_fieldCodeOptionsDto.Id);
            Assert.NotNull(result);
            Assert.Equal(fields, result);
            _dbMock.Verify(x => x.FieldCodeOptions.GetAll(null), Times.Once);
        }

        [Fact]
        public async Task SaveFieldCodeOptionsTest()
        {
            List<FieldCodeOptionsDto> fields = new List<FieldCodeOptionsDto>() { _fieldCodeOptionsDto };
            _dbMock.Setup(x => x.FieldCodeOptions.GetAll(null)).Returns(Task.FromResult(fields));

            _dbMock.Setup(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()))
                .Returns(Task.FromResult(_fieldCodeOptionsDto2));

            var result = await _fieldCodeOptionsService.SaveFieldCodeOptions(_fieldCodeOptionsDto2);
            Assert.NotNull(result);
            Assert.Equal(_fieldCodeOptionsDto2, result);
            _dbMock.Verify(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()), Times.Once);
        }


        [Fact]
        public async Task UpdateFieldCodeOptionsTest()
        {
            List<FieldCodeOptionsDto> fieldList = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto, _fieldCodeOptionsDto2 };
            List<FieldCodeOptionsDto> field = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto};
            List<FieldCodeOptionsDto> field2 = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto2 };

            _dbMock.SetupSequence(x => x.FieldCodeOptions.GetAll(null))
                .ReturnsAsync(field)
                .ReturnsAsync(fieldList)
                .ReturnsAsync(field2)
                .ReturnsAsync(field2);


            _dbMock.Setup(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>())).Returns(Task.FromResult(_fieldCodeOptionsDto2));
            _dbMock.Setup(x => x.FieldCodeOptions.Delete(It.IsAny<int>()))
               .Returns(Task.FromResult(_fieldCodeOptionsDto));

            var result = await _fieldCodeOptionsService.UpdateFieldCodeOptions(field2);

            _dbMock.Verify(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()), Times.Once);
            _dbMock.Verify(x => x.FieldCodeOptions.Delete(It.IsAny<int>()), Times.Once);
        }


        [Fact]
        public async Task DeleteFieldCode()
        {
            List<FieldCodeOptionsDto> fields = new List<FieldCodeOptionsDto>() { _fieldCodeOptionsDto };
            _dbMock.Setup(x => x.FieldCodeOptions.GetAll(null)).Returns(Task.FromResult(fields));

            _dbMock.Setup(x => x.FieldCodeOptions.Delete(It.IsAny<int>()))
                .Returns(Task.FromResult(_fieldCodeOptionsDto));

            var result = await _fieldCodeOptionsService.DeleteFieldCodeOptions(_fieldCodeOptionsDto);
            Assert.NotNull(result);
            Assert.Equal(_fieldCodeOptionsDto, result);
            _dbMock.Verify(r => r.FieldCodeOptions.Delete(It.IsAny<int>()), Times.Once);
        }

    }
}
