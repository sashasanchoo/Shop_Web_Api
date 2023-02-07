using IShop.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IShop.Controllers
{
    [EnableCors("MyClient", PolicyName = "MyClient")]
    //[Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme}", Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoleStore<IdentityRole> _roleStore;
        private readonly UserManager<User> _userManager;
        public RolesController(RoleManager<IdentityRole> roleManager, IRoleStore<IdentityRole> roleStore, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _roleStore = roleStore;
            _userManager = userManager;
        }
        // GET: api/Roles
        [Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme}", Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> GetAllRoles()
        {
            return await _roleManager.Roles.ToListAsync();
        }
        // GET: api/Post
        [Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme}", Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateRole(InputRole role)
        {
            if(ModelState.IsValid)
            {
                await _roleManager.CreateAsync(new IdentityRole(role.RoleName));
            }
            return CreatedAtAction("GetAllRoles", role);
        }
        // DELETE: api/Categories/5
        [Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme}", Roles = "Admin")]
        [HttpDelete("{roleName:alpha}")]
        public async Task<IActionResult> DeleteCategory(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound();
            }
            await _roleStore.DeleteAsync(role, CancellationToken.None);
            return NoContent();
        }
        [Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme}")]
        [HttpGet("IsAdmin")]
        public async Task<ActionResult<bool>> IsAdmin()
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            return Ok(await _userManager.IsInRoleAsync(user, "Admin"));
        }
    }
}
