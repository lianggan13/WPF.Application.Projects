namespace SmartParking.Server.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   

    [Table("sys_menu")]
    public partial class sys_menu
    {
        [Key]
        public long menu_id { get; set; }

        public long? parent_id { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        [StringLength(200)]
        public string url { get; set; }

        [StringLength(500)]
        public string perms { get; set; }

        public int? type { get; set; }

        [StringLength(50)]
        public string icon { get; set; }

        public int? order_num { get; set; }
    }
}
