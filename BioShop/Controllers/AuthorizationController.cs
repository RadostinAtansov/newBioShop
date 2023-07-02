namespace BioShop.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using BioShop.Data.Services.Interfaces;
    using BioShop.Data.ViewModels.UserModels;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationUserService _userService;

        public AuthorizationController(IAuthorizationUserService userService)
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