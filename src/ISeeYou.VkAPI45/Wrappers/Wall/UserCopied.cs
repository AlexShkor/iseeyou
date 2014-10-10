#region Using

using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Wall
{
    public class UserCopied
    {
        public bool? Copied;
        public int? UserId;

        public UserCopied(XmlNode node)
        {
            UserId = node.Int("uid");
            Copied = node.Bool("copied");
        }
    }
}