using AutoMapper;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Personal_Blogging_Platform.Data.DTOs;
using Personal_Blogging_Platform.Data.Entities;
using Personal_Blogging_Platform.Data.Repositories;
using Talkable.Data.DTOs.Personal_Blogging_Platform.Data.DTOs;
using static System.Net.WebRequestMethods;

namespace Personal_Blogging_Platform.Service
{
    public class AuthService
    {
        private readonly AuthRepository _repo;
        private readonly IMapper _mapper;
        private readonly EMailService _emailService;
        public AuthService(AuthRepository repo, IMapper mapper, EMailService emailService)
        {
            _repo = repo;
            _mapper = mapper;
            _emailService = emailService;
        }

        internal async Task Regester(UserDto userDto)
        {
            try 
            {
                var existingUser = await _repo.GetUserByEmailAsync(userDto.Email);
                if (existingUser != null)
                {
                    throw new Exception("User with this email already exists.");
                }
                var user = _mapper.Map<User>(userDto);
                user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
                await _repo.AddUserAsync(user);
                string otp = Random.Shared.Next(100000, 999999).ToString();
                OTP userOTP = new OTP
                {
                    UserId = user.Id,
                    Code = otp,
                    ExpirationTime = DateTime.UtcNow.AddMinutes(10)
                };
                await _repo.SaveOTP(userOTP);
                await _emailService.SendEmailAsync(user.Email, "OTP Email Verification", otp);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during registration: " + ex.Message);
            }
        }

        internal async Task VerifyEmail(VerifyEmailDto verifyEmail)
        {
            var existingUser = await _repo.GetUserByEmailAsync(verifyEmail.Email);
            if (existingUser == null)
            {
                throw new Exception("User with this email does not exist.");
            }
            bool checkOtp = await _repo.GetOTPAsync(existingUser.Id, verifyEmail.Otp);
            if (!checkOtp)
            {
                throw new Exception("Invalid OTP.");
            }
            existingUser.IsEmailVerified = true;
            await _repo.UpdateUserAsync(existingUser);
            await _repo.DeleteOTPAsync(existingUser.Id, verifyEmail.Otp);


        }
    }
}
