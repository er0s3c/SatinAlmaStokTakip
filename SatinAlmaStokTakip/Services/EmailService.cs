using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace SatinAlmaStokTakip.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = false);
        Task<bool> SendNotificationAsync(string to, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _smtpServer = _configuration["Email:SmtpServer"] ?? "smtp.gmail.com";
            _smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            _smtpUsername = _configuration["Email:Username"] ?? "";
            _smtpPassword = _configuration["Email:Password"] ?? "";
            _fromEmail = _configuration["Email:FromEmail"] ?? _smtpUsername;
            _fromName = _configuration["Email:FromName"] ?? "Satın Alma Stok Takip Sistemi";
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = false)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_fromName, _fromEmail));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;

                var builder = new BodyBuilder();
                if (isHtml)
                {
                    builder.HtmlBody = body;
                }
                else
                {
                    builder.TextBody = body;
                }

                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_smtpUsername, _smtpPassword);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                // Loglama yapılabilir
                Console.WriteLine($"E-posta gönderme hatası: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendNotificationAsync(string to, string subject, string message)
        {
            var htmlBody = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <div style='background-color: #f8f9fa; padding: 20px;'>
                        <h2 style='color: #007bff;'>{subject}</h2>
                        <p>{message}</p>
                        <hr>
                        <p style='font-size: 12px; color: #6c757d;'>
                            Bu e-posta Satın Alma Stok Takip Sistemi tarafından otomatik olarak gönderilmiştir.
                        </p>
                    </div>
                </body>
                </html>";

            return await SendEmailAsync(to, subject, htmlBody, true);
        }
    }
} 