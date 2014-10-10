#region Using

using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Wall
{
    public class PostSource
    {
        public enum VkDataEnum
        {
            ProfileActivity,
            ProfilePhoto,
            GroupDonation,
            WishlistAdd,
            WishlistBuy,
            MerchantBuy
        }

        public enum WidgetDataEnum
        {
            Comments,
            Like,
            Poll,
            Donation
        }

        public object Data;
        public PostSourceTypeEnum Type;

        public PostSource(XmlNode node)
        {
            var type = node.String("type");
            switch (type)
            {
                case "vk":
                    Type = PostSourceTypeEnum.Vk;
                    break;
                case "widget":
                    Type = PostSourceTypeEnum.Widget;
                    break;
                case "api":
                    Type = PostSourceTypeEnum.API;
                    break;
                case "rss":
                    Type = PostSourceTypeEnum.RSS;
                    break;
                case "sms":
                    Type = PostSourceTypeEnum.SMS;
                    break;
            }

            var data = node.SelectSingleNode("data");
            if (Type == PostSourceTypeEnum.Vk && data != null)
            {
                switch (data.InnerText)
                {
                    case "profile_activity":
                        Data = VkDataEnum.ProfileActivity;
                        break;
                    case "profile_photo":
                        Data = VkDataEnum.ProfilePhoto;
                        break;
                    case "group_donation":
                        Data = VkDataEnum.GroupDonation;
                        break;
                    case "wishlist_add":
                        Data = VkDataEnum.WishlistAdd;
                        break;
                    case "wishlist_buy":
                        Data = VkDataEnum.WishlistBuy;
                        break;
                    case "merchant_buy":
                        Data = VkDataEnum.MerchantBuy;
                        break;
                }
            }
            if (Type == PostSourceTypeEnum.Widget && data != null)
            {
                switch (data.InnerText)
                {
                    case "like":
                        Data = WidgetDataEnum.Like;
                        break;
                    case "comments":
                        Data = WidgetDataEnum.Comments;
                        break;
                    case "poll":
                        Data = WidgetDataEnum.Poll;
                        break;
                    case "donation":
                        Data = WidgetDataEnum.Donation;
                        break;
                }
            }
        }
    }

    public enum PostSourceTypeEnum
    {
        Vk,
        Widget,
        API,
        RSS,
        SMS
    }
}