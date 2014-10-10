namespace VkAPIAsync.Wrappers.Common
{
    /// <summary>
    /// Сущность
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Идентификатор владельца
        /// </summary>
        public int? OwnerId { get; set; }
    }
}