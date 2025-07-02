namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ZeytinyagiUretimleri")]
    public partial class ZeytinyagiUretimleri
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ZeytinyagiUretimleri()
        {
            ZeytinyagiUretimi_MalaksorKullanim = new HashSet<ZeytinyagiUretimi_MalaksorKullanim>();
            ZeytinyagiUretimi_ZeytinBoxKasa_Map = new HashSet<ZeytinyagiUretimi_ZeytinBoxKasa_Map>();
        }

        [Key]
        public int ZeytinyagiUretimID { get; set; }

        public int SirketID { get; set; }

        public int? MustahsilID { get; set; }

        [StringLength(50)]
        public string PartiNo { get; set; }

        public byte? PlakaNo { get; set; }

        public DateTime? GelisTarihi { get; set; }

        public int? GelisKg { get; set; }

        public int? UretimeAlinanKg { get; set; }

        public int UrunID { get; set; }

        public int? ZeytinyagiUretim_islemID { get; set; }

        public int? Operator_KullaniciID { get; set; }

        public DateTime? UretimBaslamaZamani { get; set; }

        public DateTime? UretimBitisZamani { get; set; }

        public int? Cikan_UrunID { get; set; }

        public int? TankID { get; set; }

        public decimal? Asidite { get; set; }

        public decimal? TahsiliyeKgUcreti { get; set; }

        public decimal? TahsiliyeToplamUcreti { get; set; }

        public virtual Kullanicilar Kullanicilar { get; set; }

        public virtual Mustahsiller Mustahsiller { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        public virtual Tanklar Tanklar { get; set; }

        public virtual Urunler Urunler { get; set; }

        public virtual Urunler Urunler1 { get; set; }

        public virtual ZeytinyagiUretim_islemleri ZeytinyagiUretim_islemleri { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ZeytinyagiUretimi_MalaksorKullanim> ZeytinyagiUretimi_MalaksorKullanim { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ZeytinyagiUretimi_ZeytinBoxKasa_Map> ZeytinyagiUretimi_ZeytinBoxKasa_Map { get; set; }
    }
}
