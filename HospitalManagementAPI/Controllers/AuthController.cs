using Microsoft.AspNetCore.Mvc;
using HospitalManagementAPI.Data;
using HospitalManagementAPI.Models;

using HospitalManagementAPI.Services.Interfaces;

namespace HospitalManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IPasswordService _passwordService;
        private readonly IImageService _imageService;

        public AuthController(
            AppDbContext context,
            IEmailService emailService,
            IPasswordService passwordService,
            IImageService imageService)
        {
            _context = context;
            _emailService = emailService;
            _passwordService = passwordService;
            _imageService = imageService;
        }

        // 1ï¸âƒ£ Send OTP
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp(string email)
        {
            var otp = new Random().Next(100000, 999999).ToString();

            var otpData = new OtpVerification
            {
                Email = email,
                OtpCode = otp,
                ExpiryTime = DateTime.Now.AddMinutes(5),
                IsVerified = false
            };

            _context.OtpVerifications.Add(otpData);
            await _context.SaveChangesAsync();

            await _emailService.SendOtpEmailAsync(email, otp);

            return Ok("OTP sent successfully");
        }

        // 2ï¸âƒ£ Verify OTP
        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp(string email, string otp)
        {
            var otpData = _context.OtpVerifications
                .Where(x => x.Email == email && x.OtpCode == otp)
                .FirstOrDefault();

            if (otpData == null)
                return BadRequest("Invalid OTP");

            if (otpData.ExpiryTime < DateTime.Now)
                return BadRequest("OTP expired");

            otpData.IsVerified = true;
            _context.SaveChanges();

            return Ok("OTP verified successfully");
        }

        // 3ï¸âƒ£ Register Doctor
        [HttpPost("register-doctor")]
        public async Task<IActionResult> RegisterDoctor(
            [FromForm] Doctor doctor,
            IFormFile? profilePhoto)
        {
            var otpVerified = _context.OtpVerifications
                .Where(x => x.Email == doctor.Email && x.IsVerified == true)
                .FirstOrDefault();

            if (otpVerified == null)
                return BadRequest("Email not verified");

            if (profilePhoto != null)
            {
                var imageUrl = await _imageService.UploadImageAsync(profilePhoto);
                doctor.ProfilePhoto = imageUrl;
            }

                        if (string.IsNullOrWhiteSpace(doctor.Password))
                return BadRequest("Password is required");

            doctor.Password = _passwordService.HashPassword(doctor.Password);

            doctor.Status = "Pending";
            doctor.CreatedDate = DateTime.Now;

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Doctor registered successfully. Waiting for admin approval."
            });
        }
    }
}
