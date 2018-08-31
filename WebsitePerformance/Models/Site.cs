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

        public List<string> PageUrls { get; set; }

        public List<PageResponse> PageResponses { get; set; }
    }
}