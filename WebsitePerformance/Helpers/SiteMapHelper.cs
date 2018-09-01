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
    }
}