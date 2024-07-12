using HRIS.Services.Interfaces;
using System.Net.Mail;
using RR.UnitOfWork;
using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.Shared;
using HRIS.Services.Interfaces.Helper;

namespace HRIS.Services.Services;

public class EmailService : IEmailService
{
    private readonly IUnitOfWork _db;
    private readonly IErrorLoggingService _logger;
    private readonly IEmailHelper _helper;

    public EmailService(IUnitOfWork db, IErrorLoggingService logger, IEmailHelper helper)
    {
        _db = db;
        _logger = logger;
        _helper = helper;
    }

    public async Task Send(MailAddress toAddress, string templateName, object data)
    {
        var template = await _helper.GetTemplate(templateName);
        var message = _helper.CompileMessage(toAddress, template, data);
        var history = await _db.EmailHistory.Add(new EmailHistory(message, template.Id));

        try
        {
            await _helper.SendMailAsync(message);
        }
        catch (SmtpException se)
        {
            _logger.LogException(se);
            history.Status = EmailStatus.Failed;
        }

        if (history.Status == EmailStatus.Draft)
            history.Status = EmailStatus.Sent;

        await _db.EmailHistory.Update(history);
    }

    public async Task Send(EmployeeDto employee, string templateName)
    {
        var toAddress = new MailAddress(employee.Email, $"{employee.Name} {employee.Surname}");
        await Send(toAddress, templateName, employee);
    }
}