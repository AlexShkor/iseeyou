﻿using System;
using Newtonsoft.Json.Serialization;

namespace ISeeYou.Web.Controllers.ActionResults
{
    public class LowerCamelCaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return String.Format("{0}{1}", propertyName.ToLower()[0], propertyName.Substring(1));
        }
    }
}