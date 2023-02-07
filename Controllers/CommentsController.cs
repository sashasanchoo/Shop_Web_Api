using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IShop.Data;
using IShop.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;

namespace IShop.Controllers
{
    [EnableCors("MyClient", PolicyName = "MyClient")]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IShopContext _context;
        public CommentsController(IShopContext context)
        {
            _context = context;
        }

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme}")]
        [HttpPost]
        public async Task<ActionResult> PostComment(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            else if (!await _context.Product.AnyAsync(p => p.Id == comment.ProductId))
            {
                ModelState.AddModelError(string.Empty, "The product with given identifier does not exist.");
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            comment.Username = User.Identity?.Name;
            comment.Published = DateTime.Now;
            _context.Comment.Add(comment);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Comments/5
        [Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme}", Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                ModelState.AddModelError(string.Empty, "The comment with given identifier does not exist.");
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }

            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
