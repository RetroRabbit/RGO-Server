using Moq;
using RR.UnitOfWork;
using HRIS.Services.Helpers;
using System.Net.Mail;
using Xunit;
using RR.UnitOfWork.Entities.Shared;
using System.Linq.Expressions;

namespace HRIS.Services.Tests.Helpers;

public class EmailHelperUnitTests
{
    private readonly Mock<IUnitOfWork> _db;
    private readonly EmailHelper _helper;

    public EmailHelperUnitTests()
    {
        _db = new Mock<IUnitOfWork>();
        _helper = new EmailHelper(_db.Object);
    }

    [Fact(Skip="Needs to be fixed/updated")]
    public void CompileMessageTest()
    {
        var mailAddress = new MailAddress("test@domain.com", "Jane Doe");
        var emailTemplate = new EmailTemplate
        {
            Id = 1,
            Name = "TestTemplate",
            Subject = "Attention to {{Name}}",
            Body = "This is a dynamic body. {{Date}}",
        };

        var message = _helper.CompileMessage(mailAddress, emailTemplate, new { Name = "Jane", Date = "2027/07/02"});

        Assert.Equal("\"Jane Doe\" <test@domain.com>", message.To.ToString());
        Assert.True(message.IsBodyHtml);
        Assert.Equal("Attention to Jane", message.Subject);
        Assert.Equal("This is a dynamic body. 2027/07/02", message.Body);
    }

    [Fact]
    public void CompileStringTest()
    {
        var message = _helper.CompileString("This is a dynamic body. {{Date}}", new { Date = "2027/07/02" });
        Assert.Equal("This is a dynamic body. 2027/07/02", message);
    }

    [Fact]

    public async Task GetTemplateSuccess()
    {
        _db.Setup(x => x.EmailTemplate.FirstOrDefault(It.IsAny<Expression<Func<EmailTemplate, bool>>>())).ReturnsAsync(new EmailTemplate());

        var template = await _helper.GetTemplate(It.IsAny<string>());

        Assert.NotNull(template);
        _db.Verify(x => x.EmailTemplate.FirstOrDefault(It.IsAny<Expression<Func<EmailTemplate, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetTemplateFail()
    {
        _db.Setup(x => x.EmailTemplate.FirstOrDefault(It.IsAny<Expression<Func<EmailTemplate, bool>>>())).ReturnsAsync((EmailTemplate)null);

        var exception = await Assert.ThrowsAnyAsync<Exception>(() => _helper.GetTemplate("TestTemplate"));

        _db.Verify(x => x.EmailTemplate.FirstOrDefault(It.IsAny<Expression<Func<EmailTemplate, bool>>>()), Times.Once);
        Assert.Equal($"Email template 'TestTemplate' does not exist", exception.Message);
    }
}