using budgetManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace budgetManager.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet("{guid}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(string Id)
        {
            try
            {
                var userFromRepo = await _userService.GetUserById(Id);
                return Ok(userFromRepo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Failed User request");
            }
        }
    }
}
