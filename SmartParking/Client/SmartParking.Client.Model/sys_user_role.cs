namespace SmartParking.Client.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   

    [Table("sys_user_role")]
    public partial class sys_user_role
    {
        public long id { get; set; }

        public long? user_id { get; set; }

        public long? role_id { get; set; }
    }
}
