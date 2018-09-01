using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebsitePerformance.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<PageResponse> PageResponses { get; set; }

        public DbSet<Site> Sites { get; set; }

        public DbSet<Page> Pages { get; set; }

        public ApplicationDbContext()
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }
    }
}