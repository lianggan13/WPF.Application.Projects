using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace YunDa.ASIS.Server.Models
{

    /// <summary>
    /// 描述一个用户
    /// </summary>
    [BsonIgnoreExtraElements]
    public class User : DynamicBson
    {
        /// <summary>
        /// 序号
        /// </summary>
        [JsonProperty("Id")]
        public int ID { get; set; }

        /// <summary>
        /// 获取或设置用户工号
        /// </summary>
        public int No { get; set; }

        /// <summary>
        /// 获取或设置用户卡号
        /// </summary>
        public int? CardNo { get; set; }

        /// <summary>
        /// 获取或设置用户名称
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 获取或设置用户照片
        /// </summary>
        [JsonIgnore]
        public string Photo { get; set; } = null!;

        /// <summary>
        /// 获取用户的照片地址
        /// </summary>
        [BsonIgnore]
        public string PhotoUrl { get; set; } = null!;

        /// <summary>
        /// 获取或设置用户的登录密码
        /// </summary>
        [JsonIgnore]
        public string Password { get; set; } = null!;

        /// <summary>
        /// 获取或设置用户设置的旧密码
        /// </summary>
        [BsonIgnore]
        public string OldPassword { get; set; } = null!;

        /// <summary>
        /// 获取或设置用户设置的新密码
        /// </summary>
        [BsonIgnore]
        public string NewPassword { get; set; } = null!;

        /// <summary>
        /// 获取或设置用户的角色编号
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 是否允许更新，该用户下发权限后不允许更新
        /// </summary>
        public bool AllowUpdate { get; set; }

        /// <summary>
        /// 获取或设置用户班组编号
        /// </summary>
        public int UserGroupId { get; set; }

        //[BsonIgnore]
        //[JsonIgnore]
        //public IssueType IssueType { get; set; }

        public override bool Equals(object? obj)
        {
            if (!(obj is User anohter))
            {
                return false;
            }
            return this.ID == anohter.ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public SimpleUser ToSimpleUser()
        {
            return new SimpleUser()
            {
                Id = ID,
                No = No,
                CardNo = CardNo,
                Name = Name,
                UserGroupId = UserGroupId
            };
        }

        protected override void UpdateProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            throw new NotImplementedException();
        }


    }

    public class SimpleUser
    {
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置用户工号
        /// </summary>
        public int No { get; set; }

        /// <summary>
        /// 获取或设置用户卡号
        /// </summary>
        public int? CardNo { get; set; }

        /// <summary>
        /// 获取或设置用户姓名
        /// </summary>
        public string Name { get; set; }

        public int UserGroupId { get; set; }

        //public string UserGroup
        //{
        //    get
        //    {
        //        UserGroup group = DataProvider.Instance.UserGroupList.FirstOrDefault(i => i.ID == UserGroupId);
        //        return group?.Name;
        //    }
        //}
    }

}
