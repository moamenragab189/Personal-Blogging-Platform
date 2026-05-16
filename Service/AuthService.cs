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
        private readonly ILogger<AuthService> _logger;
        public AuthService(AuthRepository repo, IMapper mapper, EMailService emailService, JwtService jwtService, ILogger<AuthService> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _emailService = emailService;
            _jwtService = jwtService;
            _logger = logger;
        }


        internal async Task Regester(UserDto userDto)
        {
            _logger.LogInformation("Registering a new user with email: {Email}", userDto.Email);
                var existingUser = await _repo.GetUserByEmailAsync(userDto.Email);
                if (existingUser != null)
                {
                  _logger.LogWarning("Registration failed: User with email {Email} already exists.", userDto.Email);
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
                _logger.LogInformation("OTP sent to user with email: {Email}", user.Email);
        }

        internal async Task VerifyEmail(VerifyEmailDto verifyEmail)
        {
            _logger.LogInformation("Verifying email for user with email: {Email}", verifyEmail.Email);
            var existingUser = await _repo.GetUserByEmailAsync(verifyEmail.Email);
            if (existingUser == null)
            {
                   _logger.LogWarning("Email verification failed: User with email {Email} does not exist.", verifyEmail.Email);
                throw new BadRequestException("User with this email does not exist.");
            }
            bool checkOtp = await _repo.GetOTPAsync(existingUser.Id, verifyEmail.Otp);
            if (!checkOtp)
            {
                _logger.LogWarning("Email verification failed: Invalid OTP for user with email {Email}.", verifyEmail.Email);
                throw new BadRequestException("Invalid OTP.");
            }
            existingUser.IsEmailVerified = true;
            await _repo.UpdateUserAsync(existingUser);
            await _repo.DeleteOTPAsync(existingUser.Id, verifyEmail.Otp);
            _logger.LogInformation("Email verification successful for user with email: {Email}", verifyEmail.Email);


        }
        internal async Task<string> Login(LoginDto loginDto)
        {
            _logger.LogInformation("User attempting to login with email: {Email}", loginDto.Email);
                var existingUser = await _repo.GetUserByEmailAsync(loginDto.Email);
                if (existingUser == null)
                {
                    _logger.LogWarning("Login failed: User with email {Email} does not exist.", loginDto.Email);
                    throw new NotFoundException("User with this email does not exist.");
                }
                bool CheckPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, existingUser.HashedPassword);
                if (!CheckPassword)
                {
                    _logger.LogWarning("Login failed: Invalid password for user with email {Email}.", loginDto.Email);
                    throw new BadRequestException("Invalid password.");
                }
                if (!existingUser.IsEmailVerified)
                {
                    _logger.LogWarning("Login failed: Email not verified for user with email {Email}.", loginDto.Email);
                    throw new UnauthorizedException("Email is not verified.");
                }
                var claims = _jwtService.AddUserClaims(existingUser.Id, existingUser.Name);
                _logger.LogInformation("Login successful for user with email: {Email}", loginDto.Email);
                return _jwtService.CreateToken(claims);
         
            

        }
    }
}
