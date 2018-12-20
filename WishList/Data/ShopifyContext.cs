using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WishList.Models;

namespace WishList.Data
{
    public class ShopifyContext : DbContext
    {
        public ShopifyContext(DbContextOptions<ShopifyContext> options) : base(options)
        {
        }

        public DbSet<Shopify> Shopify { get; set; }
        public DbSet<Customer> Customer { get; set; }
    }
}
