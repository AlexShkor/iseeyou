using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Elmah.ContentSyndication;
using ISeeYou.Documents;
using ISeeYou.Platform.Mvc;
using ISeeYou.ViewServices;
using MongoDB.Driver.Linq;

namespace ISeeYou.Web.Controllers
{
    [RoutePrefix("stats")]
    public class StatsController : BaseController
    {
        private readonly FetchingStatsService _fetchingStats;
        private readonly SourceStatsViewService _sourceStats;

        public StatsController(FetchingStatsService fetchingStats, SourceStatsViewService sourceStats)
        {
            _fetchingStats = fetchingStats;
            _sourceStats = sourceStats;
        }

        [GET("")]
        public ActionResult Index()
        {
            return View();
        }

        [GET("fetching")]
        public ActionResult Fetching()
        {
            var items = _fetchingStats.Items.FindAll().GroupBy(x => x.SourceId)
                .ToDictionary(x => x.Key, v => v.Count());

            var model = _sourceStats.Items.FindAll().Select(x => new StatItem
            {
                SourceId = x.SourceId,
                Fetched = x.Fetched,
                FetchedCount = items.ContainsKey(x.SourceId) ? items[x.SourceId] : 0,
                Count = x.Count
            });

            ViewBag.FetchedCount = model.Count(x => x.FetchedCount > 0);
            return View(model);
        }
    }

    public class StatItem
    {
        public int SourceId { get; set; }
        public int Count { get; set; }
        public DateTime Fetched { get; set; }
        public int FetchedCount { get; set; }
    }
}
