using System;
using Microsoft.EntityFrameworkCore;

namespace test5.Models
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options)
            : base(options)
        {
        }

        public DbSet<test5.Models.Inventory> Inventory { get; set; }
    }
}
