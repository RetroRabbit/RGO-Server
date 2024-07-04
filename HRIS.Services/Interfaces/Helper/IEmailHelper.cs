using System.Net.Mail;
using RR.UnitOfWork.Entities.Shared;

namespace HRIS.Services.Interfaces.Helper;

public interface IEmailHelper
{
    Task SendMailAsync(MailMessage message);
    MailMessage CompileMessage(MailAddress toAddress, EmailTemplate template, object data);
    string CompileString(string body, object data);
    Task<EmailTemplate> GetTemplate(string templateName);
}