using Microsoft.EntityFrameworkCore;
using ReviewManagementService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewManagementService.Data
{
    public class ReviewContext : DbContext
    {
        public ReviewContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Rating> Rating { get; set; }
    }
}
