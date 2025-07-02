namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StokHareketleri")]
    public partial class StokHareketleri
    {
        [Key]
        public int HareketID { get; set; }

        public int SirketID { get; set; }

        public DateTime IslemTarihi { get; set; }

        [Required]
        [StringLength(20)]
        public string HareketTipi { get; set; }

        public int DepoID { get; set; }

        public int UrunID { get; set; }

        public decimal Miktar { get; set; }

        [StringLength(50)]
        public string ReferansNo { get; set; }

        public int? ReferansID { get; set; }

        [StringLength(20)]
        public string ReferansTipi { get; set; }

        [StringLength(500)]
        public string Aciklama { get; set; }

        public int? KullaniciID { get; set; }

        public virtual Depolar Depolar { get; set; }

        public virtual Kullanicilar Kullanicilar { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        public virtual Urunler Urunler { get; set; }
    }
}
