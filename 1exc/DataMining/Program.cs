using System;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.Linq;
using System.Text;
using DataMining.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.AudioBypassService;
using VkNet.AudioBypassService.Extensions;
using VkNet.Model.GroupUpdate;
using Microsoft.Extensions.Configuration;

namespace DataMining
{
    class Program
    {
        private static SQLiteConnection CreateConnection(string path)
        {
            var builder = (SQLiteConnectionStringBuilder)SQLiteProviderFactory.Instance.CreateConnectionStringBuilder();
            if (builder == null) return null;

            builder.DataSource = path;
            builder.FailIfMissing = false;
            return new SQLiteConnection(builder.ToString());
        }
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddAudioBypass(); 

            var api = new VkApi(services);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            api.Authorize(new ApiAuthParams()
            {
                ApplicationId = 7866166,
                Login = "+79503243190",
                Password = "16IDzaxar",
                Settings = Settings.All
            });


            Console.WriteLine("Группа:");
            int id = int.Parse(Console.ReadLine());
            id *= -1;
            if(id == 0)
            {
                Console.WriteLine("Пользователь");
                id = int.Parse(Console.ReadLine());
            }

            /*35488145 vk itis group id*/

            for (int i = 0; i <= 100; i += 100)
            {
                var itis_group = api.Wall.Get(new WallGetParams() { OwnerId = id, Offset = (ulong)i });
                using (var db = new WordDbContext())
                {
                    foreach (var post in itis_group.WallPosts)
                    {
                        foreach (var word in post.Text.Split(new char[] { ' ', ';', '.', '!', '?', '&', ',' }))
                        {
                            if (word != "" && word != " ")
                            {
                                Console.WriteLine(word.ToLower());
                                var word_to_update = db.Words.FirstOrDefault(w => w.Text.ToLower() == word.ToLower());
                                if (word_to_update is null)
                                {
                                    db.Words.Add(new Word() { Text = word.ToLower() });
                                }
                                else
                                {
                                    word_to_update.Count = word_to_update.Count + 1;
                                }

                                db.SaveChanges();
                            }
                        }
                    }
                    db.SaveChanges();
                }
            }

            /*var db = new WordDbContext();
            foreach (var word in db.Words)
            {
                Console.WriteLine(word.Id + "    " + word.Text + "    " + word.Count );
            }*/

        }
    }
}