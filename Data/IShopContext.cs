using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IShop.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IShop.Data
{
    public class IShopContext : IdentityDbContext<User, IdentityRole, string>
    {
        public IShopContext (DbContextOptions<IShopContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; } = default!;

        public DbSet<Product> Product { get; set; }

        public DbSet<UserApiKey> UserApiKeys { get; set; }
    }
}
