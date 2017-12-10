using System;
using Microsoft.EntityFrameworkCore;

namespace test5.Models
{
    public class User2Context : DbContext
    {
        public User2Context(DbContextOptions<User2Context> options)
            : base(options)
        {
        }

        public DbSet<test5.Models.User2> AspNetUsers { get; set; }
    }
}