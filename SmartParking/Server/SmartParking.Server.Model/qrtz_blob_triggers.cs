namespace SmartParking.Server.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("qrtz_blob_triggers")]
    public partial class qrtz_blob_triggers
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(120)]
        public string SCHED_NAME { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string TRIGGER_NAME { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(200)]
        public string TRIGGER_GROUP { get; set; }

        [Column(TypeName = "blob")]
        public byte[] BLOB_DATA { get; set; }

        public virtual qrtz_triggers qrtz_triggers { get; set; }
    }
}
