using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Task5WebApp.Model;

namespace Task5WebApp.Data
{
    public class Task5WebAppContext : DbContext
    {
        public Task5WebAppContext (DbContextOptions<Task5WebAppContext> options)
            : base(options)
        {
        }

        public DbSet<Task5WebApp.Model.ApplicationUser> ApplicationUser { get; set; } = default!;
        public DbSet<Task5WebApp.Model.Message> Message { get; set; } = default!;
    }
}
