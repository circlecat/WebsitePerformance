using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebsitePerformance.Models
{
    public class TestedSite
    {
        public int Id { get; set; }

        [Required]
        [UrlValidation]
        public string Url { get; set; }

        public int? MaxTime { get; set; }

        public int? MinTime { get; set; }

        public TestedSite()
        {
            MaxTime = null;
            MinTime = null;
        }
    }
}