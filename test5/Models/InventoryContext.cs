using System;
using Microsoft.EntityFrameworkCore;

namespace test5.Models
{
    public class InventoryContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=wwwroot\\App_Data\\Inventory.db");
        }


        public InventoryContext(DbContextOptions<InventoryContext> options)
            : base(options)
        {
        }

        public InventoryContext()
        {
        }

        public DbSet<test5.Models.Inventory> Inventory { get; set; }
    }
}
