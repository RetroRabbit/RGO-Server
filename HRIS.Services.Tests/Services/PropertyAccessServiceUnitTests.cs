using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RGO.Tests.Services;

public class PropertyAccessServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IPropertyAccessService> _propertyAccessService;

    public PropertyAccessServiceUnitTests()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _propertyAccessService = new Mock<IPropertyAccessService>();
        propertyAccessService = new PropertyAccessService(_dbMock.Object);

    }

    public readonly PropertyAccessService propertyAccessService;

    [Fact]
    public async Task GetAllTest()
    {
        _dbMock.Setup(p => p.PropertyAccess.Get(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
               .Returns(PropertyAccessTestData.PropertyAccessList.ToMockIQueryable());

        var result = await propertyAccessService.GetAll();

        Assert.NotNull(result);
        Assert.IsType<List<PropertyAccessDto>>(result);
        Assert.True(result.SequenceEqual(result.OrderBy(e => e.Id)));
        _dbMock.Verify(p => p.PropertyAccess.Get(It.IsAny<Expression<Func<PropertyAccess, bool>>>()), Times.Once);
    }

    [Fact]
    public void GetAccessListByEmployeeIdTest()
    {
        _dbMock.Setup(e => e.PropertyAccess.Get(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
               .Returns(PropertyAccessTestData.PropertyAccessList.ToMockIQueryable());

        var result = propertyAccessService.GetAccessListByEmployeeId(1);

        Assert.NotNull(result);
    }
    [Fact]
    public void GetAccessListByRoleIdTest()
    {
        _dbMock.Setup(e => e.PropertyAccess.Get(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
               .Returns(PropertyAccessTestData.PropertyAccessList.ToMockIQueryable());


        var result = propertyAccessService.GetAccessListByRoleId(1);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdatePropertyAccess()
    {
        _propertyAccessService.Setup(r => r.UpdatePropertyAccess(PropertyAccessTestData.PropertyAccessOne.Id, PropertyAccessLevel.read));
                              
        _dbMock.Setup(r => r.PropertyAccess.Update(It.IsAny<PropertyAccess>()))
               .ReturnsAsync(PropertyAccessTestData.PropertyAccessOne);

            await propertyAccessService.UpdatePropertyAccess(PropertyAccessTestData.PropertyAccessOne.Id, PropertyAccessLevel.read);
    }
}