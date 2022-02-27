using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace YunDa.ASIS.Server.Models
{
    [BsonIgnoreExtraElements]
    public class Role : DynamicBson
    {
        /// <summary>
        /// 序号
        /// </summary>
        [JsonProperty("Id"), BsonId]
        public int Id { get; set; }

        private string name;
        /// <summary>
        /// 获取或设置角色名称
        /// </summary>
        [JsonRequired]
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    UpdateProperty(value);
                }
            }
        }

        private List<int> functions;
        /// <summary>
        /// 获取或设置角色可用功能列表
        /// </summary>
        [JsonRequired]
        public List<int> Functions
        {
            get { return functions; }
            set
            {
                if (functions != value)
                {
                    functions = value;
                    UpdateProperty(value);
                }
            }
        }

        private string describe;
        /// <summary>
        /// 获取或设置描述信息
        /// </summary>
        [JsonRequired]
        public string Describe
        {
            get { return describe; }
            set
            {
                if (describe != value)
                {
                    describe = value;
                    UpdateProperty(value);
                }
            }
        }

        protected override void UpdateProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            return;
            if (!CanUpdate)
            {
                return;
            }
            FilterDefinition<Role> filter = Builders<Role>.Filter.Eq(i => i.Id, Id);
            FieldDefinition<Role, T> field = propertyName;
            UpdateDefinition<Role> update = Builders<Role>.Update.Set(field, value);
        }
    }
}
