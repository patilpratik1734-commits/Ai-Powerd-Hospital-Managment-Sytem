using System.Net;
using System.Net.Mail;
using HospitalManagementAPI.Services.Interfaces;

namespace HospitalManagementAPI.Services.Implementations
{
    public class EmailService : IEmailService
    {
        public async Task SendOtpEmailAsync(string email, string otp)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("systemhospitalmangement@gmail.com", "hcca oifw bwmx ovjp"),
                EnableSsl = true
            };

            string htmlBody = $@"
            <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f6f8;
                        padding: 20px;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: auto;
                        background: #ffffff;
                        border-radius: 10px;
                        padding: 30px;
                        box-shadow: 0 0 10px rgba(0,0,0,0.1);
                    }}
                    .header {{
                        text-align: center;
                        color: #0ea5e9;
                        font-size: 24px;
                        font-weight: bold;
                        margin-bottom: 20px;
                    }}
                    .otp-box {{
                        background: #0ea5e9;
                        color: #ffffff;
                        font-size: 28px;
                        font-weight: bold;
                        text-align: center;
                        padding: 15px;
                        border-radius: 8px;
                        letter-spacing: 6px;
                        margin: 20px 0;
                    }}
                    .footer {{
                        margin-top: 30px;
                        font-size: 12px;
                        color: #777;
                        text-align: center;
                    }}
                </style>
            </head>

            <body>
                <div class='container'>
                    
                    <div class='header'>
                        AI Powered Hospital Management System
                    </div>

                    <p>Hello,</p>

                    <p>
                        Your One-Time Password (OTP) for verification is:
                    </p>

                    <div class='otp-box'>
                        {otp}
                    </div>

                    <p>
                        This OTP is valid for the next <b>5 minutes</b>. Please do not share this code with anyone.
                    </p>

                    <p>
                        If you did not request this OTP, please ignore this email.
                    </p>

                    <div class='footer'>
                        © 2026 AI Powered Hospital Management System <br/>
                        Secure Healthcare Technology
                    </div>

                </div>
            </body>
            </html>";

            var message = new MailMessage
            {
                From = new MailAddress("systemhospitalmangement@gmail.com", "AI Powered Hospital Management System"),
                Subject = "Your OTP Verification Code",
                Body = htmlBody,
                IsBodyHtml = true
            };

            message.To.Add(email);

            await smtpClient.SendMailAsync(message);
        }
    }

}
