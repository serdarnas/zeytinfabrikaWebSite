-- Menu table INSERT statements for fabrika section
-- Main level menus (UstMenuID = 0)
INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (100, 0, 'Genel Bakış', 'fa fa-dashboard', '/fabrika/Default.aspx', 1, 'FAB_GENEL');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (200, 0, 'Zeytinyağı', 'fa fa-leaf', '/fabrika/Zeytinyagi/Default.aspx', 2, 'FAB_ZEYTIN');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (300, 0, 'Müstahsil', 'fa fa-users', '/fabrika/Mustahsil/Default.aspx', 3, 'FAB_MUSTAHSIL');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (400, 0, 'Müşteriler', 'fa fa-user', '/fabrika/Musteriler/Default.aspx', 4, 'FAB_MUSTERI');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (500, 0, 'Tedarikçiler', 'fa fa-truck', '/fabrika/Tedarikciler/Default.aspx', 5, 'FAB_TEDARIKCI');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (600, 0, 'Ürünler', 'fa fa-cubes', '/fabrika/Urunler/UrunListesi.aspx', 6, 'FAB_URUN');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (700, 0, 'Depo', 'fa fa-archive', '/fabrika/Depo/Depo.aspx', 7, 'FAB_DEPO');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (800, 0, 'Üretim', 'fa fa-industry', '/fabrika/Uretim/Default.aspx', 8, 'FAB_URETIM');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (900, 0, 'Pazarlama', 'fa fa-bullhorn', '/fabrika/Pazarlama/Default.aspx', 9, 'FAB_PAZARLAMA');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (1000, 0, 'Projeler', 'fa fa-tasks', '/fabrika/Proje/Projelerim.aspx', 10, 'FAB_PROJE');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (1100, 0, 'Şirket', 'fa fa-building', '/fabrika/Sirket/SirketProfil.aspx', 11, 'FAB_SIRKET');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (1200, 0, 'Kullanıcılar', 'fa fa-users', '/fabrika/Kullanici/Default.aspx', 12, 'FAB_KULLANICI');

-- Zeytinyağı alt menüleri
INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (201, 200, 'Zeytin Kabul', 'fa fa-check-circle', '/fabrika/Zeytinyagi/ZeytinKabul.aspx', 1, 'FAB_ZEYTIN_KABUL');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (202, 200, 'Yeni Zeytin Kabul', 'fa fa-plus-circle', '/fabrika/Zeytinyagi/ZeytinKabulYeni.aspx', 2, 'FAB_ZEYTIN_KABUL_YENI');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (203, 200, 'Box/Kasa Oluştur', 'fa fa-cube', '/fabrika/Zeytinyagi/ZeytinBoxKasaOlustur.aspx', 3, 'FAB_ZEYTIN_BOX');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (204, 200, 'Parti Makine Seçimi', 'fa fa-cogs', '/fabrika/Zeytinyagi/PartiMakineSecimi.aspx', 4, 'FAB_ZEYTIN_PARTI');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (205, 200, 'Üretim Takip', 'fa fa-line-chart', '/fabrika/Zeytinyagi/UretimTakip.aspx', 5, 'FAB_ZEYTIN_URETIM');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (206, 200, 'Kalite Kontrol', 'fa fa-check-square', '/fabrika/Zeytinyagi/KaliteKotrolYonetimi.aspx', 6, 'FAB_ZEYTIN_KALITE');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (207, 200, 'Stok Yönetimi', 'fa fa-database', '/fabrika/Zeytinyagi/ZeytinyagStokYonetimi.aspx', 7, 'FAB_ZEYTIN_STOK');

-- Müstahsil alt menüleri
INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (301, 300, 'Müstahsil Listesi', 'fa fa-list', '/fabrika/Mustahsil/Default.aspx', 1, 'FAB_MUSTAHSIL_LISTE');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (302, 300, 'Yeni Müstahsil', 'fa fa-plus', '/fabrika/Mustahsil/YeniMustahsil.aspx', 2, 'FAB_MUSTAHSIL_EKLE');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (303, 300, 'Excel ile Yükle', 'fa fa-file-excel-o', '/fabrika/Mustahsil/Mustahsil_yukle_excel.aspx', 3, 'FAB_MUSTAHSIL_EXCEL');

-- Müşteriler alt menüleri
INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (401, 400, 'Müşteri Listesi', 'fa fa-list', '/fabrika/Musteriler/Default.aspx', 1, 'FAB_MUSTERI_LISTE');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (402, 400, 'Yeni Müşteri', 'fa fa-plus', '/fabrika/Musteriler/YeniMusteri.aspx', 2, 'FAB_MUSTERI_EKLE');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (403, 400, 'Müşteri Satış', 'fa fa-shopping-cart', '/fabrika/Musteriler/MusteriSatis.aspx', 3, 'FAB_MUSTERI_SATIS');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (404, 400, 'Excel ile Yükle', 'fa fa-file-excel-o', '/fabrika/Musteriler/musteri_yukle_excel.aspx', 4, 'FAB_MUSTERI_EXCEL');

-- Tedarikçiler alt menüleri
INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (501, 500, 'Tedarikçi Listesi', 'fa fa-list', '/fabrika/Tedarikciler/Default.aspx', 1, 'FAB_TEDARIKCI_LISTE');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (502, 500, 'Yeni Tedarikçi', 'fa fa-plus', '/fabrika/Tedarikciler/YeniTedarikci.aspx', 2, 'FAB_TEDARIKCI_EKLE');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (503, 500, 'Alış İşlemleri', 'fa fa-download', '/fabrika/Tedarikciler/Alis.aspx', 3, 'FAB_TEDARIKCI_ALIS');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (504, 500, 'Satış İşlemleri', 'fa fa-upload', '/fabrika/Tedarikciler/Satis.aspx', 4, 'FAB_TEDARIKCI_SATIS');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (505, 500, 'Excel ile Yükle', 'fa fa-file-excel-o', '/fabrika/Tedarikciler/Tedarikci_yukle_excel.aspx', 5, 'FAB_TEDARIKCI_EXCEL');

-- Ürünler alt menüleri
INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (601, 600, 'Ürün Listesi', 'fa fa-list', '/fabrika/Urunler/UrunListesi.aspx', 1, 'FAB_URUN_LISTE');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (602, 600, 'Yeni Ürün', 'fa fa-plus', '/fabrika/Urunler/YeniUrun.aspx', 2, 'FAB_URUN_EKLE');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (603, 600, 'Excel ile Yükle', 'fa fa-file-excel-o', '/fabrika/Urunler/Urun_yukle_excel.aspx', 3, 'FAB_URUN_EXCEL');

-- Projeler alt menüleri
INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (1001, 1000, 'Projelerim', 'fa fa-list', '/fabrika/Proje/Projelerim.aspx', 1, 'FAB_PROJE_LISTE');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (1002, 1000, 'Proje Detayı', 'fa fa-info-circle', '/fabrika/Proje/ProjeDetay.aspx', 2, 'FAB_PROJE_DETAY');

-- Kullanıcılar alt menüleri
INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (1201, 1200, 'Kullanıcı Listesi', 'fa fa-list', '/fabrika/Kullanici/Default.aspx', 1, 'FAB_KULLANICI_LISTE');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (1202, 1200, 'Yeni Kullanıcı', 'fa fa-user-plus', '/fabrika/Kullanici/YeniKullanici.aspx', 2, 'FAB_KULLANICI_EKLE');

INSERT INTO [Menu] ([MenuID], [UstMenuID], [MenuAdi], [Ikon], [SayfaURL], [Sira], [YetkiKodu])
VALUES (1203, 1200, 'Kullanıcı Profili', 'fa fa-id-card', '/fabrika/Kullanici/KullaniciProfil.aspx', 3, 'FAB_KULLANICI_PROFIL');
