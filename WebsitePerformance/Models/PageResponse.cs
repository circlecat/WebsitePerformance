using System.ComponentModel.DataAnnotations.Schema;

namespace WebsitePerformance.Models
{
    public class PageResponse
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public int ResponseTime { get; set; }

        public Site Site { get; set; }

        public int SiteId { get; set; }
    }
}