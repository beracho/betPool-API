using budgetManager.Dto.AuthDto;
using budgetManager.Dto.UserDto;
using budgetManager.Helpers.Smtp;
using budgetManager.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace budgetManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService, IUserService userService, IMailService mailService)
        {
            _logger = logger;
            _authService = authService;
            _userService = userService;
            _mailService = mailService;
        }

        [HttpPatch("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                resetPasswordDto.Email = resetPasswordDto.Email.ToLower();
                string errorMessage = await _authService.ValidateUsernameOrEmailExist(resetPasswordDto.Email);
                if (errorMessage == "")
                {
                    return BadRequest("Username or email don't exist.");
                }
                if(!await _authService.ValidateRecoveryKey(resetPasswordDto.RecoveryKey, resetPasswordDto.Email))
                {
                    return BadRequest("Key no longer valid");
                }

                await _userService.UpdateUserPassword(resetPasswordDto.Email, resetPasswordDto.NewPassword);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return Problem("Password reset failed");
            }
        }

        [HttpPatch("Recover")]
        public async Task<IActionResult> Recover(RecoveryEmailDto resetPasswordDto)
        {
            try
            {
                var userExists = await _authService.ValidateEmailExist(resetPasswordDto.RecoveryEmail);
                if (!userExists)
                {
                    return BadRequest("Email doesn't exist");

                }
                MailRequest request = new MailRequest()
                {
                    ToEmail = resetPasswordDto.RecoveryEmail,
                    Subject = "Recover your password",
                    Body = await _mailService.GetEmailBody("_password_recovery", resetPasswordDto.RecoveryEmail)
                };
                if (request.Body == "")
                {
                    return BadRequest("Email template not defined");
                }

                var mailResponse = await _mailService.SendEmailAsync(request);

                if (mailResponse != "")
                {
                    return BadRequest(mailResponse);
                }

                return Ok(mailResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return Problem("Register request failed");
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            try
            {
                userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

                string errorMessage = await _authService.ValidateUsernameOrEmailExist(userForRegisterDto.Username);
                if (errorMessage != "")
                {
                    return BadRequest(errorMessage);
                }

                var userCreated = await _authService.Register(userForRegisterDto);

                return CreatedAtRoute("GetUser", new { guid = userCreated.Id.ToString("N") }, userCreated);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return Problem("Register request failed");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            try
            {
                var usernameOrEmail = userForLoginDto.UsernameOrEmail.Trim().ToLower();

                var loginValidation = await _authService.ValidateAccountForLogin(usernameOrEmail);
                if (loginValidation.Message != "Success")
                    return BadRequest(loginValidation.Message);

                var validLogin = await _authService.Login(usernameOrEmail, userForLoginDto.Password);
                if (!validLogin)
                {
                    if (loginValidation.AttemptsLeft > 0)
                        return BadRequest("Wrong password, you have " + loginValidation.AttemptsLeft + " attempts left.");
                    else
                        return BadRequest("Wrong password, you have no attempts left. Please wait one minute to retry");
                }

                var authenticationData = await _authService.AuthenticationData(usernameOrEmail);

                return Ok(new
                {
                    authenticationData.Token,
                    authenticationData.User
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Login failed.");
            }
        }
    }
}
