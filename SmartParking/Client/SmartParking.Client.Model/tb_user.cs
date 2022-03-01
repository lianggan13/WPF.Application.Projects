namespace SmartParking.Client.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("tb_user")]
    public partial class tb_user
    {
        [Key]
        public long user_id { get; set; }

        [Required]
        [StringLength(50)]
        public string username { get; set; }

        [Required]
        [StringLength(20)]
        public string mobile { get; set; }

        [StringLength(64)]
        public string password { get; set; }

        public DateTime? create_time { get; set; }
    }
}
