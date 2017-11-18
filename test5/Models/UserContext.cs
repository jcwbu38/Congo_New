using System;
using Microsoft.EntityFrameworkCore;

namespace test5.Models
{
    public class UserContext : DbContext
    {
        public UserContext()
        {
        }

        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }

        public DbSet<test5.Models.User> User { get; set; }
    }
}
