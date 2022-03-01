namespace SmartParking.Client.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("t_tools_info")]
    public partial class t_tools_info
    {
        public long ID { get; set; }

        [StringLength(50)]
        public string CODE { get; set; }

        [StringLength(50)]
        public string TYPE { get; set; }

        [StringLength(50)]
        public string RFID { get; set; }

        [StringLength(10)]
        public string STATUS { get; set; }

        [StringLength(20)]
        public string CABINET_ID { get; set; }

        [StringLength(20)]
        public string REMOTE_ID { get; set; }
    }
}
