﻿using budgetManager.Dto.AuthDto;
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
        private readonly IMailService _mailService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService, IMailService mailService)
        {
            _logger = logger;
            _authService = authService;
            _mailService = mailService;
        }

        [HttpPatch("Recover")]
        public async Task<IActionResult> Recover()
        {
            try
            {
                MailRequest request = new MailRequest()
                {
                    ToEmail= "b.beracho@gmail.com",
                    Subject= "Sending emails",
                    Body= "This email was sent successfully"
                };
                var mailResponse = await _mailService.SendEmailAsync(request);

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

                string errorMessage = await _authService.ValidateUsernameOrEmailExist(userForRegisterDto);
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
                    if(loginValidation.AttemptsLeft > 0)
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
