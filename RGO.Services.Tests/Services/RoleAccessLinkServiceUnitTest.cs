using Moq;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.Linq.Expressions;
using Xunit;

namespace RGO.Services.Tests.Services;

public class RoleAccessLinkServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeRoleService> _employeeRoleServiceMock;
    private readonly RoleAccessLinkService _roleAccessLinkService;
    private readonly RoleDto _roleDto;
    private readonly RoleAccessDto _roleAccessDto;
    private readonly RoleAccessLinkDto _roleAccessLinkDto;

    public RoleAccessLinkServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
        _roleAccessLinkService = new RoleAccessLinkService(_dbMock.Object, _employeeRoleServiceMock.Object);
        _roleDto = new RoleDto(1, "Employee");
        _roleAccessDto = new RoleAccessDto(1, "ViewEmployee");
        _roleAccessLinkDto = new RoleAccessLinkDto(1, _roleDto, _roleAccessDto);
    }

    [Fact]
    public async Task SaveTest()
    {
        _dbMock
            .Setup(r => r.RoleAccessLink.Add(It.IsAny<RoleAccessLink>()))
            .Returns(Task.FromResult(_roleAccessLinkDto));

        var result = await _roleAccessLinkService.Save(_roleAccessLinkDto);

        Assert.NotNull(result);
        Assert.Equal(_roleAccessLinkDto, result);
        _dbMock.Verify(r => r.RoleAccessLink.Add(It.IsAny<RoleAccessLink>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTest()
    {
        List<RoleAccessLinkDto> roleAccessLinks = new List<RoleAccessLinkDto>() { _roleAccessLinkDto };

        _dbMock
            .Setup(r => r.RoleAccessLink.GetAll(null))
            .Returns(Task.FromResult(roleAccessLinks));

        _dbMock
            .Setup(r => r.RoleAccessLink.Delete(It.IsAny<int>()))
            .Returns(Task.FromResult(_roleAccessLinkDto));

        var result = await _roleAccessLinkService.Delete(_roleDto.Description, _roleAccessDto.Permission);

        Assert.NotNull(result);
        Assert.Equal(roleAccessLinks.First().Id, result.Id);
        _dbMock.Verify(r => r.RoleAccessLink.GetAll(null), Times.Once);
    }

    [Fact]
    public async Task GetAllTest()
    {
        var roleAccessLinks = new List<RoleAccessLinkDto>() { _roleAccessLinkDto };
        var mergedRoleAccessLinks = roleAccessLinks
            .GroupBy(r => r.Role.Description)
            .ToDictionary(
                group => group.Key,
                group => group.Select(link => link.RoleAccess.Permission).ToList());

        _dbMock
            .Setup(r => r.RoleAccessLink.GetAll(null))
            .Returns(Task.FromResult(roleAccessLinks));

        var result = await _roleAccessLinkService.GetAll();

        Assert.NotNull(result);
        Assert.Equal(mergedRoleAccessLinks.Keys, result.Keys);
        Assert.Equal(mergedRoleAccessLinks.Values, result.Values);
        _dbMock.Verify(r => r.RoleAccessLink.GetAll(null), Times.Once);
    }

    [Fact]
    public async Task GetByRoleTest()
    {
        var roleAccessLinks = new List<RoleAccessLinkDto>() { _roleAccessLinkDto };
        var mergedRoleAccessLinks = roleAccessLinks
            .GroupBy(r => r.Role.Description)
            .ToDictionary(
                group => group.Key,
                group => group.Select(link => link.RoleAccess.Permission).ToList());

        _dbMock
            .Setup(r => r.RoleAccessLink.GetAll(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
            .Returns(Task.FromResult(roleAccessLinks));

        var result = await _roleAccessLinkService.GetByRole(_roleDto.Description);

        Assert.NotNull(result);
        Assert.Equal(mergedRoleAccessLinks.Keys, result.Keys);
        Assert.Equal(mergedRoleAccessLinks.Values, result.Values);
        _dbMock.Verify(r => r.RoleAccessLink.GetAll(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetRoleByEmployeeTest()
    {
        string email = "test@email.com";
        List<string> employeeRoles = new List<string>() { "Employee", "Admin" };
        List<EmployeeRoleDto> employeeRoleDtos = employeeRoles
            .Select(role => new EmployeeRoleDto(1, null, new RoleDto(1, role)))
            .ToList();

        Dictionary<string, List<string>> roleAccessLinkByRole1 = new Dictionary<string, List<string>>() {
        { "Employee", new List<string>() { "ViewEmployee", "EditEmployee" } }};

        Dictionary<string, List<string>> roleAccessLinkByRole2 = new Dictionary<string, List<string>>() {
        { "Admin", new List<string>() { "ViewAdmin", "EditAdmin" } }};

        _employeeRoleServiceMock
            .Setup(e => e.GetEmployeeRoles(email))
            .ReturnsAsync(employeeRoleDtos);

        _dbMock
            .Setup(r => r.RoleAccessLink.GetAll(r => r.Role.Description == "Employee"))
            .ReturnsAsync(new List<RoleAccessLinkDto> {
                new RoleAccessLinkDto(1, new RoleDto(1, "Employee"), new RoleAccessDto(1, "ViewEmployee")),
                new RoleAccessLinkDto(2, new RoleDto(1, "Employee"), new RoleAccessDto(2, "EditEmployee"))});
        _dbMock
            .Setup(r => r.RoleAccessLink.GetAll(r => r.Role.Description == "Admin"))
            .ReturnsAsync(new List<RoleAccessLinkDto> {
                new RoleAccessLinkDto(3, new RoleDto(2, "Admin"), new RoleAccessDto(3, "ViewAdmin")),
                new RoleAccessLinkDto(4, new RoleDto(2, "Admin"), new RoleAccessDto(4, "EditAdmin"))});

        var result = await _roleAccessLinkService.GetRoleByEmployee(email);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(roleAccessLinkByRole1["Employee"], result["Employee"]);
        Assert.Equal(roleAccessLinkByRole2["Admin"], result["Admin"]);
    }

    [Fact]
    public async Task Update()
    {
        var roleAccessLinkToUpdate = new RoleAccessLinkDto(
            _roleAccessLinkDto.Id,
            new RoleDto(1, "Employee"),
            new RoleAccessDto(2, "EditEmployee"));

        _dbMock
            .Setup(r => r.RoleAccessLink.Update(It.IsAny<RoleAccessLink>()))
            .Returns(Task.FromResult(_roleAccessLinkDto));

        var result = await _roleAccessLinkService.Update(roleAccessLinkToUpdate);

        Assert.NotNull(result);
        Assert.NotNull(result.Role);
        Assert.NotNull(result.RoleAccess);
        Assert.Equal(_roleAccessLinkDto, result);
        _dbMock.Verify(r => r.RoleAccessLink.Update(It.IsAny<RoleAccessLink>()), Times.Once);
    }
}