﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
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
            var items = _fetchingStats.Items.FindAll().GroupBy(x => x.SourceId).Select(x => new StatItem
            {
                SourceId = x.Key,
                Count = x.Count()
            }).OrderByDescending(x=> x.Count).ToList();
            ViewBag.SourcesCount = _sourceStats.Items.FindAll().Count();
            return View(items);
        }
    }

    public class StatItem
    {
        public int SourceId { get; set; }
        public int Count { get; set; }
    }
}
