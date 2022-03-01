namespace SmartParking.Client.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   

    [Table("sys_userfinger")]
    public partial class sys_userfinger
    {
        public long ID { get; set; }

        public long? USERID { get; set; }

        [StringLength(32)]
        public string TYPE { get; set; }

        [StringLength(1024)]
        public string FINGER { get; set; }
    }
}
