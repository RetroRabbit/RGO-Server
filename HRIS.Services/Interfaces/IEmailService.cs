using HRIS.Models;
using MimeKit;
using System.Net.Mail;

namespace HRIS.Services.Interfaces;

public interface IEmailService
{
    Task Send(MailAddress toAddress, string templateName, object data);
    Task Send(EmployeeDto employee, string templateName);
}