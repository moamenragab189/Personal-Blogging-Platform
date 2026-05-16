using AutoMapper;
using Personal_Blogging_Platform.Data.DTOs;
using Personal_Blogging_Platform.Data.DTOs.auth;
using Personal_Blogging_Platform.Data.Entities;
using Personal_Blogging_Platform.Data.Repositories;
using Personal_Blogging_Platform.Exceptions;


namespace Personal_Blogging_Platform.Service
{
    public class AuthService
    {
        private readonly AuthRepository _repo;
        private readonly IMapper _mapper;
        private readonly EMailService _emailService;
        private readonly JwtService _jwtService;
        public AuthService(AuthRepository repo, IMapper mapper, EMailService emailService, JwtService jwtService)
        {
            _repo = repo;
            _mapper = mapper;
            _emailService = emailService;
            _jwtService = jwtService;

        }


        internal async Task Regester(UserDto userDto)
        {
            
                var existingUser = await _repo.GetUserByEmailAsync(userDto.Email);
                if (existingUser != null)
                {
                    throw new BadRequestException("User with this email already exists.");
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

        internal async Task VerifyEmail(VerifyEmailDto verifyEmail)
        {
            var existingUser = await _repo.GetUserByEmailAsync(verifyEmail.Email);
            if (existingUser == null)
            {
                throw new BadRequestException("User with this email does not exist.");
            }
            bool checkOtp = await _repo.GetOTPAsync(existingUser.Id, verifyEmail.Otp);
            if (!checkOtp)
            {
                throw new BadRequestException("Invalid OTP.");
            }
            existingUser.IsEmailVerified = true;
            await _repo.UpdateUserAsync(existingUser);
            await _repo.DeleteOTPAsync(existingUser.Id, verifyEmail.Otp);


        }
        internal async Task<string> Login(LoginDto loginDto)
        {
            
                var existingUser = await _repo.GetUserByEmailAsync(loginDto.Email);
                if (existingUser == null)
                {
                    throw new NotFoundException("User with this email does not exist.");
                }
                bool CheckPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, existingUser.HashedPassword);
                if (!CheckPassword)
                {
                    throw new BadRequestException("Invalid password.");
                }
                if (!existingUser.IsEmailVerified)
                {
                    throw new UnauthorizedException("Email is not verified.");
                }
                var claims = _jwtService.AddUserClaims(existingUser.Id, existingUser.Name);
                return _jwtService.CreateToken(claims);
         
            

        }
    }
}
