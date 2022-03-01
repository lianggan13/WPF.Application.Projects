namespace SmartParking.Client.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("sys_user")]
    public partial class sys_user
    {
        [Key]
        public long user_id { get; set; }

        [StringLength(50)]
        public string username { get; set; }

        [StringLength(255)]
        public string nickname { get; set; }

        [StringLength(100)]
        public string password { get; set; }

        [StringLength(50)]
        public string remote_id { get; set; }

        [StringLength(20)]
        public string salt { get; set; }

        [StringLength(50)]
        public string idcard { get; set; }

        [StringLength(100)]
        public string email { get; set; }

        [StringLength(100)]
        public string mobile { get; set; }

        public sbyte? status { get; set; }

        public long? create_user_id { get; set; }

        public DateTime? create_time { get; set; }

        [StringLength(50)]
        public string staff_code { get; set; }
    }
}
