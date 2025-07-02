namespace
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ZeytinModel : DbContext
    {
        public ZeytinModel()
            : base("name=ZeytinModel")
        {
        }

        public virtual DbSet<C__EFMigrationsHistory> C__EFMigrationsHistory { get; set; }
        public virtual DbSet<AlisDetaylari> AlisDetaylaris { get; set; }
        public virtual DbSet<Alislar> Alislars { get; set; }
        public virtual DbSet<BankaHesaplari> BankaHesaplaris { get; set; }
        public virtual DbSet<Birimler> Birimlers { get; set; }
        public virtual DbSet<BlogKategorileri> BlogKategorileris { get; set; }
        public virtual DbSet<BlogYazilari> BlogYazilaris { get; set; }
        public virtual DbSet<CekDurumlari> CekDurumlaris { get; set; }
        public virtual DbSet<CekHareketleri> CekHareketleris { get; set; }
        public virtual DbSet<CekIslemTipleri> CekIslemTipleris { get; set; }
        public virtual DbSet<Cekler> Ceklers { get; set; }
        public virtual DbSet<Depolar> Depolars { get; set; }
        public virtual DbSet<DepoStok> DepoStoks { get; set; }
        public virtual DbSet<Faturalar> Faturalars { get; set; }
        public virtual DbSet<Finan> Finans { get; set; }
        public virtual DbSet<FinansalKurumlar> FinansalKurumlars { get; set; }
        public virtual DbSet<Kullanicilar> Kullanicilars { get; set; }
        public virtual DbSet<KullaniciYetki> KullaniciYetkis { get; set; }
        public virtual DbSet<MailListeleri> MailListeleris { get; set; }
        public virtual DbSet<MalaksorBakimKayitlari> MalaksorBakimKayitlaris { get; set; }
        public virtual DbSet<Markalar> Markalars { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MustahsilCKSBelgeleri> MustahsilCKSBelgeleris { get; set; }
        public virtual DbSet<Mustahsiller> Mustahsillers { get; set; }
        public virtual DbSet<MustahsilTarlalar> MustahsilTarlalars { get; set; }
        public virtual DbSet<MusteriKategorileri> MusteriKategorileris { get; set; }
        public virtual DbSet<Musteriler> Musterilers { get; set; }
        public virtual DbSet<MusteriSubeler> MusteriSubelers { get; set; }
        public virtual DbSet<NakitIslemler> NakitIslemlers { get; set; }
        public virtual DbSet<PaletTipleri> PaletTipleris { get; set; }
        public virtual DbSet<ParaBirimileri> ParaBirimileris { get; set; }
        public virtual DbSet<Pazarlamacilar> Pazarlamacilars { get; set; }
        public virtual DbSet<Projeler> Projelers { get; set; }
        public virtual DbSet<SatisDetaylari> SatisDetaylaris { get; set; }
        public virtual DbSet<Satislar> Satislars { get; set; }
        public virtual DbSet<SenetDurumlari> SenetDurumlaris { get; set; }
        public virtual DbSet<SenetHareketleri> SenetHareketleris { get; set; }
        public virtual DbSet<SenetIslemTipleri> SenetIslemTipleris { get; set; }
        public virtual DbSet<Senetler> Senetlers { get; set; }
        public virtual DbSet<SirketAyarlari> SirketAyarlaris { get; set; }
        public virtual DbSet<Sirketler> Sirketlers { get; set; }
        public virtual DbSet<SirketZeytinyagiMakinaBakimDefteri> SirketZeytinyagiMakinaBakimDefteris { get; set; }
        public virtual DbSet<SirketZeytinyagiMakinalari> SirketZeytinyagiMakinalaris { get; set; }
        public virtual DbSet<SirketZeytinyagiMakinaMalaksorler> SirketZeytinyagiMakinaMalaksorlers { get; set; }
        public virtual DbSet<StokHareketleri> StokHareketleris { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Tanklar> Tanklars { get; set; }
        public virtual DbSet<Tedarikciler> Tedarikcilers { get; set; }
        public virtual DbSet<TeklifDetaylari> TeklifDetaylaris { get; set; }
        public virtual DbSet<Teklifler> Tekliflers { get; set; }
        public virtual DbSet<Uretim> Uretims { get; set; }
        public virtual DbSet<UrunGorselleri> UrunGorselleris { get; set; }
        public virtual DbSet<UrunKategorileri> UrunKategorileris { get; set; }
        public virtual DbSet<Urunler> Urunlers { get; set; }
        public virtual DbSet<UrunPaketlemeveLojistikBilgiIeri> UrunPaketlemeveLojistikBilgiIeris { get; set; }
        public virtual DbSet<UrunVaryantDetay> UrunVaryantDetays { get; set; }
        public virtual DbSet<UrunVaryantlari> UrunVaryantlaris { get; set; }
        public virtual DbSet<VaryantDegerleri> VaryantDegerleris { get; set; }
        public virtual DbSet<VaryantTurleri> VaryantTurleris { get; set; }
        public virtual DbSet<Web_iletisimFormu> Web_iletisimFormu { get; set; }
        public virtual DbSet<Yetkiler> Yetkilers { get; set; }
        public virtual DbSet<ZeytinBoxKasalari> ZeytinBoxKasalaris { get; set; }
        public virtual DbSet<ZeytinyagiMakinaMarkalari> ZeytinyagiMakinaMarkalaris { get; set; }
        public virtual DbSet<ZeytinyagiMakinaModelleri> ZeytinyagiMakinaModelleris { get; set; }
        public virtual DbSet<ZeytinyagiUretim_islemleri> ZeytinyagiUretim_islemleri { get; set; }
        public virtual DbSet<ZeytinyagiUretimi_MalaksorKullanim> ZeytinyagiUretimi_MalaksorKullanim { get; set; }
        public virtual DbSet<ZeytinyagiUretimi_ZeytinBoxKasa_Map> ZeytinyagiUretimi_ZeytinBoxKasa_Map { get; set; }
        public virtual DbSet<ZeytinyagiUretimleri> ZeytinyagiUretimleris { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        public virtual DbSet<VW_MalaksorKullanimDurumu> VW_MalaksorKullanimDurumu { get; set; }
        public virtual DbSet<VW_MalaksorPerformansRaporu> VW_MalaksorPerformansRaporu { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlisDetaylari>()
                .Property(e => e.Miktar)
                .HasPrecision(10, 2);

            modelBuilder.Entity<AlisDetaylari>()
                .Property(e => e.BirimFiyat)
                .HasPrecision(10, 2);

            modelBuilder.Entity<AlisDetaylari>()
                .Property(e => e.KDVOrani)
                .HasPrecision(5, 2);

            modelBuilder.Entity<AlisDetaylari>()
                .Property(e => e.KDVTutari)
                .HasPrecision(10, 2);

            modelBuilder.Entity<AlisDetaylari>()
                .Property(e => e.ToplamTutar)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Alislar>()
                .Property(e => e.ToplamTutar)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Alislar>()
                .Property(e => e.KDVToplam)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Alislar>()
                .Property(e => e.GenelToplam)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Alislar>()
                .HasMany(e => e.AlisDetaylaris)
                .WithRequired(e => e.Alislar)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BankaHesaplari>()
                .Property(e => e.Bakiye)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Birimler>()
                .HasMany(e => e.Uretims)
                .WithRequired(e => e.Birimler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CekDurumlari>()
                .HasMany(e => e.Ceklers)
                .WithRequired(e => e.CekDurumlari)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CekIslemTipleri>()
                .HasMany(e => e.CekHareketleris)
                .WithRequired(e => e.CekIslemTipleri)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cekler>()
                .HasMany(e => e.CekHareketleris)
                .WithRequired(e => e.Cekler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Depolar>()
                .Property(e => e.Kapasite)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Depolar>()
                .Property(e => e.DoluMiktar)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Depolar>()
                .HasMany(e => e.DepoStoks)
                .WithRequired(e => e.Depolar)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Depolar>()
                .HasMany(e => e.StokHareketleris)
                .WithRequired(e => e.Depolar)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DepoStok>()
                .Property(e => e.Miktar)
                .HasPrecision(10, 2);

            modelBuilder.Entity<DepoStok>()
                .Property(e => e.MinimumMiktar)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Faturalar>()
                .Property(e => e.ToplamTutar)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Faturalar>()
                .Property(e => e.KDVToplam)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Faturalar>()
                .Property(e => e.GenelToplam)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Finan>()
                .Property(e => e.Tutar)
                .HasPrecision(10, 2);

            modelBuilder.Entity<FinansalKurumlar>()
                .Property(e => e.KurumTipi)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<FinansalKurumlar>()
                .Property(e => e.Telefon)
                .IsUnicode(false);

            modelBuilder.Entity<FinansalKurumlar>()
                .HasMany(e => e.CekHareketleris)
                .WithOptional(e => e.FinansalKurumlar)
                .HasForeignKey(e => e.IlgiliFinansalKurumID);

            modelBuilder.Entity<FinansalKurumlar>()
                .HasMany(e => e.SenetHareketleris)
                .WithOptional(e => e.FinansalKurumlar)
                .HasForeignKey(e => e.IlgiliFinansalKurumID);

            modelBuilder.Entity<Kullanicilar>()
                .HasMany(e => e.BlogYazilaris)
                .WithOptional(e => e.Kullanicilar)
                .HasForeignKey(e => e.YazarID);

            modelBuilder.Entity<Kullanicilar>()
                .HasMany(e => e.ZeytinyagiUretimleris)
                .WithOptional(e => e.Kullanicilar)
                .HasForeignKey(e => e.Operator_KullaniciID);

            modelBuilder.Entity<KullaniciYetki>()
                .Property(e => e.YetkiKodu)
                .IsUnicode(false);

            modelBuilder.Entity<MalaksorBakimKayitlari>()
                .Property(e => e.Maliyet)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Menu>()
                .Property(e => e.YetkiKodu)
                .IsUnicode(false);

            modelBuilder.Entity<Menu>()
                .HasMany(e => e.Menu1)
                .WithOptional(e => e.Menu2)
                .HasForeignKey(e => e.UstMenuID);

            modelBuilder.Entity<Musteriler>()
                .HasMany(e => e.CekHareketleris)
                .WithOptional(e => e.Musteriler)
                .HasForeignKey(e => e.IlgiliMusteriID);

            modelBuilder.Entity<Musteriler>()
                .HasMany(e => e.Ceklers)
                .WithRequired(e => e.Musteriler)
                .HasForeignKey(e => e.AlinanMusteriID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Musteriler>()
                .HasMany(e => e.Faturalars)
                .WithRequired(e => e.Musteriler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Musteriler>()
                .HasMany(e => e.SenetHareketleris)
                .WithOptional(e => e.Musteriler)
                .HasForeignKey(e => e.IlgiliMusteriID);

            modelBuilder.Entity<Musteriler>()
                .HasMany(e => e.Senetlers)
                .WithOptional(e => e.Musteriler)
                .HasForeignKey(e => e.IlgiliMusteriID);

            modelBuilder.Entity<Musteriler>()
                .HasMany(e => e.MusteriSubelers)
                .WithRequired(e => e.Musteriler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Musteriler>()
                .HasMany(e => e.Satislars)
                .WithRequired(e => e.Musteriler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Musteriler>()
                .HasMany(e => e.Tekliflers)
                .WithRequired(e => e.Musteriler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NakitIslemler>()
                .Property(e => e.Tutar)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ParaBirimileri>()
                .Property(e => e.ParaBirimi)
                .IsFixedLength();

            modelBuilder.Entity<Pazarlamacilar>()
                .Property(e => e.Maaş)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SatisDetaylari>()
                .Property(e => e.BirimFiyat)
                .HasPrecision(10, 2);

            modelBuilder.Entity<SatisDetaylari>()
                .Property(e => e.indirimTutari)
                .HasPrecision(10, 2);

            modelBuilder.Entity<SatisDetaylari>()
                .Property(e => e.KDVTutari)
                .HasPrecision(10, 2);

            modelBuilder.Entity<SatisDetaylari>()
                .Property(e => e.ToplamTutar)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Satislar>()
                .Property(e => e.ToplamTutar)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Satislar>()
                .Property(e => e.indirimOrani)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Satislar>()
                .Property(e => e.indirimTutari)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Satislar>()
                .Property(e => e.KDVToplam)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Satislar>()
                .Property(e => e.GenelToplam)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Satislar>()
                .HasMany(e => e.SatisDetaylaris)
                .WithRequired(e => e.Satislar)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SenetDurumlari>()
                .HasMany(e => e.Senetlers)
                .WithRequired(e => e.SenetDurumlari)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SenetIslemTipleri>()
                .HasMany(e => e.SenetHareketleris)
                .WithRequired(e => e.SenetIslemTipleri)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Senetler>()
                .Property(e => e.SenetTipi)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Senetler>()
                .HasMany(e => e.SenetHareketleris)
                .WithRequired(e => e.Senetler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.AlisDetaylaris)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.Alislars)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.BankaHesaplaris)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.Birimlers)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.BlogKategorileris)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.BlogYazilaris)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.CekHareketleris)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.Ceklers)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.Depolars)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.DepoStoks)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.Faturalars)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.Finans)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.FinansalKurumlars)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.MailListeleris)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.Mustahsillers)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.MusteriKategorileris)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.Musterilers)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.MusteriSubelers)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.NakitIslemlers)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.Pazarlamacilars)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.SatisDetaylaris)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.Satislars)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.SenetHareketleris)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.Senetlers)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.SirketAyarlaris)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.SirketZeytinyagiMakinaBakimDefteris)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.SirketZeytinyagiMakinalaris)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.StokHareketleris)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.Tanklars)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.Tedarikcilers)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.TeklifDetaylaris)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.Tekliflers)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.Uretims)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.UrunGorselleris)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.UrunKategorileris)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.Urunlers)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.Yetkilers)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sirketler>()
                .HasMany(e => e.ZeytinyagiUretimleris)
                .WithRequired(e => e.Sirketler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SirketZeytinyagiMakinalari>()
                .HasMany(e => e.SirketZeytinyagiMakinaBakimDefteris)
                .WithRequired(e => e.SirketZeytinyagiMakinalari)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SirketZeytinyagiMakinaMalaksorler>()
                .HasMany(e => e.MalaksorBakimKayitlaris)
                .WithRequired(e => e.SirketZeytinyagiMakinaMalaksorler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SirketZeytinyagiMakinaMalaksorler>()
                .HasMany(e => e.ZeytinyagiUretimi_MalaksorKullanim)
                .WithRequired(e => e.SirketZeytinyagiMakinaMalaksorler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StokHareketleri>()
                .Property(e => e.Miktar)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Tanklar>()
                .Property(e => e.Kapasite)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Tanklar>()
                .Property(e => e.DoluMiktar)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Tedarikciler>()
                .HasMany(e => e.CekHareketleris)
                .WithOptional(e => e.Tedarikciler)
                .HasForeignKey(e => e.IlgiliTedarikciID);

            modelBuilder.Entity<Tedarikciler>()
                .HasMany(e => e.SenetHareketleris)
                .WithOptional(e => e.Tedarikciler)
                .HasForeignKey(e => e.IlgiliTedarikciID);

            modelBuilder.Entity<Tedarikciler>()
                .HasMany(e => e.Senetlers)
                .WithOptional(e => e.Tedarikciler)
                .HasForeignKey(e => e.IlgiliTedarikciID);

            modelBuilder.Entity<TeklifDetaylari>()
                .Property(e => e.Miktar)
                .HasPrecision(10, 2);

            modelBuilder.Entity<TeklifDetaylari>()
                .Property(e => e.BirimFiyat)
                .HasPrecision(10, 2);

            modelBuilder.Entity<TeklifDetaylari>()
                .Property(e => e.KDVOrani)
                .HasPrecision(5, 2);

            modelBuilder.Entity<TeklifDetaylari>()
                .Property(e => e.KDVTutari)
                .HasPrecision(10, 2);

            modelBuilder.Entity<TeklifDetaylari>()
                .Property(e => e.ToplamTutar)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Teklifler>()
                .Property(e => e.ToplamTutar)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Teklifler>()
                .Property(e => e.KDVToplam)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Teklifler>()
                .Property(e => e.GenelToplam)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Teklifler>()
                .HasMany(e => e.TeklifDetaylaris)
                .WithRequired(e => e.Teklifler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Uretim>()
                .Property(e => e.Miktar)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Urunler>()
                .Property(e => e.StokMiktari)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Urunler>()
                .Property(e => e.MinimumStok)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Urunler>()
                .Property(e => e.AlisFiyati)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Urunler>()
                .Property(e => e.SatisFiyati)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Urunler>()
                .Property(e => e.KDVOrani)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Urunler>()
                .HasMany(e => e.AlisDetaylaris)
                .WithRequired(e => e.Urunler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Urunler>()
                .HasMany(e => e.DepoStoks)
                .WithRequired(e => e.Urunler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Urunler>()
                .HasMany(e => e.SatisDetaylaris)
                .WithRequired(e => e.Urunler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Urunler>()
                .HasMany(e => e.StokHareketleris)
                .WithRequired(e => e.Urunler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Urunler>()
                .HasMany(e => e.TeklifDetaylaris)
                .WithRequired(e => e.Urunler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Urunler>()
                .HasMany(e => e.Uretims)
                .WithRequired(e => e.Urunler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Urunler>()
                .HasMany(e => e.UrunGorselleris)
                .WithRequired(e => e.Urunler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Urunler>()
                .HasMany(e => e.ZeytinyagiUretimleris)
                .WithRequired(e => e.Urunler)
                .HasForeignKey(e => e.UrunID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Urunler>()
                .HasMany(e => e.ZeytinyagiUretimleris1)
                .WithOptional(e => e.Urunler1)
                .HasForeignKey(e => e.Cikan_UrunID);

            modelBuilder.Entity<UrunPaketlemeveLojistikBilgiIeri>()
                .Property(e => e.KoliBarkodu)
                .IsFixedLength();

            modelBuilder.Entity<ZeytinBoxKasalari>()
                .HasMany(e => e.ZeytinyagiUretimi_ZeytinBoxKasa_Map)
                .WithRequired(e => e.ZeytinBoxKasalari)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ZeytinyagiMakinaModelleri>()
                .HasMany(e => e.SirketZeytinyagiMakinalaris)
                .WithRequired(e => e.ZeytinyagiMakinaModelleri)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ZeytinyagiUretimi_MalaksorKullanim>()
                .Property(e => e.KullanilanKg)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ZeytinyagiUretimi_MalaksorKullanim>()
                .Property(e => e.SicaklikOrtalama_C)
                .HasPrecision(5, 2);

            modelBuilder.Entity<ZeytinyagiUretimleri>()
                .Property(e => e.Asidite)
                .HasPrecision(8, 2);

            modelBuilder.Entity<ZeytinyagiUretimleri>()
                .Property(e => e.TahsiliyeKgUcreti)
                .HasPrecision(8, 2);

            modelBuilder.Entity<ZeytinyagiUretimleri>()
                .Property(e => e.TahsiliyeToplamUcreti)
                .HasPrecision(8, 2);

            modelBuilder.Entity<ZeytinyagiUretimleri>()
                .HasMany(e => e.ZeytinyagiUretimi_MalaksorKullanim)
                .WithRequired(e => e.ZeytinyagiUretimleri)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ZeytinyagiUretimleri>()
                .HasMany(e => e.ZeytinyagiUretimi_ZeytinBoxKasa_Map)
                .WithRequired(e => e.ZeytinyagiUretimleri)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VW_MalaksorKullanimDurumu>()
                .Property(e => e.Son30GunToplamKg)
                .HasPrecision(38, 2);

            modelBuilder.Entity<VW_MalaksorKullanimDurumu>()
                .Property(e => e.KullanimDurumu)
                .IsUnicode(false);

            modelBuilder.Entity<VW_MalaksorPerformansRaporu>()
                .Property(e => e.KullanilanKg)
                .HasPrecision(10, 2);

            modelBuilder.Entity<VW_MalaksorPerformansRaporu>()
                .Property(e => e.SicaklikOrtalama_C)
                .HasPrecision(5, 2);

            modelBuilder.Entity<VW_MalaksorPerformansRaporu>()
                .Property(e => e.SaatlikKapasite_kg)
                .HasPrecision(24, 13);

            modelBuilder.Entity<VW_MalaksorPerformansRaporu>()
                .Property(e => e.KapasiteKullanimi_Yuzde)
                .HasPrecision(25, 13);
        }
    }
}
