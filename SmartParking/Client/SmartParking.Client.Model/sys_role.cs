namespace SmartParking.Client.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   

    [Table("sys_role")]
    public partial class sys_role
    {
        [Key]
        public long role_id { get; set; }

        [StringLength(100)]
        public string role_name { get; set; }

        [StringLength(100)]
        public string remark { get; set; }

        public long? create_user_id { get; set; }

        public DateTime? create_time { get; set; }

        [Required]
        [StringLength(10)]
        public string type { get; set; }
    }
}
