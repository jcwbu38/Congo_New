using System;
using Microsoft.EntityFrameworkCore;

namespace test5.Models
{
    public class ReportingContext : DbContext
    {
        public ReportingContext(DbContextOptions<ReportingContext> options)
            : base(options)
        {
        }

        public DbSet<test5.Models.Inventory> Reporting { get; set; }
    }
}
