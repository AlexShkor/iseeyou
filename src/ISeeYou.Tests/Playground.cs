using ISeeYou.Vk.Api;
using NUnit.Framework;

namespace ISeeYou.Tests
{
    [TestFixture]
    public class Playground
    {
        [Test]
        public void test()
        {
            var api = new VkApi();
            var walls = api.GetWall(2409833);
            Assert.NotNull(walls);
        }
    }
}