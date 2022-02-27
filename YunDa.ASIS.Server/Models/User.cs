using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace YunDa.ASIS.Server.Models
{

    /// <summary>
    /// 描述一个用户
    /// </summary>
    [BsonIgnoreExtraElements]
    public class User : DynamicBson
    {
        /// <summary>
        /// 编号
        /// </summary>
        [BsonId]
        public int Id { get; set; }

        private int no;
        /// <summary>
        /// 获取或设置用户工号
        /// </summary>
        public int No
        {
            get { return no; }
            set
            {
                if (no != value)
                {
                    no = value;
                    UpdateProperty(value);
                }
            }
        }

        private int cardNo;
        /// <summary>
        /// 获取或设置用户卡号
        /// </summary>
        public int CardNo
        {
            get { return cardNo; }
            set
            {
                if (cardNo != value)
                {
                    cardNo = value;
                    UpdateProperty(value);
                }
            }
        }

        private string name;
        /// <summary>
        /// 获取或设置用户姓名
        /// </summary>
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

        /// <summary>
        /// 获取或设置用户照片(base64编码字符串)
        /// </summary>
        [JsonIgnore]
        public string Photo { get; set; }

        private string password;
        /// <summary>
        /// 获取或设置密码
        /// </summary>
        [JsonIgnore]
        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    UpdateProperty(value);
                }
            }
        }

        private int roleId;
        /// <summary>
        /// 获取或设置用户的角色编号
        /// </summary>
        public int RoleId
        {
            get { return roleId; }
            set
            {
                if (roleId != value)
                {
                    roleId = value;
                    UpdateProperty(value);
                }
            }
        }

        public Role? Role { get; set; }

        /// <summary>
        /// 外联表必须以集合 作为 接收
        /// </summary>
        public IEnumerable<Role>? Roles { get; set; }

        private int userGroupId;
        /// <summary>
        /// 获取或设置班组编号
        /// </summary>
        public int UserGroupId
        {
            get { return userGroupId; }
            set
            {
                if (userGroupId != value)
                {
                    userGroupId = value;
                    UpdateProperty(value);
                }
            }
        }


        private ObservableCollection<int> regionIds = new();
        [JsonIgnore]
        public ObservableCollection<int> RegionIds
        {
            get { return regionIds; }
            set
            {
                regionIds = value;
                UpdateProperty(value);
            }
        }

        private ObservableCollection<int> positionIds = new ObservableCollection<int>();
        [JsonIgnore]
        public ObservableCollection<int> PositionIds
        {
            get { return positionIds; }
            set
            {
                if (positionIds != value)
                {
                    positionIds = value;
                    UpdateProperty(value);
                }
            }
        }


        private ObservableCollection<int> keyIds = new ObservableCollection<int>();
        [JsonIgnore]
        public ObservableCollection<int> KeyIds
        {
            get { return keyIds; }
            set
            {
                if (keyIds != value)
                {
                    keyIds = value;
                    UpdateProperty(value);
                }
            }
        }


        private ObservableCollection<int> toolIds = new ObservableCollection<int>();
        [JsonIgnore]
        public ObservableCollection<int> ToolIds
        {
            get { return toolIds; }
            set
            {
                if (toolIds != value)
                {
                    toolIds = value;
                    UpdateProperty(value);
                }
            }
        }


        private ObservableCollection<int> illegalToolIds = new ObservableCollection<int>();
        [JsonIgnore]
        public ObservableCollection<int> IllegalToolIds
        {
            get { return illegalToolIds; }
            set
            {
                if (illegalToolIds != value)
                {
                    illegalToolIds = value;
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
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(i => i.Id, Id);
            FieldDefinition<User, T> field = propertyName;
            UpdateDefinition<User> update = Builders<User>.Update.Set(field, value);
            //DataProvider.Instance.GetAggregate<User>(DataProvider.UserCollectionName).UpdateOne(filter, update);
        }

        public DateTime? EntryTime { get; set; }
        public DateTime? OutOfTime { get; set; }

    }
}
