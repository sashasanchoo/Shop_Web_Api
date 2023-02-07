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
using System.Web.Http.Results;
using IShop.ViewModels;
using NuGet.Protocol.Plugins;
using Azure.Core;

namespace IShop.Controllers
{
    [EnableCors("MyClient", PolicyName = "MyClient")]
    [Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme}", Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IShopContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly string _imagesDirectory;
        private readonly string _imagesFullPath;

        public ProductsController(IShopContext context, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _context = context;
            _environment = environment;
            _configuration = configuration;
            _imagesDirectory = configuration.GetValue<string>("StaticFilesDirectory");
            _imagesFullPath = Path.Combine(_environment.WebRootPath, _imagesDirectory);
        }

        // GET: api/Products
        [AllowAnonymous]
        [HttpGet("{categoryName:alpha?}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct(string categoryName = "")
        {
            //return only products
            var result = string.IsNullOrEmpty(categoryName) ? await _context.Product.Include(nameof(Category)).ToListAsync() : await _context.Product.Include(nameof(Category)).Where(p => p.Category.Name == categoryName).ToListAsync();
            return Ok(result);
        }

        // GET: api/Products/5
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Product.Include(nameof(Product.Category)).Include(nameof(Product.Comments)).FirstOrDefaultAsync(p => p.Id == id);
            if(product == null)
            {
                ModelState.AddModelError(string.Empty, "The product with given identifier does not exist.");
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            return product;
        }
        // GET: api/Products/Find/name
        [AllowAnonymous]
        [HttpGet("Find/{productName:alpha}")]
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
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PutProduct(int id,[FromForm] ProductCreatingWithFileUploadRequest request)
        {
            if (id != request.Product.Id)
            {
                return BadRequest();
            }
            var category = await _context.Category.FirstOrDefaultAsync(c => c.Name == request.Product.CategoryName);
            if (category == null)
            {
                ModelState.AddModelError(string.Empty, "The given category does not exist. ");
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var product = new Product
            {
                Id = request.Product.Id,
                Name = request.Product.Name,
                Published = request.Product.Published,
                ImagePath = request.Product.ImagePath,
                Content = request.Product.Content,
                Price = request.Product.Price,
                CategoryId = category.Id
            };
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
            await TryCopyImage(request.DataFile);
            return NoContent();
        }
        // POST: api/Products
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> PostProduct([FromForm] ProductCreatingWithFileUploadRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var category = await _context.Category.FirstOrDefaultAsync(c => c.Name == request.Product.CategoryName);
            if(category == null)
            {
                ModelState.AddModelError(string.Empty, "The given category does not exist. ");
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            _context.Product.Add(new Product
            {
                Name = request.Product.Name,
                Published = request.Product.Published,
                ImagePath = request.Product.ImagePath,
                Content = request.Product.Content,
                Price = request.Product.Price,
                CategoryId = category.Id
            });
            await _context.SaveChangesAsync();
            await TryCopyImage(request.DataFile);
            return NoContent();
        }

        // DELETE: api/Products/5
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
        private async Task TryCopyImage(IFormFile image)
        {
            if (!FileExists(image.FileName))
            {
                await using (var file = new FileStream(Path.Join(_imagesFullPath, image.FileName), FileMode.Create, FileAccess.Write))
                {
                    await image.CopyToAsync(file);
                }
            }
            
        }
        private bool FileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }
    }
}