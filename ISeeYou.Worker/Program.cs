using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISeeYou.Vk.Api;

namespace ISeeYou.Worker
{
    class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                var api = new VkApi(null);
                var result = api.GetPhotos(2409833,"profile");
            }
        }
    }
}
