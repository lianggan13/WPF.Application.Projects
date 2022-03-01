namespace SmartParking.Server.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   

    [Table("sys_fingervalidate")]
    public partial class sys_fingervalidate
    {
        public int id { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        [StringLength(50)]
        public string code { get; set; }

        [StringLength(50)]
        public string type { get; set; }

        [StringLength(50)]
        public string roleid { get; set; }
    }
}
