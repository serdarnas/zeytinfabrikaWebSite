namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SatisDetaylari")]
    public partial class SatisDetaylari
    {
        [Key]
        public int SatisDetayID { get; set; }

        public int SirketID { get; set; }

        public int SatisID { get; set; }

        public int UrunID { get; set; }

        public int Miktar { get; set; }

        public decimal BirimFiyat { get; set; }

        public int? indirimOrani { get; set; }

        public decimal? indirimTutari { get; set; }

        public int KDVOrani { get; set; }

        public decimal KDVTutari { get; set; }

        public decimal ToplamTutar { get; set; }

        public int DepoID { get; set; }

        public virtual Satislar Satislar { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        public virtual Urunler Urunler { get; set; }
    }
}
