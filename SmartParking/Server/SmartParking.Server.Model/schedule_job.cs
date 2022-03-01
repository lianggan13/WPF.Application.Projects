namespace SmartParking.Server.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   

    [Table("schedule_job")]
    public partial class schedule_job
    {
        [Key]
        public long job_id { get; set; }

        [StringLength(200)]
        public string bean_name { get; set; }

        [StringLength(100)]
        public string method_name { get; set; }

        [Column("params")]
        [StringLength(2000)]
        public string _params { get; set; }

        [StringLength(100)]
        public string cron_expression { get; set; }

        public sbyte? status { get; set; }

        [StringLength(255)]
        public string remark { get; set; }

        public DateTime? create_time { get; set; }
    }
}
