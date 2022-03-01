namespace SmartParking.Server.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   

    [Table("sys_userface")]
    public partial class sys_userface
    {
        public int id { get; set; }

        [StringLength(50)]
        public string userid { get; set; }

        [StringLength(50)]
        public string face { get; set; }
    }
}
