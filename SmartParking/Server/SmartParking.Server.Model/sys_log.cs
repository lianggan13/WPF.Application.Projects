namespace SmartParking.Server.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   

    [Table("sys_log")]
    public partial class sys_log
    {
        public long id { get; set; }

        [StringLength(50)]
        public string api_name { get; set; }

        [StringLength(50)]
        public string info { get; set; }

        [Column("params")]
        [StringLength(5000)]
        public string _params { get; set; }

        public long time { get; set; }

        [StringLength(64)]
        public string ip { get; set; }

        [StringLength(50)]
        public string create_date { get; set; }
    }
}
