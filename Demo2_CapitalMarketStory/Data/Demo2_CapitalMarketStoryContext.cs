using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Demo2_CapitalMarketStory.Models;

namespace Demo2_CapitalMarketStory.Data
{
    public class Demo2_CapitalMarketStoryContext : DbContext
    {
        public Demo2_CapitalMarketStoryContext (DbContextOptions<Demo2_CapitalMarketStoryContext> options)
            : base(options)
        {
        }

        public DbSet<Demo2_CapitalMarketStory.Models.Company> Company { get; set; } = default!;
    }
}
