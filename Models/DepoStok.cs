namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DepoStok")]
    public partial class DepoStok
    {
        public int DepoStokID { get; set; }

        public int SirketID { get; set; }

        public int DepoID { get; set; }

        public int UrunID { get; set; }

        public decimal Miktar { get; set; }

        public decimal? MinimumMiktar { get; set; }

        public DateTime? SonGuncellemeTarihi { get; set; }

        public virtual Depolar Depolar { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        public virtual Urunler Urunler { get; set; }
    }
}
