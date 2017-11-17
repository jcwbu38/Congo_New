using System;
using Microsoft.EntityFrameworkCore;

namespace test5.Models
{
    public class PurchaseOrderContext : DbContext
    {
        public PurchaseOrderContext(DbContextOptions<PurchaseOrderContext> options)
            : base(options)
        {
        }

        public DbSet<test5.Models.PurchaseOrder> PurchaseOrder { get; set; }
    }
}
