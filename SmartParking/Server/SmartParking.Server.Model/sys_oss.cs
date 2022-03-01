namespace SmartParking.Server.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   

    [Table("sys_oss")]
    public partial class sys_oss
    {
        public long id { get; set; }

        [StringLength(200)]
        public string url { get; set; }

        public DateTime? create_date { get; set; }
    }
}
