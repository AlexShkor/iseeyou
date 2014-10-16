using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISeeYou.Platform.Mvc;
using ISeeYou.ViewServices;
using MongoDB.Driver.Linq;

namespace ISeeYou.Web.Controllers
{
    public class StatsController : BaseController
    {
        private readonly FetchingStatsService _fetchingStats;

        public StatsController(FetchingStatsService fetchingStats)
        {
            _fetchingStats = fetchingStats;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Fetching()
        {
            var items = _fetchingStats.Items.AsQueryable().GroupBy(x => x.SourceId).Select(x => new StatItem
            {
                SourceId = x.Key,
                Count = x.Count()
            }).OrderByDescending(x=> x.Count);
            return View(items);
        }

    }

    public class StatItem
    {
        public int SourceId { get; set; }
        public int Count { get; set; }
    }
}
