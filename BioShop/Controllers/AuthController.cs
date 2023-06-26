namespace BioShop.Controllers
{
    using BioShop.Data.Services.Interfaces;
    using BioShop.Data.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO requestUser)
        {
            var newRegisterUser = await _userService.Register(requestUser);

            return Ok(newRegisterUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDTO requestUser)
        {
            var result = await _userService.Login(requestUser);

            return Ok(result);
        }
    }
}