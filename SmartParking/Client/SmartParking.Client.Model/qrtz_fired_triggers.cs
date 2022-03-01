namespace SmartParking.Client.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   

    [Table("qrtz_fired_triggers")]
    public partial class qrtz_fired_triggers
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(120)]
        public string SCHED_NAME { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(95)]
        public string ENTRY_ID { get; set; }

        [Required]
        [StringLength(200)]
        public string TRIGGER_NAME { get; set; }

        [Required]
        [StringLength(200)]
        public string TRIGGER_GROUP { get; set; }

        [Required]
        [StringLength(200)]
        public string INSTANCE_NAME { get; set; }

        public long FIRED_TIME { get; set; }

        public long SCHED_TIME { get; set; }

        public int PRIORITY { get; set; }

        [Required]
        [StringLength(16)]
        public string STATE { get; set; }

        [StringLength(200)]
        public string JOB_NAME { get; set; }

        [StringLength(200)]
        public string JOB_GROUP { get; set; }

        [StringLength(1)]
        public string IS_NONCONCURRENT { get; set; }

        [StringLength(1)]
        public string REQUESTS_RECOVERY { get; set; }
    }
}
