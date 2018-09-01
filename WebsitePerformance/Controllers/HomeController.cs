using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using WebsitePerformance.Helpers;
using WebsitePerformance.Models;
using WebsitePerformance.ModelView;

namespace WebsitePerformance.Controllers
{
    public class HomeController : Controller
    {
        private const int NumOfRequests = 2;

        private readonly ApplicationDbContext _db;

        public HomeController()
        {
            _db = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        public ActionResult Index() => View(new AddModelView(){PageResponses = null});

        private AddModelView CreateAddModelView(string url)
        {
            var pageResponses = new List<Tuple<string, int, int>>();
            var siteInDb = _db.Sites.Single(s => s.Url == url);
            foreach (var pageUrl in siteInDb.PageUrls)
            {
                pageResponses.Add(new Tuple<string, int, int>(pageUrl,
                    _db.PageResponses.ToList().FindAll(p => p.Url == pageUrl).Max(u => u.ResponseTime),
                    _db.PageResponses.ToList().FindAll(p => p.Url == pageUrl).Min(u => u.ResponseTime)));
            }
            return new AddModelView
            {
                PageResponses = pageResponses,

                MaxTimeJSON = JsonConvert.SerializeObject(_db.Sites.Single(u => u.Url == url)
                    .PageResponses.Select(r => new
                    {
                        label = r.Url,
                        y = _db.PageResponses.ToList()
                            .FindAll(p => p.Url == r.Url).Max(u => u.ResponseTime)
                    }).DistinctBy(s => s.label)),

                MinTimeJSON = JsonConvert.SerializeObject(_db.Sites.Single(u => u.Url == url)
                    .PageResponses.Select(r => new
                    {
                        label = r.Url,
                        y = _db.PageResponses.ToList()
                            .FindAll(p => p.Url == r.Url).Min(u => u.ResponseTime)
                    }).DistinctBy(s => s.label))
            };
        }


        [HttpPost]
        public ActionResult Add(string url)
        {
            if (!ModelState.IsValid)
                return View("Index", new AddModelView(){PageResponses = null});

            var siteInDb = _db.Sites.SingleOrDefault(s => s.Url == url);
            if (siteInDb != null)
            {
                foreach (var pageUrl in siteInDb.PageUrls)
                {
                    var resTime = sendReqAndMeasureResTime(pageUrl);
                    _db.PageResponses.Add(new PageResponse() {ResponseTime = resTime, SiteId = siteInDb.Id, Url = pageUrl});
                    siteInDb.PageResponses.Add(new PageResponse() { ResponseTime = resTime, SiteId = siteInDb.Id, Url = pageUrl });
                }
            }
            else
            {
                try
                {
                    var pageUrls = GetSitemapUrls(url);
                    var site = new Site() { Url = url };
                    foreach (var pageUrl in pageUrls)
                    {
                        site.PageUrls.Add(pageUrl);
                        site.PageResponses.AddRange(MeasureResponseTime(site, pageUrl));
                    }
                    _db.Sites.Add(site);
                }
                catch (WebException e)
                {
                    ModelState.AddModelError(string.Empty, e.Message + " Site don't have a sitemap");
                    return View("Index", new AddModelView(){PageResponses = null});
                }
            }

            _db.SaveChanges();
            return View("Index", CreateAddModelView(url));
        }

        private List<PageResponse> MeasureResponseTime(Site site, string pageUrl)
        {
            var pageResponses = new List<PageResponse>();
            for (int i = 0; i < NumOfRequests; i++)
            {
                var resTime = sendReqAndMeasureResTime(pageUrl);
                var pRes = new PageResponse()
                {
                    ResponseTime = resTime,
                    Url = pageUrl,
                    SiteId = site.Id
                };
                pageResponses.Add(pRes);
            }
            
            return pageResponses;
        }

        private List<string> GetSitemapUrls(string url)
        {
            List<string> result;
            try
            {
                 result = SiteMapHelper.GetPageUrls(url + "sitemap.xml");
            }
            catch (WebException)
            {
                 result = SiteMapHelper.GetPageUrls(SiteMapHelper.GetUrlFromRobots(url));  
            }

            return result;
        }

        

        private int sendReqAndMeasureResTime(string url)
        {
            var timer = new Stopwatch();
            timer.Start();

            var request = WebRequest.Create(url);
            try
            {
                using (request.GetResponse()) { }
            }
            catch (Exception)
            {
                // ignore all exceptions
            }

            timer.Stop();
            var time = timer.Elapsed.Milliseconds;
            return time;
        }
    }
}