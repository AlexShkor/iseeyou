namespace VkAPIAsync.Wrappers.Account
{
    /// <summary>
    /// Сервис для поиска по контактам
    /// </summary>
    public class ContactService
    {
        public enum ContactServiceEnum
        {
            Email,
            Phone,
            Twitter,
            Facebook,
            Odnoklassniki,
            Instagram,
            Google
        }

        public ContactService(ContactServiceEnum e)
        {
            StringValue = e.ToString("G").ToLower();
        }

        public string StringValue { get; set; }
    }
}
