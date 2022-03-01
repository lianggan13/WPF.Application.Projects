namespace SmartParking.Server.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   

    [Table("sys_config")]
    public partial class sys_config
    {
        public long id { get; set; }

        [StringLength(50)]
        public string param_key { get; set; }

        [StringLength(2000)]
        public string param_value { get; set; }

        public sbyte? status { get; set; }

        [StringLength(500)]
        public string remark { get; set; }
    }
}
