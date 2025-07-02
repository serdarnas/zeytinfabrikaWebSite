namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TeklifDetaylari")]
    public partial class TeklifDetaylari
    {
        [Key]
        public int TeklifDetayID { get; set; }

        public int SirketID { get; set; }

        public int TeklifID { get; set; }

        public int UrunID { get; set; }

        public decimal Miktar { get; set; }

        public decimal BirimFiyat { get; set; }

        public decimal KDVOrani { get; set; }

        public decimal KDVTutari { get; set; }

        public decimal ToplamTutar { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        public virtual Teklifler Teklifler { get; set; }

        public virtual Urunler Urunler { get; set; }
    }
}
