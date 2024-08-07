﻿using System.Net;
using System.Net.Mail;
using RR.UnitOfWork;
using HandlebarsDotNet;
using HRIS.Services.Interfaces.Helper;
using HRIS.Services.Services;
using RR.UnitOfWork.Entities.Shared;
using HRIS.Models;
using Microsoft.Extensions.Options;

namespace HRIS.Services.Helpers;

public class EmailHelper : IEmailHelper
{
    private readonly IUnitOfWork _db;
    private readonly SMTPSettings _smtpSettings;
    private readonly string _fromHost;
    private readonly string _fromName;
    private readonly string _fromMail;
    private readonly string _fromPassword;
    private readonly int _fromPort;

    public EmailHelper(IUnitOfWork db, IOptions<SMTPSettings> options)
    {
        _db = db;
        _smtpSettings = options.Value;
        _fromHost = _smtpSettings.Host ?? EnvironmentVariableHelper.SMTP_HOST!;
        _fromName = _smtpSettings.Name ?? EnvironmentVariableHelper.SMTP_NAME!;
        _fromMail = _smtpSettings.Mail ?? EnvironmentVariableHelper.SMTP_MAIL!;
        _fromPassword = _smtpSettings.Password ?? EnvironmentVariableHelper.SMTP_PASSWORD!;
        _fromPort = int.Parse(_smtpSettings.Port ?? EnvironmentVariableHelper.SMTP_PORT!);
    }

    public async Task SendMailAsync(MailMessage message)
    {
        using var smtpClient = new SmtpClient(_fromHost);
        smtpClient.Port = _fromPort;
        smtpClient.Credentials = new NetworkCredential(_fromMail, _fromPassword);
        smtpClient.EnableSsl = true;
        await smtpClient.SendMailAsync(message);
    }

    public MailMessage CompileMessage(MailAddress toAddress, EmailTemplate template, object data)
    {
        var message = new MailMessage
        {
            From = new MailAddress(_fromMail, _fromName),
            Subject = CompileString(template.Subject, data),
            Body = CompileString(template.Body, data),
            IsBodyHtml = true
        };
        message.To.Add(toAddress);
        return message;
    }

    public string CompileString(string body, object data)
    {
        return Handlebars.Compile(body)(data);
    }

    public async Task<EmailTemplate> GetTemplate(string templateName)
    {
        return (await _db.EmailTemplate.FirstOrDefault(x => x.Name == templateName)) ??
               throw new CustomException($"Email template '{templateName}' does not exist");
    }
}