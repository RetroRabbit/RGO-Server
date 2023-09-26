﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MimeKit;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Timers;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services
{
    public class EmployeeDataConsumer
    {
        private const string QueueName = "employee_data_queue";

        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        private System.Timers.Timer _consumeTimer;
        private const int TimeInterval = 5 * 60 * 1000; // currently every 5 minutes : change the first number for period.

        private string[] Scopes = { GmailService.Scope.GmailSend };
        private string ApplicationName = "Retro HR";

        public EmployeeDataConsumer(ConnectionFactory factory)
        {
            try
            {
                _factory = factory;
                _connection = _factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                _consumeTimer = new System.Timers.Timer(TimeInterval);
                _consumeTimer.Elapsed += OnTimedEvent;
                _consumeTimer.AutoReset = true;
                _consumeTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }    
        }

        private void SendEmail(Employee employee)
        { 
            if (!IsValidEmail(employee.Email))
            {
                throw new ArgumentException("Invalid email format", nameof(employee.Email));
            }

            UserCredential credential;

            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    System.Threading.CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
            
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            var message = CreateMessage(employee);
            SendMessage(message, service);
        }

        private Google.Apis.Gmail.v1.Data.Message CreateMessage(Employee employee)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Retro Rabbit", "mschoeman@retrorabbit.co.za"));
            emailMessage.To.Add(new MailboxAddress(employee.Name, employee.Email));
            emailMessage.Subject = $"Welcome to Retro Rabbit, {employee.Name}!";
            string body = $@"
            <html>
            <body>
                <p>Dear {employee.Name},</p>
                <p>We are thrilled to have you on board! Welcome to the Retro family.</p>
                <p>From today, you embark on a new journey with us, filled with exciting opportunities, challenges, and growth. At Retro, we pride ourselves on fostering a culture of collaboration, innovation, and mutual respect. We believe that every individual brings a unique perspective and talent to the team, and we can't wait to see the wonderful contributions you'll make.</p>
                <p>Remember, it's okay to feel overwhelmed or have questions. We've all been there. Don't hesitate to ask or seek clarification on anything. Our doors (and inboxes) are always open.</p>
                <p>Once again, welcome to Retro Rabbit. Here's to new beginnings and the start of a memorable journey together!</p>
                <p>Click <a href='http://localhost:4200/'>here</a> to visit our employee portal</p>
            </body>
            </html>";
            // Todo: Change link to the actual link when deployed. Currently set to localhost
            emailMessage.Body = new TextPart("html") { Text = body };

            var rawMessage = Base64UrlEncode(emailMessage.ToString());
            var message = new Google.Apis.Gmail.v1.Data.Message
            {
                Raw = rawMessage
            };
            return message;
        }
        private void SendMessage(Google.Apis.Gmail.v1.Data.Message message, GmailService service)
        {
            var sentMessage = service.Users.Messages.Send(message, "me").Execute();

            if (!string.IsNullOrEmpty(sentMessage.Id))
            {
                Console.WriteLine($"Email sent successfully. Message ID: {sentMessage.Id}");
            }
            else
            {
                Console.WriteLine("Failed to send email.");
            }
        }
        private string Base64UrlEncode(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            ConsumeAndSendBatchEmails();
        }

        public void ConsumeAndSendBatchEmails()
        {
            List<Employee> newEmployee = new List<Employee>();

            while (true)
            {
                BasicGetResult result = _channel.BasicGet(QueueName, false);
                if (result == null) break;

                var body = result.Body.ToArray();
                var employeeData = JsonConvert.DeserializeObject<Employee>(Encoding.UTF8.GetString(body));
                newEmployee.Add(employeeData);

                _channel.BasicAck(result.DeliveryTag, false);
            }

            foreach (var employee in newEmployee)
            {
                try
                {
                    if (!string.IsNullOrEmpty(employee.Email))
                    {
                        SendEmail(employee);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
