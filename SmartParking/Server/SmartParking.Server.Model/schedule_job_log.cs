namespace SmartParking.Server.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   

    [Table("schedule_job_log")]
    public partial class schedule_job_log
    {
        [Key]
        public long log_id { get; set; }

        public long job_id { get; set; }

        [StringLength(200)]
        public string bean_name { get; set; }

        [StringLength(100)]
        public string method_name { get; set; }

        [Column("params")]
        [StringLength(2000)]
        public string _params { get; set; }

        public sbyte status { get; set; }

        [StringLength(2000)]
        public string error { get; set; }

        public int times { get; set; }

        public DateTime? create_time { get; set; }
    }
}
