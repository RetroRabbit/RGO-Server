using System.Net.Mail;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Interfaces.Helper;
using HRIS.Services.Services;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.Shared;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class EmailServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _db;
    private readonly Mock<IErrorLoggingService> _logger;
    private readonly Mock<IEmailHelper> _helper;
    private readonly EmailService _service;

    public EmailServiceUnitTests()
    {
        _db = new Mock<IUnitOfWork>();
        _logger = new Mock<IErrorLoggingService>();
        _helper = new Mock<IEmailHelper>();
        _service = new EmailService(_db.Object, _logger.Object, _helper.Object);
    }

    [Fact]
    public async Task SendEmailWithSuccess()
    {
        _helper.Setup(x => x.GetTemplate(It.IsAny<string>())).ReturnsAsync(new EmailTemplate());
        _helper.Setup(x => x.CompileMessage(It.IsAny<MailAddress>(), It.IsAny<EmailTemplate>(), It.IsAny<object>())).Returns(new MailMessage());
        _helper.Setup(x => x.SendMailAsync(It.IsAny<MailMessage>()));
        _db.Setup(x => x.EmailHistory.Add(It.IsAny<EmailHistory>())).ReturnsAsync(new EmailHistory());
        _db.Setup(x => x.EmailHistory.Update(It.IsAny<EmailHistory>())).ReturnsAsync(new EmailHistory());

        await _service.Send(new MailAddress("test@domain.com", "Jane Doe"), "testTemplate", new { name = "Jane" });

        _helper.Verify(x => x.GetTemplate(It.IsAny<string>()), Times.Once);
        _helper.Verify(x => x.CompileMessage(It.IsAny<MailAddress>(), It.IsAny<EmailTemplate>(), It.IsAny<object>()), Times.Once);
        _helper.Verify(x => x.SendMailAsync(It.IsAny<MailMessage>()), Times.Once);
        _db.Verify(x => x.EmailHistory.Add(It.IsAny<EmailHistory>()), Times.Once);
        _db.Verify(x => x.EmailHistory.Update(It.IsAny<EmailHistory>()), Times.Once);
        _logger.Verify(x => x.LogException(It.IsAny<SmtpException>()), Times.Never);
    }

    [Fact]
    public async Task SendEmailWithSuccess_EmployeeObject()
    {
        _helper.Setup(x => x.GetTemplate(It.IsAny<string>())).ReturnsAsync(new EmailTemplate());
        _helper.Setup(x => x.CompileMessage(It.IsAny<MailAddress>(), It.IsAny<EmailTemplate>(), It.IsAny<object>())).Returns(new MailMessage());
        _helper.Setup(x => x.SendMailAsync(It.IsAny<MailMessage>()));
        _db.Setup(x => x.EmailHistory.Add(It.IsAny<EmailHistory>())).ReturnsAsync(new EmailHistory());
        _db.Setup(x => x.EmailHistory.Update(It.IsAny<EmailHistory>())).ReturnsAsync(new EmailHistory());

        await _service.Send(new EmployeeDto { Email = "test@domain.com", Name = "Jane", Surname = "Doe"}, "testTemplate");

        _helper.Verify(x => x.GetTemplate(It.IsAny<string>()), Times.Once);
        _helper.Verify(x => x.CompileMessage(It.IsAny<MailAddress>(), It.IsAny<EmailTemplate>(), It.IsAny<object>()), Times.Once);
        _helper.Verify(x => x.SendMailAsync(It.IsAny<MailMessage>()), Times.Once);
        _db.Verify(x => x.EmailHistory.Add(It.IsAny<EmailHistory>()), Times.Once);
        _db.Verify(x => x.EmailHistory.Update(It.IsAny<EmailHistory>()), Times.Once);
        _logger.Verify(x => x.LogException(It.IsAny<SmtpException>()), Times.Never);
    }

    [Fact]
    public async Task SendEmailWithFail()
    {
        _helper.Setup(x => x.GetTemplate(It.IsAny<string>())).ReturnsAsync(new EmailTemplate());
        _helper.Setup(x => x.CompileMessage(It.IsAny<MailAddress>(), It.IsAny<EmailTemplate>(), It.IsAny<object>())).Returns(new MailMessage());
        _helper.Setup(x => x.SendMailAsync(It.IsAny<MailMessage>())).Throws<SmtpException>();
        _db.Setup(x => x.EmailHistory.Add(It.IsAny<EmailHistory>())).ReturnsAsync(new EmailHistory());
        _db.Setup(x => x.EmailHistory.Update(It.IsAny<EmailHistory>())).ReturnsAsync(new EmailHistory());
        _logger.Setup(x => x.LogException(It.IsAny<SmtpException>()));

        await _service.Send(new MailAddress("test@domain.com", "Jane Doe"), "testTemplate", new { name = "Jane" });

        _helper.Verify(x => x.GetTemplate(It.IsAny<string>()), Times.Once);
        _helper.Verify(x => x.CompileMessage(It.IsAny<MailAddress>(), It.IsAny<EmailTemplate>(), It.IsAny<object>()), Times.Once);
        _helper.Verify(x => x.SendMailAsync(It.IsAny<MailMessage>()), Times.Once);
        _db.Verify(x => x.EmailHistory.Add(It.IsAny<EmailHistory>()), Times.Once);
        _db.Verify(x => x.EmailHistory.Update(It.IsAny<EmailHistory>()), Times.Once);
        _logger.Verify(x => x.LogException(It.IsAny<SmtpException>()), Times.Once);
    }
}