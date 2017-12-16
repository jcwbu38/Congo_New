using System;
using Microsoft.EntityFrameworkCore;

namespace test5.Models
{
    public class PurchaseOrderContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=PurchaseOrder.db");
        }



        public PurchaseOrderContext(DbContextOptions<PurchaseOrderContext> options)
            : base(options)
        {
        }

        public PurchaseOrderContext()
        {
        }

        public DbSet<test5.Models.PurchaseOrder> PurchaseOrder { get; set; }
    }
}
