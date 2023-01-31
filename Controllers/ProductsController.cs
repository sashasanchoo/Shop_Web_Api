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
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace IShop.Controllers
{
    [EnableCors("MyClient", PolicyName = "MyClient")]
    [Route("api/[controller]")]//route prefix
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IShopContext _context;

        public ProductsController(IShopContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet("{categoryName:alpha?}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct(string categoryName = "")
        {
            var result = string.IsNullOrEmpty(categoryName) ? await _context.Product.ToListAsync() : await _context.Product.Where(p => p.Category.Name == categoryName).ToListAsync();
            return Ok(new { products = result, categories = await _context.Category.ToListAsync() });
        }

        // GET: api/Products/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Product.Include(nameof(Product.Category)).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }
        // GET: api/Products/Find/name
        [HttpGet("Find/{productName:alpha}")]
        //[HttpGet("Find")]
        public async Task<ActionResult<Product>> FindProductByName(string productName)
        {
            var product = await _context.Product.Include(nameof(Product.Category)).FirstOrDefaultAsync(p => p.Name == productName);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme}", Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme}", Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme}", Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
