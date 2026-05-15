
using MailKit.Net.Smtp;
using MimeKit;

namespace Personal_Blogging_Platform.Service
{
    public class EMailService
    {
        private string verifyEmailMessage = $@"
<div style='font-family: Tahoma, Arial, sans-serif; max-width: 500px; margin: 0 auto; border: 1px solid #eee; border-radius: 10px; padding: 20px; text-align: center;'>
    <h2 style='color: #2196F3;'>Welcome to Personal Blogging Platform!</h2>
    <p style='font-size: 16px; color: #555;'>Thank you for joining us,</p>
    <p style='font-size: 14px; color: #777;'>We are thrilled to have you on <strong>Personal Blogging Platform</strong>. Please use the verification code below to confirm your email and activate your account:</p>
    
    <div style='background-color: #f9f9f9; padding: 15px; border-radius: 8px; margin: 20px 0;'>
        <p style='font-size: 12px; color: #999; margin-bottom: 5px;'>Account Activation Code (OTP):</p>
        <h1 style='color: #2196F3; letter-spacing: 5px; margin: 0;'>Code</h1>
    </div>

    <p style='font-size: 13px; color: #888;'>This code is valid for 10 minutes.</p>
    <hr style='border: 0; border-top: 1px solid #eee; margin: 20px 0;'>
    <p style='font-size: 11px; color: #aaa;'>If you did not create an account on Personal Blogging Platform, you can ignore this email.</p>
    <p style='font-size: 12px; color: #2196F3; font-weight: bold;'>Personal Blogging Platform Team</p>
</div>";
        public async Task SendEmailAsync(string to, string subject, string code)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("moamenragab66@gmail.com"));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
           
         
            
            

                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = verifyEmailMessage.Replace("Code", code)
                };
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("moamenragab66@gmail.com", "lltzexpbvqguzoeh");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
