using IShop.Data;
using IShop.Model;
using IShop.Services;
using IShop.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IShop.Controllers
{
    [EnableCors("MyClient", PolicyName = "MyClient")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtService _jwtService;
        public UsersController(UserManager<User> userManager, JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }
        [Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme}")]
        [HttpGet]
        public async Task<ActionResult> GetUser()
        {
            User user = await _userManager.FindByNameAsync(User.Identity?.Name);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(new { user.UserName });
        }
        // POST: api/Users
        //Register
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userManager.CreateAsync(user, user.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            user.Password = null;
            user.ConfirmPassword = null;
            return CreatedAtAction("GetUser", new { username = user.UserName }, user);
        }

        // POST: api/Users/ChangePassword
        [Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme}")]
        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            User user = await _userManager.FindByNameAsync(User.Identity?.Name);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            return Ok();
        }

        // POST: api/Users/BearerToken
        //Login
        [HttpPost("BearerToken")]
        public async Task<ActionResult<AuthenticationResponse>> CreateBearerToken(AuthenticationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad credentials");
            }
            var user = await _userManager.FindByEmailAsync(request.EmailAddress);

            //var user = await _userManager.FindByNameAsync(request.EmailAddress);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return BadRequest("Wrong password");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.CreateToken(user, roles);
            return Ok(token);
        }
    }
}