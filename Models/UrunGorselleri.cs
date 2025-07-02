namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UrunGorselleri")]
    public partial class UrunGorselleri
    {
        [Key]
        public int UrunGorselID { get; set; }

        public int UrunID { get; set; }

        public int SirketID { get; set; }

        public int? UrunVaryantID { get; set; }

        [StringLength(250)]
        public string ResimYol { get; set; }

        public bool? Kapak { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        public virtual Urunler Urunler { get; set; }
    }
}
