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
        public List<Tuple<string, int, int>> PageResponses { get; set; }
        public string MinTimeJSON { get; set; }
        public string MaxTimeJSON { get; set; }
    }
}