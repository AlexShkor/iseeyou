using System;

namespace ISeeYou.Vk.Infrastructure
{
    public class VkResponseException : Exception
    {
        public VkResponseException(string message):base(message)
        {
            
        }
    }
}
