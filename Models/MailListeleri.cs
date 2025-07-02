namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MailListeleri")]
    public partial class MailListeleri
    {
        [Key]
        public int MailListeID { get; set; }

        public bool? Gonderildimi { get; set; }

        public DateTime? GonderimTarihSaat { get; set; }

        public int SirketID { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        [StringLength(150)]
        public string Mail_to { get; set; }

        [StringLength(250)]
        public string Mail_subject { get; set; }

        public string Mail_body { get; set; }

        public virtual Sirketler Sirketler { get; set; }
    }
}
