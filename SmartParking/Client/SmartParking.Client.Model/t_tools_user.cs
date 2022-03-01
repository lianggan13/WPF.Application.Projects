namespace SmartParking.Client.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   

    [Table("t_tools_user")]
    public partial class t_tools_user
    {
        public long ID { get; set; }

        [StringLength(20)]
        public string TOOLS_ID { get; set; }

        [StringLength(20)]
        public string USER_ID { get; set; }
    }
}
