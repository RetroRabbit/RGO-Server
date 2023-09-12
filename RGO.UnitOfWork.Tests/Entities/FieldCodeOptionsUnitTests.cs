using Moq;
using RGO.Models;
using RGO.Models.Enums;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using RGO.UnitOfWork.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Npgsql.PostgresTypes.PostgresCompositeType;

namespace RGO.UnitOfWork.Tests.Entities
{
    public class FieldCodeOptionsUnitTests
    {
        private readonly Mock<IUnitOfWork> _dbMock;
        private readonly FieldCodeOptionsService _fieldCodeOptionsService;
        private readonly FieldCodeOptionsDto _fieldCodeOptionsDto;
        private readonly FieldCodeOptionsDto _fieldCodeOptionsDto2;

        public FieldCodeOptionsUnitTests()
        {
            _dbMock = new Mock<IUnitOfWork>();
            _fieldCodeOptionsService = new FieldCodeOptionsService(_dbMock.Object);
            _fieldCodeOptionsDto = new FieldCodeOptionsDto(Id:0, FieldCodeId:0  , Option: "string");
            _fieldCodeOptionsDto2 = new FieldCodeOptionsDto(Id: 0, FieldCodeId: 0, Option: "string2");
        }

        [Fact]
        public async Task FieldCodeOptionsTest()
        {
            var fieldCodeOptions = new FieldCodeOptions();
            Assert.IsType<FieldCodeOptions>(fieldCodeOptions);
            Assert.NotNull(fieldCodeOptions);
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
                .Returns(Task.FromResult(_fieldCodeOptionsDto));

            var result = await _fieldCodeOptionsService.SaveFieldCodeOptions(_fieldCodeOptionsDto2);
            Assert.NotNull(result);
            Assert.Equal(_fieldCodeOptionsDto, result);
            _dbMock.Verify(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()), Times.Once);
        }


        [Fact]
        public async Task UpdateFieldCodeOptionsTest()
        {
            List<FieldCodeOptionsDto> field = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto, _fieldCodeOptionsDto2 };

            _dbMock.Setup(x => x.FieldCodeOptions.GetAll(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()))
                   .ReturnsAsync(new List<FieldCodeOptionsDto>());

            _dbMock.Setup(x => x.FieldCodeOptions.Delete(It.IsAny<int>()))
               .Returns(Task.FromResult(_fieldCodeOptionsDto));

            var result = await _fieldCodeOptionsService.UpdateFieldCodeOptions(field);
            int expectedNumberOfDeletes = result.Count(option =>
            {
                return !field.Any(dto =>
                    dto.FieldCodeId == option.FieldCodeId && dto.Option.ToLower() == option.Option.ToLower());
            });

            _dbMock.Verify(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()), Times.Exactly(field.Count));
            _dbMock.Verify(x => x.FieldCodeOptions.Delete(It.IsAny<int>()), Times.Exactly(expectedNumberOfDeletes));
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
