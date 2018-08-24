using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebsitePerformance.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<TestedSite> TestedSites { get; set; }
    }
}