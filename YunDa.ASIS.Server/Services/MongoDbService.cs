using Microsoft.Extensions.Options;
using MongoDB.Driver;
using YunDa.ASIS.Server.Models;

namespace YunDa.ASIS.Server.Services
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;
    }

    public class MongoDbService
    {
        private static MongoDbService? instance;

        public static MongoDbService? Instance
        {
            get
            {
                if (instance == null)
                    instance = ServiceLocator.GetService<MongoDbService>();
                return instance;
            }
        }

        private readonly MongoClient? Client;
        private readonly IMongoDatabase? DataBase;

        public MongoDbService(IOptions<MongoDbSettings> options)
        {
            Client = new MongoClient(
              options.Value.ConnectionString);

            DataBase = Client.GetDatabase(
               options.Value.DatabaseName);


            UserColl = DataBase.GetCollection<User>(ASIS_User);
            RoleColl = DataBase.GetCollection<Role>(ASIS_Role);
            //var s = UserColl.Find(FilterDefinition<User>.Empty).CountDocuments();
        }

        public const string ASIS_User = "ASIS_User";
        public const string ASIS_Role = "ASIS_Role";


        public IMongoCollection<User> UserColl { get; set; }
        public IMongoCollection<Role> RoleColl { get; set; }

    }
}
