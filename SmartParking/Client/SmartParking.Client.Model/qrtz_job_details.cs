namespace SmartParking.Client.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   

    [Table("qrtz_job_details")]
    public partial class qrtz_job_details
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public qrtz_job_details()
        {
            qrtz_triggers = new HashSet<qrtz_triggers>();
        }

        [Key]
        [Column(Order = 0)]
        [StringLength(120)]
        public string SCHED_NAME { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string JOB_NAME { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(200)]
        public string JOB_GROUP { get; set; }

        [StringLength(250)]
        public string DESCRIPTION { get; set; }

        [Required]
        [StringLength(250)]
        public string JOB_CLASS_NAME { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_DURABLE { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_NONCONCURRENT { get; set; }

        [Required]
        [StringLength(1)]
        public string IS_UPDATE_DATA { get; set; }

        [Required]
        [StringLength(1)]
        public string REQUESTS_RECOVERY { get; set; }

        [Column(TypeName = "blob")]
        public byte[] JOB_DATA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<qrtz_triggers> qrtz_triggers { get; set; }
    }
}
