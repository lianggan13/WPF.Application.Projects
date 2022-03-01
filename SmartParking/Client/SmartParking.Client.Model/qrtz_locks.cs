namespace SmartParking.Client.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("qrtz_locks")]
    public partial class qrtz_locks
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(120)]
        public string SCHED_NAME { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(40)]
        public string LOCK_NAME { get; set; }
    }
}
