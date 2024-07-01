using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TijanasBlog.Models;

namespace TijanasBlog.Data
{
    public class TijanasBlogContext : DbContext
    {
        public TijanasBlogContext (DbContextOptions<TijanasBlogContext> options)
            : base(options)
        {
        }

        public DbSet<TijanasBlog.Models.Brands> Brands { get; set; } = default!;
        public DbSet<TijanasBlog.Models.Items> Items { get; set; } = default!;
        public DbSet<TijanasBlog.Models.ItemsShops> ItemsShops { get; set; } = default!;
        public DbSet<TijanasBlog.Models.Reviews> Reviews { get; set; } = default!;
        public DbSet<TijanasBlog.Models.Shops> Shops { get; set; } = default!;
        public DbSet<TijanasBlog.Models.Users> Users { get; set; } = default!;
    }
}
