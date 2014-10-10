using ISeeYou.Platform.Extensions;
using ISeeYou.Platform.ViewServices;

namespace ISeeYou.Platform.ViewModels
{
    public class BaseFilterModel<T> where T : BaseFilter, new()
    {
        public string OrderByKey { get; set; }
        public bool Desc { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; }

        public BaseFilterModel()
        {
            CurrentPage = 1;
            TotalPages = 1;
            ItemsPerPage = 10;
        }

        public virtual T ToFilter()
        {
            var filter = new T
            {
                PagingInfo = new PagingInfo() { CurrentPage = CurrentPage, ItemsPerPage = ItemsPerPage }
            };
            if (OrderByKey.HasValue())
            {
                foreach (var key in OrderByKey.Split(','))
                {
                    filter.AddOrder(key.Trim(), Desc);
                }
            }
            return filter;
        }

        public void UpdatePagingInfo(PagingInfo pagingInfo)
        {
            CurrentPage = pagingInfo.CurrentPage;
            TotalPages = pagingInfo.TotalPagesCount;
        }
    }
}