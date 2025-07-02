namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Urunler")]
    public partial class Urunler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Urunler()
        {
            AlisDetaylaris = new HashSet<AlisDetaylari>();
            DepoStoks = new HashSet<DepoStok>();
            SatisDetaylaris = new HashSet<SatisDetaylari>();
            StokHareketleris = new HashSet<StokHareketleri>();
            TeklifDetaylaris = new HashSet<TeklifDetaylari>();
            Uretims = new HashSet<Uretim>();
            UrunGorselleris = new HashSet<UrunGorselleri>();
            UrunVaryantlaris = new HashSet<UrunVaryantlari>();
            UrunPaketlemeveLojistikBilgiIeris = new HashSet<UrunPaketlemeveLojistikBilgiIeri>();
            ZeytinyagiUretimleris = new HashSet<ZeytinyagiUretimleri>();
            ZeytinyagiUretimleris1 = new HashSet<ZeytinyagiUretimleri>();
        }

        [Key]
        public int UrunID { get; set; }

        public int SirketID { get; set; }

        public int? MarkaID { get; set; }

        [StringLength(50)]
        public string UrunKodu { get; set; }

        public bool? UrunTipiStoklu { get; set; }

        [StringLength(50)]
        public string Barkod { get; set; }

        [Required]
        [StringLength(100)]
        public string UrunAdi { get; set; }

        public int? KategoriID { get; set; }

        public int? BirimID { get; set; }

        public decimal? StokMiktari { get; set; }

        public decimal? MinimumStok { get; set; }

        public decimal? AlisFiyati { get; set; }

        public int? AlisKdv { get; set; }

        public int? AlisParaBirimi { get; set; }

        public bool? AlisFiyatiKdvDahilmi { get; set; }

        public decimal? SatisFiyati { get; set; }

        public int? SatisKdv { get; set; }

        public int? SatisParaBirimi { get; set; }

        public bool? SatisFiyatiKdvDahilmi { get; set; }

        public int? ParaBirimiID { get; set; }

        public decimal? KDVOrani { get; set; }

        public bool? Durum { get; set; }

        public string Notlar { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        public bool? Mamul { get; set; }

        public bool? YariManul { get; set; }

        public decimal? PerakendeSatisFiyati { get; set; }

        public bool? PerakendeSatisKdvDahilmi { get; set; }

        public bool? PerakendeSatisVarmi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlisDetaylari> AlisDetaylaris { get; set; }

        public virtual Birimler Birimler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DepoStok> DepoStoks { get; set; }

        public virtual Markalar Markalar { get; set; }

        public virtual ParaBirimileri ParaBirimileri { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SatisDetaylari> SatisDetaylaris { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StokHareketleri> StokHareketleris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TeklifDetaylari> TeklifDetaylaris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Uretim> Uretims { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UrunGorselleri> UrunGorselleris { get; set; }

        public virtual UrunKategorileri UrunKategorileri { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UrunVaryantlari> UrunVaryantlaris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UrunPaketlemeveLojistikBilgiIeri> UrunPaketlemeveLojistikBilgiIeris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ZeytinyagiUretimleri> ZeytinyagiUretimleris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ZeytinyagiUretimleri> ZeytinyagiUretimleris1 { get; set; }
    }
}
