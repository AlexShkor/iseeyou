using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ISeeYou.Helpers
{
    public class DateHelper
    {
        public static List<SelectListItem> Months
        {
            get
            {
                return new Dictionary<int, string>
                {
                    {1, "January"},
                    {2, "February"},
                    {3, "March"},
                    {4, "April"},
                    {5, "May"},
                    {6, "June"},
                    {7, "July"},
                    {8, "August"},
                    {9, "September"},
                    {10, "October"},
                    {11, "November"},
                    {12, "December"},
                }.Select(x => new SelectListItem {Value = x.Key.ToString(CultureInfo.InvariantCulture), Text = x.Value}).ToList();
            }
        }

        public static List<SelectListItem> Days
        {
            get
            {
                var result = new List<SelectListItem>();
                for (var i = 1; i <= 28; i++)
                {
                    result.Add(new SelectListItem {Value = i.ToString(CultureInfo.InvariantCulture), Text = DayWithPostfix(i)});
                }
                result.Add(new SelectListItem {Value = "31", Text = "Last"});
                return result;
            }
        }

        public static List<SelectListItem> Years
        {
            get
            {
                var years = new List<SelectListItem>();
                var currentYear = DateTime.Now.Year;
                for (var year = currentYear; year < currentYear + 20; year++)
                {
                    years.Add(new SelectListItem
                    {
                        Text = year.ToString(CultureInfo.InvariantCulture),
                        Value = year.ToString(CultureInfo.InvariantCulture)
                    });
                }
                return years;
            }
        } 

        private static string DayWithPostfix(int i)
        {
            string postfix = "th";
            var isTeenth = i%100/10 == 1;
            if (!isTeenth)
            {
                switch (i % 10)
                {
                    case 1:
                        postfix = "st";
                        break;
                    case 2:
                        postfix = "nd";
                        break;
                    case 3:
                        postfix = "rd";
                        break;
                }
            }

            return string.Format("{0}{1}", i, postfix);
        }
    }
}