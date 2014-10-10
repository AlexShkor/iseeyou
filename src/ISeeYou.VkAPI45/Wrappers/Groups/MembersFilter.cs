using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VkAPIAsync.Wrappers.Groups
{
    /// <summary>
    /// Фильтр участников сообщества
    /// </summary>
    public class MembersFilter
    {
        public enum MembersFilterEnum
        {
            /// <summary>
            /// Будут возвращены только друзья в этом сообществе.
            /// </summary>
            Friends,

            /// <summary>
            ///  Будут возвращены пользователи, которые выбрали «Возможно пойду» (если сообщество относится к мероприятиям).
            /// </summary>
            Unsure
        }

        public MembersFilter(MembersFilterEnum value)
        {
            switch (value)
            {
                case MembersFilterEnum.Friends:
                    Value = "friends";
                    break;
                case MembersFilterEnum.Unsure:
                    Value = "unsure";
                    break;
            }
        }

        public string Value { get; private set; }
    }
}
