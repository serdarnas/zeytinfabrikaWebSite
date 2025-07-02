# Nakit Modülü Veritabanı Kurulum Rehberi

Bu dokümanda Zeytin Fabrika Yönetim Sistemi - Nakit Modülü için gerekli veritabanı tablolarının kurulum bilgileri yer almaktadır.

## Gerekli Tablolar

### 1. Nakit İşlemleri İçin Tablolar (Mevcut)

Bu tablolar zaten oluşturulmuş durumda:

- `Kasalar` - Kasa tanımları
- `NakitIslemler` - Nakit tahsilat ve ödemeler
- `KasaHareketler` - Kasa giriş/çıkış hareketleri
- `KasaVirman` - Kasalar arası transferler
- `OdemeTipleri` - Ödeme tipleri (Nakit, Havale, vb.)

### 2. Çek Yönetimi İçin Tablolar

Aşağıdaki tablolar oluşturulması gereken çek tabloları:

```sql
-- 1. Finansal Kurumlar Tablosu
CREATE TABLE FinansalKurumlar (
    FinansalKurumID INT PRIMARY KEY IDENTITY(1,1),
    SirketID INT NOT NULL FOREIGN KEY REFERENCES Sirketler(SirketID),
    KurumTipi CHAR(1) NOT NULL, -- 'B': Banka, 'F': Faktöring
    KurumAdi NVARCHAR(250) NOT NULL,
    SubeAdi NVARCHAR(150),
    Iban NVARCHAR(34),
    YetkiliAdi NVARCHAR(150),
    Telefon VARCHAR(20),
    AktifMi BIT DEFAULT 1
);

-- 2. Çek Durumları Tablosu
CREATE TABLE CekDurumlari (
    DurumID INT PRIMARY KEY,
    DurumAdi NVARCHAR(50) NOT NULL
);

-- Çek Durumları Verileri
INSERT INTO CekDurumlari (DurumID, DurumAdi) VALUES
(1, 'Portföyde'),
(2, 'Tedarikçiye Ciro Edildi'),
(3, 'Bankaya Tahsile Verildi'),
(4, 'Faktöringe Verildi'),
(5, 'Tahsil Edildi'),
(6, 'Karşılıksız'),
(7, 'İade Edildi');

-- 3. Çek İşlem Tipleri Tablosu
CREATE TABLE CekIslemTipleri (
    IslemTipiID INT PRIMARY KEY,
    IslemTipiAdi NVARCHAR(100) NOT NULL
);

-- Çek İşlem Tipleri Verileri
INSERT INTO CekIslemTipleri (IslemTipiID, IslemTipiAdi) VALUES
(10, 'Müşteriden Çek Alındı'),
(20, 'Tedarikçiye Ciro Edildi'),
(30, 'Bankaya Tahsile Verildi'),
(40, 'Faktöringe Verildi'),
(50, 'Banka Tarafından Tahsil Edildi'),
(51, 'Faktöring Tarafından Tahsil Edildi'),
(60, 'Karşılıksız Çıktı'),
(70, 'Müşteriye İade Edildi'),
(71, 'Tedarikçiden İade Alındı');

-- 4. Çekler Ana Tablosu
CREATE TABLE Cekler (
    CekID INT PRIMARY KEY IDENTITY(1,1),
    SirketID INT NOT NULL FOREIGN KEY REFERENCES Sirketler(SirketID),
    
    -- Çeki aldığımız müşteri
    AlinanMusteriID INT NOT NULL FOREIGN KEY REFERENCES Musteriler(MusteriID),
    AlisTarihi DATE NOT NULL,

    -- Çekin fiziksel bilgileri
    SeriNo NVARCHAR(100) NOT NULL,
    BankaAdi NVARCHAR(150) NOT NULL,
    SubeAdi NVARCHAR(150) NOT NULL,
    HesapNo NVARCHAR(50) NOT NULL,
    Kesideci NVARCHAR(250) NOT NULL,
    Tutar DECIMAL(18, 2) NOT NULL,
    ParaBirimiID INT NOT NULL FOREIGN KEY REFERENCES ParaBirimileri(ParaBirimiID),
    VadeTarihi DATE NOT NULL,
    KesideTarihi DATE NOT NULL,
    OdemeYeri NVARCHAR(100),

    -- Çekin anlık durumu
    DurumID INT NOT NULL FOREIGN KEY REFERENCES CekDurumlari(DurumID),

    Aciklama NVARCHAR(1000),
    OlusturmaTarihi DATETIME DEFAULT GETDATE()
);

-- 5. Çek Hareketleri Tablosu
CREATE TABLE CekHareketleri (
    HareketID BIGINT PRIMARY KEY IDENTITY(1,1),
    SirketID INT NOT NULL FOREIGN KEY REFERENCES Sirketler(SirketID),
    CekID INT NOT NULL FOREIGN KEY REFERENCES Cekler(CekID),
    IslemTarihi DATETIME NOT NULL,
    IslemTipiID INT NOT NULL FOREIGN KEY REFERENCES CekIslemTipleri(IslemTipiID),

    -- İşlemin karşı tarafı (sadece biri dolu olacak)
    IlgiliMusteriID INT NULL FOREIGN KEY REFERENCES Musteriler(MusteriID),
    IlgiliTedarikciID INT NULL FOREIGN KEY REFERENCES Tedarikciler(TedarikciID),
    IlgiliFinansalKurumID INT NULL FOREIGN KEY REFERENCES FinansalKurumlar(FinansalKurumID),
    
    Tutar DECIMAL(18, 2) NOT NULL,
    Aciklama NVARCHAR(1000),

    -- Sadece bir ilgili ID'nin dolu olmasını zorunlu kılan kural
    CONSTRAINT CHK_CekHareket_IlgiliID_TekilOlmalı CHECK (
        (CASE WHEN IlgiliMusteriID IS NOT NULL THEN 1 ELSE 0 END +
         CASE WHEN IlgiliTedarikciID IS NOT NULL THEN 1 ELSE 0 END +
         CASE WHEN IlgiliFinansalKurumID IS NOT NULL THEN 1 ELSE 0 END) <= 1
    )
);
```

### 3. Senet Yönetimi İçin Tablolar

```sql
-- 1. Senet Durumları Tablosu
CREATE TABLE SenetDurumlari (
    DurumID INT PRIMARY KEY,
    DurumAdi NVARCHAR(50) NOT NULL
);

-- Senet Durumları Verileri
INSERT INTO SenetDurumlari (DurumID, DurumAdi) VALUES
-- Müşteri Senetleri İçin Durumlar
(10, 'Portföyde'),
(11, 'Tedarikçiye Ciro Edildi'),
(12, 'Bankaya Tahsile Verildi'),
(13, 'Faktöringe Verildi'),
(14, 'Tahsil Edildi'),
(15, 'Protesto Edildi'),
(16, 'Müşteriye İade Edildi'),

-- Kendi Senetlerimiz İçin Durumlar
(20, 'Tedarikçide (Ödenmedi)'),
(21, 'Ödendi'),
(22, 'İptal Edildi');

-- 2. Senet İşlem Tipleri Tablosu
CREATE TABLE SenetIslemTipleri (
    IslemTipiID INT PRIMARY KEY,
    IslemTipiAdi NVARCHAR(100) NOT NULL
);

-- Senet İşlem Tipleri Verileri
INSERT INTO SenetIslemTipleri (IslemTipiID, IslemTipiAdi) VALUES
(100, 'Müşteriden Senet Alındı'),
(110, 'Tedarikçiye Ciro Edildi'),
(120, 'Bankaya Tahsile Verildi'),
(130, 'Faktöringe Verildi'),
(140, 'Müşteri Senedi Tahsil Edildi'),
(150, 'Senet Protesto Edildi'),
(160, 'Müşteriye İade Edildi'),
(200, 'Tedarikçiye Senet Verildi'),
(210, 'Kendi Senedimiz Ödendi');

-- 3. Senetler Ana Tablosu
CREATE TABLE Senetler (
    SenetID INT PRIMARY KEY IDENTITY(1,1),
    SirketID INT NOT NULL FOREIGN KEY REFERENCES Sirketler(SirketID),

    -- 'A': Alınan Senet (Müşteri Senedi)
    -- 'V': Verilen Senet (Kendi Senedimiz/Borç Senedi)
    SenetTipi CHAR(1) NOT NULL,

    -- Senet Bilgileri
    SeriNo NVARCHAR(100),
    VadeTarihi DATE NOT NULL,
    DuzenlemeTarihi DATE NOT NULL,
    Tutar DECIMAL(18, 2) NOT NULL,
    ParaBirimiID INT NOT NULL FOREIGN KEY REFERENCES ParaBirimileri(ParaBirimiID),

    -- Senet üzerindeki taraflar
    Borclu NVARCHAR(250) NOT NULL,
    OdemeYeri NVARCHAR(100),

    -- İlişkili Taraf (Senet tipine göre anlamı değişir)
    IlgiliMusteriID INT NULL FOREIGN KEY REFERENCES Musteriler(MusteriID),
    IlgiliTedarikciID INT NULL FOREIGN KEY REFERENCES Tedarikciler(TedarikciID),

    -- Anlık Durum
    DurumID INT NOT NULL FOREIGN KEY REFERENCES SenetDurumlari(DurumID),

    Aciklama NVARCHAR(1000),
    OlusturmaTarihi DATETIME DEFAULT GETDATE(),

    -- Sadece bir ilgili ID'nin dolu olmasını sağlayan kural
    CONSTRAINT CHK_Senet_IlgiliID CHECK (
        (CASE WHEN IlgiliMusteriID IS NOT NULL THEN 1 ELSE 0 END +
         CASE WHEN IlgiliTedarikciID IS NOT NULL THEN 1 ELSE 0 END) <= 1
    )
);

-- 4. Senet Hareketleri Tablosu
CREATE TABLE SenetHareketleri (
    HareketID BIGINT PRIMARY KEY IDENTITY(1,1),
    SirketID INT NOT NULL FOREIGN KEY REFERENCES Sirketler(SirketID),
    SenetID INT NOT NULL FOREIGN KEY REFERENCES Senetler(SenetID),
    IslemTarihi DATETIME NOT NULL,
    IslemTipiID INT NOT NULL FOREIGN KEY REFERENCES SenetIslemTipleri(IslemTipiID),

    -- İşlemin karşı tarafı (sadece biri dolu olacak)
    IlgiliMusteriID INT NULL FOREIGN KEY REFERENCES Musteriler(MusteriID),
    IlgiliTedarikciID INT NULL FOREIGN KEY REFERENCES Tedarikciler(TedarikciID),
    IlgiliFinansalKurumID INT NULL FOREIGN KEY REFERENCES FinansalKurumlar(FinansalKurumID),
    
    Tutar DECIMAL(18, 2) NOT NULL,
    Aciklama NVARCHAR(1000),

    CONSTRAINT CHK_SenetHareket_IlgiliID_TekilOlmalı CHECK (
        (CASE WHEN IlgiliMusteriID IS NOT NULL THEN 1 ELSE 0 END +
         CASE WHEN IlgiliTedarikciID IS NOT NULL THEN 1 ELSE 0 END +
         CASE WHEN IlgiliFinansalKurumID IS NOT NULL THEN 1 ELSE 0 END) <= 1
    )
);
```

## Kurulum Adımları

1. **Çek Tablolarını Oluşturun:**
   - Yukarıdaki çek tablolarını sırasıyla oluşturun
   - Önce referans tabloları (CekDurumlari, CekIslemTipleri), sonra ana tabloları

2. **Senet Tablolarını Oluşturun:**
   - Yukarıdaki senet tablolarını sırasıyla oluşturun
   - Önce referans tabloları, sonra ana tabloları

3. **Verileri Kontrol Edin:**
   - Seed verilerinin doğru şekilde eklendiğini kontrol edin
   - Foreign key ilişkilerinin çalıştığını test edin

## Önemli Notlar

- **Transaction Kullanımı:** Tüm finansal işlemlerde mutlaka SQL Transaction kullanılmalıdır
- **Veri Bütünlüğü:** Foreign key kısıtlamaları mutlaka uygulanmalıdır
- **İndeksler:** Performans için gerekli indeksler oluşturulmalıdır (SirketID, tarih alanları vb.)
- **Yetkilendirme:** Veritabanı kullanıcılarının bu tablolara gerekli izinleri olduğundan emin olun

## Dosya Yapısı

Nakit modülü aşağıdaki sayfa yapısına sahiptir:

```
WebSite/fabrika/Nakit/
├── Default.aspx(.cs)          # Ana dashboard
├── Kasalar.aspx(.cs)          # Kasa yönetimi
├── NakitIslemler.aspx(.cs)    # Nakit işlemler listesi
├── KasaHareketleri.aspx(.cs)  # Kasa hareket detayları
├── Cekler.aspx(.cs)           # Çek yönetimi
├── CekDetay.aspx(.cs)         # Çek detay sayfası (gelecekte)
├── Senetler.aspx(.cs)         # Senet yönetimi
├── SenetDetay.aspx(.cs)       # Senet detay sayfası (gelecekte)
└── README_Veritabani_Setup.md # Bu dosya
```

## Test Senaryoları

Kurulum sonrası aşağıdaki test senaryolarını gerçekleştirin:

1. **Kasa İşlemleri:**
   - Yeni kasa oluşturma
   - Nakit tahsilat/ödeme kaydetme
   - Kasalar arası transfer

2. **Çek İşlemleri:**
   - Müşteriden çek alma
   - Çek ciro etme
   - Çek tahsile verme

3. **Senet İşlemleri:**
   - Müşteriden senet alma
   - Tedarikçiye senet verme
   - Senet işlemleri

Bu rehberi takip ederek Nakit Modülünün tam fonksiyonel hale gelmesini sağlayabilirsiniz. 