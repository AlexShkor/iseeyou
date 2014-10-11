﻿using Newtonsoft.Json.Linq;

namespace ISeeYou.Vk.Infrastructure
{
    public abstract class VkExtendedAttachment
    {
        public virtual string MediaType { get; private set; }

        public virtual JObject  GetValue()
        {
            return new JObject();
        }
    }
}