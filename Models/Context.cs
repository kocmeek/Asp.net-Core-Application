using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class Context :DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server = DESKTOP-ICEIUI6; database = GeoSYSDB ; integrated security = true");
        }

        public DbSet<Categories> Categories { get; set; }
        public DbSet<Products> Products { get; set; }

    }
}
