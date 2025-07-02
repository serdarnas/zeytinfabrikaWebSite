namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ErrorLog
    {
        [Key]
        public int ErrorID { get; set; }

        public DateTime? ErrorDate { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }

        [StringLength(200)]
        public string PageUrl { get; set; }

        [StringLength(500)]
        public string UserAgent { get; set; }
    }
}
