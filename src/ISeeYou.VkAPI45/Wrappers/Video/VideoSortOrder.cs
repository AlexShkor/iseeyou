namespace VkAPIAsync.Wrappers.Videos
{
    public class VideoSortOrder
    {
        public enum VideoSortOrderEnum
        {
            ByDate = 0,
            ByDuration = 1,
            ByRelevancy = 2
        }

        public VideoSortOrder(VideoSortOrderEnum order)
        {
            Value = (int) order;
        }

        public int Value { get; set; }
    }
}