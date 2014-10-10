namespace VkAPIAsync.Wrappers.Audios
{
    public class AudioSortOrder
    {
        public enum AudioSortOrderEnum
        {
            ByPopularity = 2,
            ByDuration = 1,
            ByDate = 0
        }

        public AudioSortOrder(AudioSortOrderEnum order)
        {
            Value = (int) order;
        }

        public int Value { get; set; }
    }
}