using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;

namespace WebsitePerformance.Helpers
{
    public static class SiteMapHelper
    {
        public static List<string> GetPageUrls(string url)
        {
            var wc = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var reply = wc.DownloadString(url); //Here throws exception, if something going wrong
            var urlDoc = new XmlDocument();
            urlDoc.LoadXml(reply);

            var xList = urlDoc.GetElementsByTagName("loc");

            var result = new List<string>();

            foreach (XmlNode node in xList)
            {
                var locUrl = node.InnerText; //Add to list (result)
                result.Add(locUrl);
            }

            return result;
        }


        public static string GetUrlFromRobots(string url)
        {
            var resultUrl = "";
            var request = WebRequest.Create(url + "robots.txt");
            using (var response = request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var line = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line.Contains("Sitemap:"))
                                resultUrl = line.Substring(9);
                        }
                    }
                }
            }
            return resultUrl;
        }

        /*var pageResponses = new List<Tuple<string, int,int>>();
            var siteInDb = _db.Sites.Single(u => u.Url == url);
            foreach (var res in siteInDb.PageUrls)
            {
                pageResponses.Add(new Tuple<string, int, int>(res,
                    _db.PageResponses.ToList().FindAll(p => p.Url == res).Max(u => u.ResponseTime),
                    _db.PageResponses.ToList().FindAll(p => p.Url == res).Min(u => u.ResponseTime)));
            }

            return new AddModelView
            {
                PageResponses = pageResponses,

                MaxTimeJSON = JsonConvert.SerializeObject(_db.Sites.Single(u => u.Url == url)
                .PageResponses.DistinctBy(s => s.Url).Select(r => new
                {
                    label = r.Url,
                    y = _db.PageResponses.ToList()
                        .FindAll(p => p.Url == r.Url).Max(u => u.ResponseTime)
                })),

                MinTimeJSON = JsonConvert.SerializeObject(_db.Sites.Single(u => u.Url == url)
                .PageResponses.DistinctBy(s => s.Url).Select(r => new
                {
                    label = r.Url,
                    y = _db.PageResponses.ToList()
                        .FindAll(p => p.Url == r.Url).Min(u => u.ResponseTime)
                }))
            };*/
    }
}