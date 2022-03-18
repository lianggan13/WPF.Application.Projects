using MongoDB.Bson;
using MongoDB.Driver;
using YunDa.ASIS.Server.Models;


namespace YunDa.ASIS.Server.Test
{
    public class MongoDBTest
    {
        private readonly IMongoDatabase db;
        private readonly IMongoCollection<User> users;
        private readonly IMongoCollection<Role> roles;
        private readonly string DP_User = nameof(DP_User);
        private readonly string DP_Role = nameof(DP_Role);

        public MongoDBTest(string connStr
            = "mongodb://localhost:27017", string dbName = "Shen12DP")
        {
            MongoClient client = new MongoClient(connStr);
            this.db = client.GetDatabase(dbName);
            this.users = db.GetCollection<User>(DP_User);
            this.roles = db.GetCollection<Role>(DP_Role);
        }

        public void TestAdd()
        {
            //.Should().Be(1);
            // InsertOne
            {
                int id = (int)users.CountDocuments(u => true) + 1;
                var user = new User
                {
                    ID = id,
                    No = id,
                    Name = "zhangliang",
                };

                users.InsertOne(user);
            }
            // InsertMany
            {
                IMongoCollection<BsonDocument> col = db.GetCollection<BsonDocument>("NewDocument");
                IEnumerable<BsonDocument> doc = new[]
                {
                    new BsonDocument{
                        { "DepartmentName","开发部"},
                        { "People",new  BsonArray
                            {
                                new BsonDocument{ { "Name", "狗娃" },{"Age",20 } },
                                new BsonDocument{ { "Name", "狗剩" },{"Age",22 } },
                                new BsonDocument{ { "Name", "铁蛋" },{"Age",24 } }
                            }
                        },
                        {"Sum",18 },
                        { "dim_cm", new BsonArray { 14, 21 } }
                    },
                    new BsonDocument{
                        { "DepartmentName","测试部"},
                        { "People",new  BsonArray
                            {
                                new BsonDocument{ { "Name", "张三" },{"Age",11 } },
                                new BsonDocument{ { "Name", "李四" },{"Age",34 } },
                                new BsonDocument{ { "Name", "王五" },{"Age",33 } }
                            }
                        },
                        { "Sum",4 },
                        { "dim_cm", new BsonArray { 14, 21 } }
                    },
                    new BsonDocument{
                        { "DepartmentName","运维部"},
                        { "People",new  BsonArray
                            {
                                new BsonDocument{ { "Name", "闫" },{"Age",20 } },
                                new BsonDocument{ { "Name", "王" },{"Age",22 } },
                                new BsonDocument{ { "Name", "赵" },{"Age",24 } }
                            }
                        },
                        { "Sum",2 },
                        { "dim_cm", new BsonArray { 22.85, 30 } }
                    }
                };
                col.InsertMany(doc);
            }
        }

        public void TestDelete()
        {
            {
                var filter = Builders<User>.Filter.Eq("Name", "zhangliang");
                users.DeleteOne(filter);
            }
        }

        public void TestUpdate()
        {

            //var col = GetAggregate<OperationRecordEx>(OperationRecord);
            //if (data.Id == 0)
            //{
            //    long n = DataBase.GetCount<OperationRecordEx>(OperationRecord);
            //    data.Id = ++n;
            //}
            //ReplaceOptions opt = new() { IsUpsert = true };
            //col.ReplaceOne(i => i.Id == data.Id, data, opt);

            // UpdateOne
            {
                var filter = Builders<User>.Filter.Eq("Name", "zhangliang");
                var update = Builders<User>.Update.Set("Password", "yunda123").Set(u => u.Name, "lianggan13");
                users.UpdateOne(filter, update);
            }
            // UpdateMany
            {
                var builder = Builders<User>.Filter;
                var filter = builder.Eq("Name", "zhangliang") | builder.Eq("Name", "liumei");
                var update = Builders<User>.Update.Set("Password", "yunda123");
                users.UpdateMany(filter, update);
            }
            //var builder = Builders<Widget>.Update;
            //var update = builder.Set(widget => widget.X, 1).Set(widget => widget.Y, 3).Inc(widget => widget.Z, 1);
        }

        public void TestFind()
        {
            {
                var filter = Builders<User>.Filter.Gte("No", 13);
                var list = users.Find(filter).ToList();
            }

            {
                var builder = Builders<User>.Filter;
                //var filter1 = builder.Lte("No", 13);
                var filter1 = builder.Lte(u => u.No, 13);
                var filter2 = builder.Empty;
                var filter = builder.And(filter1, filter2); //  var filter = filter1 & filter2;
                var list = users.Find(filter).ToEnumerable();
            }

            {
                var builder = Builders<User>.Filter;
                var filter = builder.In(u => u.No, new[] { 13, 123456 });
                var list = users.Find(filter).ToList();
            }

            //{
            //    var builder = Builders<User>.Filter;
            //    var filter = builder.Gte(u => u.KeyIds.Count, 1);
            //    var list = users.Find(filter).ToList();
            //}
        }


        public void TestMatch()
        {
            // {   { "$match" : { "No" : { "$in" : [13, 123456] } } }   }
            {
                var builder = Builders<User>.Filter;
                var filter = builder.In(u => u.No, new[] { 13, 123456 });
                var pipeLine = PipelineStageDefinitionBuilder.Match(filter);
                var list = users.Aggregate().AppendStage(pipeLine).ToList();
            }

            {
                var doc = BsonDocument.Parse("{No: {$in: [13, 123456]}}");
                var list = users.Aggregate().Match(doc).ToList();
            }

            {
                var doc = BsonDocument.Parse("{$match: {No: {$in: [13, 123456]}}}");
                var pipeLine = new BsonDocumentPipelineStageDefinition<User, User>(doc);
                var list = users.Aggregate().AppendStage(pipeLine).ToList();
            }

            {
                PipelineDefinition<User, User> pipeline = new BsonDocument[]
                {
                      new BsonDocument { {"$match", new BsonDocument{ { "No", new BsonDocument{ {"$in", new BsonArray { 13,123456}}, } } } } },
                      new BsonDocument { { "$sort", new BsonDocument("No", 1) } }
                };
                var list = users.Aggregate(pipeline).ToList();
            }

        }

        public void TestProjection()
        {
            var builder = Builders<User>.Projection;
            var projection = builder.Include(u => u.Name).Exclude(u => u.ID);
            var list = users.Find(u => true).Project(projection).ToEnumerable().Select(doc => doc.AsBsonValue).ToList();
        }

        public void TestSort()
        {
            {
                var sort = Builders<User>.Sort.Descending(u => u.No);
                var list = users.Find(u => true).Sort(sort).ToEnumerable();
            }
        }

        public void TestGroup()
        {
            {
                //// 按照 RoleId 分组，统计每组的 人数 和 钥匙数量 
                //var list = users.Aggregate().Group(u => new { u.RoleId }, g => new { Name = g.Key, Count = g.Sum(u => 1), KeyNums = g.Sum(u => u.KeyIds.Count) }).ToList();

                //var list2 = (from u in users.AsQueryable()
                //             group u by u.RoleId
                //             into g
                //             select new
                //             {
                //                 Name = g.Key,
                //                 Count = g.Sum(u => 1),
                //                 KeyNums = g.Sum(u => u.KeyIds.Count)
                //             }).ToList();
            }
        }

        public void TestJoin()
        {
            //{
            //    FieldDefinition<User> localField = nameof(User.RoleId);
            //    FieldDefinition<Role> foreignField = nameof(Role.Id);
            //    FieldDefinition<User> @as = nameof(User.Roles);
            //    var user_roles = users.Aggregate().Lookup<Role, User>(DP_Role, localField, foreignField, @as).ToList();

            //    var builder = Builders<User>.Projection;
            //    var projection = builder.Exclude(u => u.Role).Exclude(u => u.Roles);

            //    int id = (int)users.CountDocuments(u => true) + 1;
            //    InsertOneOptions options = new InsertOneOptions();
            //    //options.
            //    user_roles[0].ID = id;
            //    users.InsertOne(user_roles[0]);
            //}

            //{
            //    var urs = users.AsQueryable().GroupJoin(roles.AsQueryable(), u => u.RoleId, r => r.Id,
            //          (u, gr) => new { u, gr }).ToList();
            //    var list = urs.Select(l =>
            //    {
            //        l.u.Roles = l.gr;
            //        return l.u;
            //    });
            //}

            //{
            //    var urs = (from u in users.AsQueryable()
            //               join r in roles.AsQueryable()
            //               on u.RoleId equals r.Id
            //               into gr    // 增加 into 变为 left join
            //               select new
            //               {
            //                   u,
            //                   gr,
            //               }).ToList();
            //    var list = urs.Select(l =>
            //    {
            //        l.u.Roles = l.gr;
            //        return l.u;
            //    });
            //}

        }
    }
}
