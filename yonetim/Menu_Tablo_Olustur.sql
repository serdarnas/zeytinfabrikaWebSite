-- Menu tablosunu oluştur (eğer yoksa)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Menu]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Menu](
        [MenuID] [int] IDENTITY(1,1) NOT NULL,
        [UstMenuID] [int] NULL,
        [MenuAdi] [nvarchar](100) NOT NULL,
        [Ikon] [nvarchar](50) NULL,
        [SayfaURL] [nvarchar](250) NULL,
        [Sira] [int] NOT NULL,
        [YetkiKodu] [nvarchar](50) NOT NULL,
        CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
        (
            [MenuID] ASC
        )
    )
    
    -- Üst menüye referans ekle
    ALTER TABLE [dbo].[Menu] ADD CONSTRAINT [FK_Menu_Menu] 
    FOREIGN KEY ([UstMenuID]) REFERENCES [dbo].[Menu] ([MenuID])
    
    PRINT 'Menu tablosu oluşturuldu.'
END
ELSE
BEGIN
    PRINT 'Menu tablosu zaten var.'
END

-- Örnek menu verileri
-- Önce tabloyu temizle
DELETE FROM [dbo].[KullaniciYetki] WHERE YetkiKodu IN (
    SELECT YetkiKodu FROM [dbo].[Menu]
)
DELETE FROM [dbo].[Menu]
DBCC CHECKIDENT ('[dbo].[Menu]', RESEED, 0)

-- Ana menüler
SET IDENTITY_INSERT [dbo].[Menu] ON

-- Anasayfa
INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (1, NULL, N'Anasayfa', N'fa-home', N'/fabrika/Default.aspx', 10, N'Anasayfa')

-- Fatura İşlemleri
INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (2, NULL, N'Fatura İşlemleri', N'fa-file-text-o', NULL, 20, N'FaturaIslemleri')

-- Tahsilat/Ödeme
INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (3, NULL, N'Tahsilat/Ödeme', N'fa-money', NULL, 30, N'TahsilatOdeme')

-- Müşteri/Tedarikçi
INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (4, NULL, N'Müşteri/Tedarikçi', N'fa-users', NULL, 40, N'MusteriTedarikci')

-- Stok İşlemleri
INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (5, NULL, N'Stok İşlemleri', N'fa-cubes', NULL, 50, N'StokIslemleri')

-- Raporlar
INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (6, NULL, N'Raporlar', N'fa-bar-chart-o', NULL, 60, N'Raporlar')

-- Sistem
INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (7, NULL, N'Sistem', N'fa-cogs', NULL, 70, N'Sistem')

-- Alt menüler - Fatura İşlemleri
INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (8, 2, N'Yeni Fatura', N'fa-plus-square', N'/fabrika/Fatura/YeniFatura.aspx', 10, N'FaturaYeniFatura')

INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (9, 2, N'Fatura Listesi', N'fa-list', N'/fabrika/Fatura/FaturaListesi.aspx', 20, N'FaturaListesi')

-- Alt menüler - Tahsilat/Ödeme
INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (10, 3, N'Yeni Tahsilat', N'fa-plus-square', N'/fabrika/Tahsilat/YeniTahsilat.aspx', 10, N'TahsilatYeniTahsilat')

INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (11, 3, N'Tahsilat Listesi', N'fa-list', N'/fabrika/Tahsilat/TahsilatListesi.aspx', 20, N'TahsilatListesi')

-- Alt menüler - Müşteri/Tedarikçi
INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (12, 4, N'Yeni Müşteri', N'fa-user-plus', N'/fabrika/Musteri/YeniMusteri.aspx', 10, N'MusteriYeniMusteri')

INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (13, 4, N'Müşteri Listesi', N'fa-list', N'/fabrika/Musteri/MusteriListesi.aspx', 20, N'MusteriListesi')

INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (14, 4, N'Yeni Mustahsil', N'fa-leaf', N'/fabrika/Musteri/YeniMustahsil.aspx', 30, N'MusteriYeniMustahsil')

INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (15, 4, N'Mustahsil Listesi', N'fa-list', N'/fabrika/Musteri/MustahsilListesi.aspx', 40, N'MusteriMustahsilListesi')

-- Alt menüler - Stok İşlemleri
INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (16, 5, N'Yeni Ürün', N'fa-plus-square', N'/fabrika/Stok/YeniUrun.aspx', 10, N'StokYeniUrun')

INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (17, 5, N'Ürün Listesi', N'fa-list', N'/fabrika/Stok/UrunListesi.aspx', 20, N'StokUrunListesi')

INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (18, 5, N'Stok Hareketleri', N'fa-exchange', N'/fabrika/Stok/StokHareketleri.aspx', 30, N'StokHareketleri')

-- Alt menüler - Raporlar
INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (19, 6, N'Satış Raporları', N'fa-line-chart', N'/fabrika/Rapor/SatisRaporlari.aspx', 10, N'RaporSatisRaporlari')

INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (20, 6, N'Cari Raporlar', N'fa-pie-chart', N'/fabrika/Rapor/CariRaporlar.aspx', 20, N'RaporCariRaporlar')

INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (21, 6, N'Stok Raporları', N'fa-bar-chart', N'/fabrika/Rapor/StokRaporlari.aspx', 30, N'RaporStokRaporlari')

-- Alt menüler - Sistem
INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (22, 7, N'Kullanıcı Yönetimi', N'fa-users', N'/fabrika/Kullanici/YeniKullanici.aspx', 10, N'SistemKullaniciYonetimi')

INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (23, 7, N'Menü Yönetimi', N'fa-list', N'/yonetim/MenuYonetimi.aspx', 20, N'SistemMenuYonetimi')

INSERT [dbo].[Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (24, 7, N'Sistem Ayarları', N'fa-gear', N'/yonetim/Ayarlar.aspx', 30, N'SistemAyarlar')

SET IDENTITY_INSERT [dbo].[Menu] OFF

PRINT 'Örnek menü verileri eklendi.'

-- Kullanıcı Yetki tablosunu oluştur (eğer yoksa)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[KullaniciYetki]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[KullaniciYetki](
        [ID] [int] IDENTITY(1,1) NOT NULL,
        [KullaniciID] [int] NOT NULL,
        [YetkiKodu] [nvarchar](50) NOT NULL,
        [Aktif] [bit] NOT NULL,
        CONSTRAINT [PK_KullaniciYetki] PRIMARY KEY CLUSTERED 
        (
            [ID] ASC
        )
    )
    
    PRINT 'KullaniciYetki tablosu oluşturuldu.'
END
ELSE
BEGIN
    PRINT 'KullaniciYetki tablosu zaten var.'
END 