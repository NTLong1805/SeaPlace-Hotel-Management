using FirstProjectNET.Models;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace FirstProjectNET.Service
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
            public async Task SendBookingConfirmationEmail(string toEmail,string customerName,RentForm form)
            {
			    var emailSettings = _configuration.GetSection("EmailSettings");


                
			    var email = new MimeMessage();
                email.From.Add(new MailboxAddress(emailSettings["FromName"], emailSettings["FromEmail"]));
                email.To.Add(new MailboxAddress(customerName, toEmail));
                email.Subject = "Booking Confirmation";

                email.Body = new TextPart("html")
                {
					Text = $@"
                    <h2>Xin chào {customerName},</h2>
                    <p>Bạn đã đặt phòng thành công tại khách sạn của chúng tôi.</p>
                    <p><strong>Thông tin đặt phòng:</strong></p>
                    <ul>
                        <li><strong>Mã đặt phòng:</strong> {form.BookingID}</li>
                        <li><strong>Phòng:</strong> {form.RoomID}</li>
                        <li><strong>Ngày nhận phòng:</strong> {form.DateCheckIn:dd/MM/yyyy}</li>
                        <li><strong>Ngày trả phòng:</strong> {form.DateCheckOut:dd/MM/yyyy}</li>
                    </ul>
                    <p>Cảm ơn bạn đã tin tưởng dịch vụ của chúng tôi.</p>
                    <h2>Seaplace - Hotel</h2>"
				};
			try
			{
				using var smtp = new SmtpClient();
				await smtp.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]), SecureSocketOptions.StartTls);
				await smtp.AuthenticateAsync(emailSettings["FromEmail"], emailSettings["SmtpPassword"]);
				await smtp.SendAsync(email);
				await smtp.DisconnectAsync(true);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi khi gửi email: " + ex.Message);
			}

		}
	}
}
