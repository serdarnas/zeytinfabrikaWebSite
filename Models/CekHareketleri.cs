namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CekHareketleri")]
    public partial class CekHareketleri
    {
        [Key]
        public long HareketID { get; set; }

        public int SirketID { get; set; }

        public int CekID { get; set; }

        public DateTime IslemTarihi { get; set; }

        public int IslemTipiID { get; set; }

        public int? IlgiliMusteriID { get; set; }

        public int? IlgiliTedarikciID { get; set; }

        public int? IlgiliFinansalKurumID { get; set; }

        public decimal Tutar { get; set; }

        [StringLength(1000)]
        public string Aciklama { get; set; }

        public virtual Cekler Cekler { get; set; }

        public virtual Musteriler Musteriler { get; set; }

        public virtual Tedarikciler Tedarikciler { get; set; }

        public virtual FinansalKurumlar FinansalKurumlar { get; set; }

        public virtual CekIslemTipleri CekIslemTipleri { get; set; }

        public virtual Sirketler Sirketler { get; set; }
    }
}
