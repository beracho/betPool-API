using budgetManager.Dto.UserDto;
using budgetManager.Services.Interfaces;
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

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
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
    }
}
