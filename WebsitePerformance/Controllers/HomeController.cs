using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using WebsitePerformance.Models;
using WebsitePerformance.ModelView;

namespace WebsitePerformance.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController()
        {
            _db = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        public ActionResult Index()
        {
            return View(CreateAddModelView());
        } 

        private AddModelView CreateAddModelView() => new AddModelView
            {
                MaxId = _db.TestedSites.Any() ? _db.TestedSites.Max(s => s.Id) : 0,
                TestedSites = _db.TestedSites.OrderByDescending(s => s.MaxTime).ToList(),
                MaxTimeJSON = JsonConvert.SerializeObject(_db.TestedSites.Select(s => new { label = s.Url, y = s.MaxTime }).ToList()),
                MinTimeJSON = JsonConvert.SerializeObject(_db.TestedSites.Select(s => new { label = s.Url, y = s.MinTime }).ToList())
            };


        [HttpPost]
        public ActionResult Add(TestedSite testedSite)
        {
            if (!ModelState.IsValid)
                return View("Index", CreateAddModelView());

            if (!testedSite.Url.EndsWith("/")) testedSite.Url += "/";
            try
            {
                var sitemapReqTime = SitemapRequest(testedSite.Url);
                var siteInDb = _db.TestedSites.SingleOrDefault(s => s.Url == testedSite.Url);

                if (siteInDb != null)
                {
                    UpdateTime(siteInDb, sitemapReqTime);
                }
                else
                {
                    UpdateTime(testedSite, sitemapReqTime);
                    _db.TestedSites.Add(testedSite);
                }
            }
            catch (WebException exRobot)
            { 
                ModelState.AddModelError(string.Empty, exRobot.Message + " Site don't have a sitemap");
                return View("Index", CreateAddModelView());
            }

            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        private static void UpdateTime(TestedSite site, int time)
        {
            if (time > site.MaxTime || site.MaxTime == null) site.MaxTime = time;
            if (time < site.MinTime || site.MinTime == null) site.MinTime = time;
        }

        private int SitemapRequest(string url)
        {
            var timer = new Stopwatch();
            timer.Start();
            try
            {
                var request = WebRequest.Create(url + "sitemap.xml");
                using (var response = request.GetResponse()){}
            }
            catch (WebException)
            {
                    var robot = GetUrlFromRobots(url);
                    var request = WebRequest.Create(robot);
                using (var response = request.GetResponse()){}
            }
            timer.Stop();
            var time = timer.Elapsed.Milliseconds;
            return time;
        }

        private string GetUrlFromRobots(string url)
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