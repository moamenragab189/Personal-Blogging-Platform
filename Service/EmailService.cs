using SendGrid;
using SendGrid.Helpers.Mail;

namespace Personal_Blogging_Platform.Service
{
    public class EmailService
    {
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _apiKey = configuration["SendGrid:ApiKey"]
                ?? throw new InvalidOperationException("SendGrid API key not configured");
            _fromEmail = configuration["SendGrid:FromEmail"] ?? "noreply@yourdomain.com";
            _fromName = configuration["SendGrid:FromName"] ?? "Personal Blogging Platform";
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string code)
        {
            _logger.LogInformation("Attempting to send email via SendGrid to {To} from {From}", to, _fromEmail);

            try
            {
                var client = new SendGridClient(_apiKey);
                var from = new EmailAddress(_fromEmail, _fromName);
                var toAddress = new EmailAddress(to);

                var htmlContent = $@"
<div style='font-family: Tahoma, Arial, sans-serif; max-width: 500px; margin: 0 auto; border: 1px solid #eee; border-radius: 10px; padding: 20px; text-align: center;'>
    <h2 style='color: #2196F3;'>Welcome to Personal Blogging Platform!</h2>
    <p style='font-size: 16px; color: #555;'>Thank you for joining us,</p>
    <p style='font-size: 14px; color: #777;'>We are thrilled to have you on <strong>Personal Blogging Platform</strong>. Please use the verification code below to confirm your email and activate your account:</p>
    
    <div style='background-color: #f9f9f9; padding: 15px; border-radius: 8px; margin: 20px 0;'>
        <p style='font-size: 12px; color: #999; margin-bottom: 5px;'>Account Activation Code (OTP):</p>
        <h1 style='color: #2196F3; letter-spacing: 5px; margin: 0;'>{code}</h1>
    </div>

    <p style='font-size: 13px; color: #888;'>This code is valid for 10 minutes.</p>
    <hr style='border: 0; border-top: 1px solid #eee; margin: 20px 0;'>
    <p style='font-size: 11px; color: #aaa;'>If you did not create an account on Personal Blogging Platform, you can ignore this email.</p>
    <p style='font-size: 12px; color: #2196F3; font-weight: bold;'>Personal Blogging Platform Team</p>
</div>";

                var msg = MailHelper.CreateSingleEmail(from, toAddress, subject, $"Your OTP is: {code}", htmlContent);

                _logger.LogDebug("SendGrid request prepared. API Key prefix: {Prefix}", _apiKey[..10]);

                var response = await client.SendEmailAsync(msg);

                _logger.LogInformation("SendGrid response received. Status: {StatusCode}", (int)response.StatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Body.ReadAsStringAsync();
                    _logger.LogError("SendGrid API error. Status: {StatusCode}, Body: {ErrorBody}", (int)response.StatusCode, errorBody);
                    throw new InvalidOperationException($"SendGrid failed with status {(int)response.StatusCode}: {errorBody}");
                }

                _logger.LogInformation("Email sent successfully to {To} with subject '{Subject}'", to, subject);
            }
            catch (Exception ex) when (ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Unexpected error while sending email to {To}", to);
                throw;
            }
        }
    }
}