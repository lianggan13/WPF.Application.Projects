namespace SmartParking.Client.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("qrtz_paused_trigger_grps")]
    public partial class qrtz_paused_trigger_grps
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(120)]
        public string SCHED_NAME { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string TRIGGER_GROUP { get; set; }
    }
}
