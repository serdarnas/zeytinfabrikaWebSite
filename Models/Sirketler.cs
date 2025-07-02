namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sirketler")]
    public partial class Sirketler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sirketler()
        {
            AlisDetaylaris = new HashSet<AlisDetaylari>();
            Alislars = new HashSet<Alislar>();
            BankaHesaplaris = new HashSet<BankaHesaplari>();
            Birimlers = new HashSet<Birimler>();
            BlogKategorileris = new HashSet<BlogKategorileri>();
            BlogYazilaris = new HashSet<BlogYazilari>();
            CekHareketleris = new HashSet<CekHareketleri>();
            Ceklers = new HashSet<Cekler>();
            Depolars = new HashSet<Depolar>();
            DepoStoks = new HashSet<DepoStok>();
            Faturalars = new HashSet<Faturalar>();
            Finans = new HashSet<Finan>();
            FinansalKurumlars = new HashSet<FinansalKurumlar>();
            MailListeleris = new HashSet<MailListeleri>();
            Markalars = new HashSet<Markalar>();
            Mustahsillers = new HashSet<Mustahsiller>();
            MusteriKategorileris = new HashSet<MusteriKategorileri>();
            Musterilers = new HashSet<Musteriler>();
            MusteriSubelers = new HashSet<MusteriSubeler>();
            NakitIslemlers = new HashSet<NakitIslemler>();
            Pazarlamacilars = new HashSet<Pazarlamacilar>();
            Projelers = new HashSet<Projeler>();
            SatisDetaylaris = new HashSet<SatisDetaylari>();
            Satislars = new HashSet<Satislar>();
            SenetHareketleris = new HashSet<SenetHareketleri>();
            Senetlers = new HashSet<Senetler>();
            SirketAyarlaris = new HashSet<SirketAyarlari>();
            SirketZeytinyagiMakinaBakimDefteris = new HashSet<SirketZeytinyagiMakinaBakimDefteri>();
            SirketZeytinyagiMakinalaris = new HashSet<SirketZeytinyagiMakinalari>();
            SirketZeytinyagiMakinaMalaksorlers = new HashSet<SirketZeytinyagiMakinaMalaksorler>();
            StokHareketleris = new HashSet<StokHareketleri>();
            Tanklars = new HashSet<Tanklar>();
            Tedarikcilers = new HashSet<Tedarikciler>();
            TeklifDetaylaris = new HashSet<TeklifDetaylari>();
            Tekliflers = new HashSet<Teklifler>();
            Uretims = new HashSet<Uretim>();
            UrunGorselleris = new HashSet<UrunGorselleri>();
            UrunKategorileris = new HashSet<UrunKategorileri>();
            Urunlers = new HashSet<Urunler>();
            UrunPaketlemeveLojistikBilgiIeris = new HashSet<UrunPaketlemeveLojistikBilgiIeri>();
            UrunVaryantlaris = new HashSet<UrunVaryantlari>();
            VaryantDegerleris = new HashSet<VaryantDegerleri>();
            VaryantTurleris = new HashSet<VaryantTurleri>();
            Yetkilers = new HashSet<Yetkiler>();
            ZeytinBoxKasalaris = new HashSet<ZeytinBoxKasalari>();
            ZeytinyagiUretimleris = new HashSet<ZeytinyagiUretimleri>();
        }

        [Key]
        public int SirketID { get; set; }

        [Required]
        [StringLength(100)]
        public string SirketAdi { get; set; }

        [StringLength(50)]
        public string VergiDairesi { get; set; }

        [StringLength(20)]
        public string VergiNo { get; set; }

        [StringLength(200)]
        public string Adres { get; set; }

        [StringLength(20)]
        public string Telefon { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(200)]
        public string LogoURL { get; set; }

        public bool? Aktif { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlisDetaylari> AlisDetaylaris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Alislar> Alislars { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BankaHesaplari> BankaHesaplaris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Birimler> Birimlers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlogKategorileri> BlogKategorileris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlogYazilari> BlogYazilaris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CekHareketleri> CekHareketleris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cekler> Ceklers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Depolar> Depolars { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DepoStok> DepoStoks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Faturalar> Faturalars { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Finan> Finans { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FinansalKurumlar> FinansalKurumlars { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MailListeleri> MailListeleris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Markalar> Markalars { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mustahsiller> Mustahsillers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MusteriKategorileri> MusteriKategorileris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Musteriler> Musterilers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MusteriSubeler> MusteriSubelers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NakitIslemler> NakitIslemlers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pazarlamacilar> Pazarlamacilars { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Projeler> Projelers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SatisDetaylari> SatisDetaylaris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Satislar> Satislars { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SenetHareketleri> SenetHareketleris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Senetler> Senetlers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SirketAyarlari> SirketAyarlaris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SirketZeytinyagiMakinaBakimDefteri> SirketZeytinyagiMakinaBakimDefteris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SirketZeytinyagiMakinalari> SirketZeytinyagiMakinalaris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SirketZeytinyagiMakinaMalaksorler> SirketZeytinyagiMakinaMalaksorlers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StokHareketleri> StokHareketleris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tanklar> Tanklars { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tedarikciler> Tedarikcilers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TeklifDetaylari> TeklifDetaylaris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Teklifler> Tekliflers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Uretim> Uretims { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UrunGorselleri> UrunGorselleris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UrunKategorileri> UrunKategorileris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Urunler> Urunlers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UrunPaketlemeveLojistikBilgiIeri> UrunPaketlemeveLojistikBilgiIeris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UrunVaryantlari> UrunVaryantlaris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VaryantDegerleri> VaryantDegerleris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VaryantTurleri> VaryantTurleris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Yetkiler> Yetkilers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ZeytinBoxKasalari> ZeytinBoxKasalaris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ZeytinyagiUretimleri> ZeytinyagiUretimleris { get; set; }
    }
}
