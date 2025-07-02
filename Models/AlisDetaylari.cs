namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AlisDetaylari")]
    public partial class AlisDetaylari
    {
        [Key]
        public int AlisDetayID { get; set; }

        public int SirketID { get; set; }

        public int AlisID { get; set; }

        public int UrunID { get; set; }

        public decimal Miktar { get; set; }

        public decimal BirimFiyat { get; set; }

        public decimal KDVOrani { get; set; }

        public decimal KDVTutari { get; set; }

        public decimal ToplamTutar { get; set; }

        public virtual Alislar Alislar { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        public virtual Urunler Urunler { get; set; }
    }
}
