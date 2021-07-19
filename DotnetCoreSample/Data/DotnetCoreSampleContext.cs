using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotnetCoreSample;

namespace DotnetCoreSample.Data
{
    public class DotnetCoreSampleContext : DbContext
    {
        public DotnetCoreSampleContext (DbContextOptions<DotnetCoreSampleContext> options)
            : base(options)
        {
        }

        public DbSet<DotnetCoreSample.User> User { get; set; }
    }
}
