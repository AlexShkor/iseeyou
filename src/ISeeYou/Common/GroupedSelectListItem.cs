using System.Collections.Generic;
using System.Web.Mvc;

namespace ISeeYou.Common
{
    public class GroupedSelectListItem : SelectListItem
    {
        public List<SelectListItem> Items { get; set; }

        public GroupedSelectListItem()
        {
            Items = new List<SelectListItem>();
        }
    }
}