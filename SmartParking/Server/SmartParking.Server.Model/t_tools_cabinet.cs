namespace SmartParking.Server.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   

    [Table("t_tools_cabinet")]
    public partial class t_tools_cabinet
    {
        public long ID { get; set; }

        [StringLength(30)]
        public string REMOTE_ID { get; set; }

        [StringLength(50)]
        public string NAME { get; set; }

        [StringLength(50)]
        public string CODE { get; set; }

        [StringLength(10)]
        public string STATUS { get; set; }

        [StringLength(10)]
        public string TOKEN { get; set; }
    }
}
