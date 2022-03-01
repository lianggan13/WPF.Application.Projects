namespace SmartParking.Server.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   

    [Table("sys_request_log")]
    public partial class sys_request_log
    {
        public long ID { get; set; }

        [StringLength(50)]
        public string API_NAME { get; set; }

        [StringLength(20)]
        public string INFO { get; set; }

        [StringLength(500)]
        public string PARAMS { get; set; }

        [StringLength(500)]
        public string RESULT_INFO { get; set; }

        [StringLength(20)]
        public string TIME { get; set; }

        [StringLength(40)]
        public string CREATE_TIME { get; set; }

        public int? NUMBER { get; set; }

        [StringLength(20)]
        public string RESULT_CODE { get; set; }

        [StringLength(40)]
        public string UPDATE_TIME { get; set; }
    }
}
