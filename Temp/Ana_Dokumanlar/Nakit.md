# Nakit İşlemleri ve Kasa Yapısı

Bu dokümanda, Zeytin Fabrika Yönetim Sistemi projesi için nakit işlemleri ve kasa yönetimi için önerilen veritabanı yapısı ve iş akışları açıklanmıştır.

## Veritabanı Tabloları

### 1. `NakitIslemler` Tablosu

Bu tablo tüm nakit tahsilat ve ödemelerin kaydını tutar:

```sql
CREATE TABLE NakitIslemler (
    NakitIslemID INT PRIMARY KEY IDENTITY(1,1),
    SirketID INT NOT NULL FOREIGN KEY REFERENCES Sirketler(SirketID),
    
    -- 'T': Tahsilat (Para Girişi)
    -- 'O': Ödeme (Para Çıkışı)
    IslemTuru CHAR(1) NOT NULL,
    
    IslemTarihi DATETIME NOT NULL,
    
    -- İşleme konu olan taraf (sadece biri dolu olmalı)
    MusteriID INT NULL FOREIGN KEY REFERENCES Musteriler(MusteriID),
    TedarikciID INT NULL FOREIGN KEY REFERENCES Tedarikciler(TedarikciID),
    MustahsilID INT NULL FOREIGN KEY REFERENCES Mustahsiller(MustahsilID),
    
    -- Tutar bilgileri
    Tutar DECIMAL(18, 2) NOT NULL,
    ParaBirimiID INT NOT NULL FOREIGN KEY REFERENCES ParaBirimileri(ParaBirimiID),
    
    -- Nakit ya da banka ödemesi
    OdemeTipiID INT NOT NULL FOREIGN KEY REFERENCES OdemeTipleri(OdemeTipiID),
    
    -- Banka ödemesi ise ilgili hesap
    BankaHesapID INT NULL FOREIGN KEY REFERENCES BankaHesaplar(BankaHesapID),
    
    -- Referans bilgileri
    ReferansNo NVARCHAR(50) NULL,
    ReferansTipi NVARCHAR(20) NULL, -- "Satış", "Alım", "Avans" vb.
    ReferansID INT NULL, -- İlgili işlemin ID'si
    
    Aciklama NVARCHAR(500) NULL,
    KullaniciID INT NULL FOREIGN KEY REFERENCES Kullanicilar(KullaniciID),
    OlusturmaTarihi DATETIME DEFAULT GETDATE(),
    
    -- Tek bir ilgili taraf kontrolü
    CONSTRAINT CHK_NakitIslem_IlgiliTaraf CHECK (
        (CASE WHEN MusteriID IS NOT NULL THEN 1 ELSE 0 END +
         CASE WHEN TedarikciID IS NOT NULL THEN 1 ELSE 0 END +
         CASE WHEN MustahsilID IS NOT NULL THEN 1 ELSE 0 END) = 1
    )
);
```

### 2. `OdemeTipleri` Tablosu

Ödeme tiplerini standartlaştırmak için:

```sql
CREATE TABLE OdemeTipleri (
    OdemeTipiID INT PRIMARY KEY,
    OdemeTipiAdi NVARCHAR(50) NOT NULL
);

-- Örnek Veriler
INSERT INTO OdemeTipleri (OdemeTipiID, OdemeTipiAdi) VALUES
(1, 'Nakit'),
(2, 'Havale/EFT'),
(3, 'Kredi Kartı'),
(4, 'Banka Kartı'),
(5, 'Temassız Kredi Kartı');
```

### 3. `BankaHesaplar` Tablosu

Şirkete ait banka hesaplarını yönetmek için:

```sql
CREATE TABLE BankaHesaplar (
    BankaHesapID INT PRIMARY KEY IDENTITY(1,1),
    SirketID INT NOT NULL FOREIGN KEY REFERENCES Sirketler(SirketID),
    HesapAdi NVARCHAR(100) NOT NULL,
    BankaAdi NVARCHAR(100) NOT NULL,
    SubeAdi NVARCHAR(100) NULL,
    HesapNo NVARCHAR(50) NULL,
    IBAN NVARCHAR(50) NULL,
    ParaBirimiID INT NOT NULL FOREIGN KEY REFERENCES ParaBirimileri(ParaBirimiID),
    AktifMi BIT DEFAULT 1,
    Aciklama NVARCHAR(500) NULL,
    OlusturmaTarihi DATETIME DEFAULT GETDATE()
);
```

### 4. `Kasalar` Tablosu

Fiziksel ve dijital kasaların tanımlandığı tablo:

```sql
CREATE TABLE Kasalar (
    KasaID INT PRIMARY KEY IDENTITY(1,1),
    SirketID INT NOT NULL FOREIGN KEY REFERENCES Sirketler(SirketID),
    KasaKodu NVARCHAR(20) NOT NULL,
    KasaAdi NVARCHAR(100) NOT NULL,
    
    -- 'F': Fiziksel Kasa, 'D': Dijital Kasa
    KasaTipi CHAR(1) NOT NULL,
    
    ParaBirimiID INT NOT NULL FOREIGN KEY REFERENCES ParaBirimileri(ParaBirimiID),
    Bakiye DECIMAL(18, 2) DEFAULT 0,
    AktifMi BIT DEFAULT 1,
    Aciklama NVARCHAR(500) NULL,
    OlusturmaTarihi DATETIME DEFAULT GETDATE()
);
```

### 5. `KasaHareketler` Tablosu

Kasalardaki para giriş-çıkışlarını izlemek için:

```sql
CREATE TABLE KasaHareketler (
    HareketID INT PRIMARY KEY IDENTITY(1,1),
    SirketID INT NOT NULL FOREIGN KEY REFERENCES Sirketler(SirketID),
    KasaID INT NOT NULL FOREIGN KEY REFERENCES Kasalar(KasaID),
    IslemTarihi DATETIME NOT NULL,
    
    -- 'G': Giriş, 'C': Çıkış
    IslemTipi CHAR(1) NOT NULL,
    
    Tutar DECIMAL(18, 2) NOT NULL,
    
    -- İlişkili işlem bilgileri
    ReferansTipi NVARCHAR(30) NULL, -- "NakitIslem", "Çek", "Senet", "Virman"
    ReferansID INT NULL, -- İlgili işlemin ID'si
    
    Aciklama NVARCHAR(500) NULL,
    KullaniciID INT NULL FOREIGN KEY REFERENCES Kullanicilar(KullaniciID),
    OlusturmaTarihi DATETIME DEFAULT GETDATE()
);
```

### 6. `KasaVirman` Tablosu

Kasalar arası para transferlerini kaydetmek için:

```sql
CREATE TABLE KasaVirman (
    VirmanID INT PRIMARY KEY IDENTITY(1,1),
    SirketID INT NOT NULL FOREIGN KEY REFERENCES Sirketler(SirketID),
    IslemTarihi DATETIME NOT NULL,
    
    KaynakKasaID INT NOT NULL FOREIGN KEY REFERENCES Kasalar(KasaID),
    HedefKasaID INT NOT NULL FOREIGN KEY REFERENCES Kasalar(KasaID),
    
    Tutar DECIMAL(18, 2) NOT NULL,
    KurDegeri DECIMAL(18, 4) NULL, -- Farklı para birimlerinde transfer yapılırsa
    
    Aciklama NVARCHAR(500) NULL,
    KullaniciID INT NULL FOREIGN KEY REFERENCES Kullanicilar(KullaniciID),
    OlusturmaTarihi DATETIME DEFAULT GETDATE()
);
```

## Uygulama Mantığı ve İş Akışları

### 1. Müşteriden Nakit Tahsilat İşlemi

1. `NakitIslemler` tablosuna yeni bir kayıt eklenir:
   - `IslemTuru = 'T'` (Tahsilat)
   - `MusteriID` = İlgili müşteri ID'si
   - `OdemeTipiID` = Seçilen ödeme tipi (Nakit, Havale, Kredi Kartı vb.)

2. `KasaHareketler` tablosuna da bir kayıt eklenir:
   - `KasaID` = İşlemin yapıldığı kasa ID'si
   - `IslemTipi = 'G'` (Giriş)
   - `ReferansTipi = 'NakitIslem'`
   - `ReferansID` = Eklenen nakit işlemin ID'si

3. İlgili kasanın bakiyesi güncellenir.

### 2. Tedarikçiye Nakit Ödeme İşlemi

1. `NakitIslemler` tablosuna yeni bir kayıt eklenir:
   - `IslemTuru = 'O'` (Ödeme)
   - `TedarikciID` = İlgili tedarikçi ID'si
   - `OdemeTipiID` = Seçilen ödeme tipi

2. `KasaHareketler` tablosuna da bir kayıt eklenir:
   - `KasaID` = İşlemin yapıldığı kasa ID'si
   - `IslemTipi = 'C'` (Çıkış)
   - `ReferansTipi = 'NakitIslem'`
   - `ReferansID` = Eklenen nakit işlemin ID'si

3. İlgili kasanın bakiyesi güncellenir.

### 3. Kasalar Arası Transfer İşlemi

1. `KasaVirman` tablosuna yeni bir kayıt eklenir.

2. `KasaHareketler` tablosuna iki kayıt eklenir:
   - Kaynak kasa için çıkış hareketi
   - Hedef kasa için giriş hareketi

3. Her iki kasanın bakiyesi güncellenir.

## Entegrasyon Noktaları

1. **Müşteri Detay Sayfası**: Mevcut çek ve senet butonlarının yanına "Nakit Tahsilat" ve "Kasa Görüntüle" butonları eklenebilir.

2. **Tedarikçi Detay Sayfası**: Benzer şekilde nakit ödeme seçenekleri eklenebilir.

3. **Raporlama**: Nakit akışı, kasa durumu ve kasa hareketlerini gösteren raporlar oluşturulabilir.

## SQL Transaction Kullanımı

Tüm nakit ve kasa işlemlerinde mutlaka SQL Transaction kullanılmalıdır. Örnek kod:

```csharp
using (var dbContext = new FabrikaDataClassesDataContext())
{
    dbContext.Connection.Open();
    using (var transaction = dbContext.Connection.BeginTransaction())
    {
        try
        {
            dbContext.Transaction = transaction;
            
            // 1. Nakit işlem kaydı
            var nakitIslem = new NakitIslemler
            {
                SirketID = sirketID,
                IslemTuru = 'T',
                MusteriID = musteriID,
                // ... diğer alanlar
            };
            
            dbContext.NakitIslemlers.InsertOnSubmit(nakitIslem);
            dbContext.SubmitChanges();
            
            // 2. Kasa hareketi kaydı
            var kasaHareket = new KasaHareketler
            {
                SirketID = sirketID,
                KasaID = kasaID,
                IslemTipi = 'G',
                // ... diğer alanlar
            };
            
            dbContext.KasaHareketlers.InsertOnSubmit(kasaHareket);
            
            // 3. Kasa bakiyesi güncelleme
            var kasa = dbContext.Kasalars.SingleOrDefault(k => k.KasaID == kasaID);
            if (kasa != null)
            {
                kasa.Bakiye += tutar;
            }
            
            dbContext.SubmitChanges();
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception("İşlem sırasında hata oluştu: " + ex.Message);
        }
    }
}
```

Bu yapı, çek ve senet yapılarıyla benzer şekilde tasarlanmış olup, tüm finansal işlemlerinizi etkin bir şekilde yönetmenizi sağlayacaktır. ASP.NET WebForms ve mevcut veritabanınızla sorunsuz entegre edilebilir.
