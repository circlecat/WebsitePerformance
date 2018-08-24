using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using WebsitePerformance.Models;

namespace WebsitePerformance.ModelView
{
    public class AddModelView
    {
        public IEnumerable<TestedSite> TestedSites { get; set; }
        public int MaxId { get; set; }
        public string MinTimeJSON { get; set; }
        public string MaxTimeJSON { get; set; }
    }
}