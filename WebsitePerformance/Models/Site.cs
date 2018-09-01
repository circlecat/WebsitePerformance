using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebsitePerformance.Models
{
    public class Site
    {
        public int Id { get; set; }

        [UrlValidation]
        [Required]
        public string Url { get; set; }

        public virtual List<Page> Pages { get; set; }

        public virtual List<PageResponse> PageResponses { get; set; }

        public Site()
        {
            PageResponses = new List<PageResponse>();
            Pages = new List<Page>();
        }
    }
}