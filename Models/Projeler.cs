namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Projeler")]
    public partial class Projeler
    {
        [Key]
        public int ProjeID { get; set; }

        public int? SirketID { get; set; }

        [StringLength(150)]
        public string Ad { get; set; }

        public string Detay { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        public bool? Kapalimi { get; set; }

        public virtual Sirketler Sirketler { get; set; }
    }
}
