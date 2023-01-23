using IShop.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IShop.Controllers
{
    [Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme}, ApiKey", Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoleStore<IdentityRole> _roleStore;
        public RolesController(RoleManager<IdentityRole> roleManager, IRoleStore<IdentityRole> roleStore)
        {
            _roleManager = roleManager;
            _roleStore = roleStore;
        }
        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> GetAllRoles()
        {
            return await _roleManager.Roles.ToListAsync();
        }
        // GET: api/Post
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
    }
}
