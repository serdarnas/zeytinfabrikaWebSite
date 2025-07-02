

**Öneri:** Bankalar ve Faktöring şirketleri gibi finansal kurumları da yönetmeniz gerekecek. Bunları `Musteriler` veya `Tedarikciler` tablosuna eklemek yerine, kendilerine ait yeni bir tabloda tutmak daha temiz bir yaklaşım olacaktır.

---

### Yeni Eklenecek Tablolar

Mevcut yapınıza ek olarak aşağıdaki 5 tabloyu oluşturmanız gerekecek. Tablo ve kolon isimlerini, sizin mevcut isimlendirme standardınıza (Türkçe, PascalCase) uygun olarak hazırladım.

#### 1. `FinansalKurumlar`
Bankalarınızı ve faktöring şirketlerinizi tanımlayacağınız tablo.

```sql
CREATE TABLE FinansalKurumlar (
    FinansalKurumID INT PRIMARY KEY IDENTITY(1,1),
    SirketID INT NOT NULL FOREIGN KEY REFERENCES Sirketler(SirketID),
    -- 'B': Banka, 'F': Faktöring
    KurumTipi CHAR(1) NOT NULL,
    KurumAdi NVARCHAR(250) NOT NULL,
    SubeAdi NVARCHAR(150),
    Iban NVARCHAR(34),
    YetkiliAdi NVARCHAR(150),
    Telefon VARCHAR(20),
    AktifMi BIT DEFAULT 1
);
```

#### 2. `CekDurumlari` (Yardımcı Tablo)
Çeklerin anlık durumlarını standartlaştırmak için.

```sql
CREATE TABLE CekDurumlari (
    DurumID INT PRIMARY KEY,
    DurumAdi NVARCHAR(50) NOT NULL
);

-- Bu verileri projenizin başında bir defa eklemeniz yeterli
INSERT INTO CekDurumlari (DurumID, DurumAdi) VALUES
(1, 'Portföyde'),
(2, 'Tedarikçiye Ciro Edildi'),
(3, 'Bankaya Tahsile Verildi'),
(4, 'Faktöringe Verildi'),
(5, 'Tahsil Edildi'),
(6, 'Karşılıksız'),
(7, 'İade Edildi');
```

#### 3. `CekIslemTipleri` (Yardımcı Tablo)
Çek hareketlerini standartlaştırmak için.

```sql
CREATE TABLE CekIslemTipleri (
    IslemTipiID INT PRIMARY KEY,
    IslemTipiAdi NVARCHAR(100) NOT NULL
);

-- Bu verileri projenizin başında bir defa eklemeniz yeterli
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
```

#### 4. `Cekler` (Ana Çek Tablosu)
Bu tablo, her bir çekin kimlik bilgilerini ve mevcut durumunu tutar.

```sql
CREATE TABLE Cekler (
    CekID INT PRIMARY KEY IDENTITY(1,1),
    SirketID INT NOT NULL FOREIGN KEY REFERENCES Sirketler(SirketID),

    -- Çeki aldığımız müşteri
    AlinanMusteriID INT NOT NULL FOREIGN KEY REFERENCES Musteriler(MusteriID),
    AlisTarihi DATE NOT NULL,

    -- Çekin fiziksel bilgileri
    SeriNo NVARCHAR(100) NOT NULL,
    BankaAdi NVARCHAR(150) NOT NULL, -- Çeki düzenleyen banka
    SubeAdi NVARCHAR(150) NOT NULL,
    HesapNo NVARCHAR(50) NOT NULL,
    Kesideci NVARCHAR(250) NOT NULL, -- Çeki imzalayan (müşteriniz veya müşterinizin müşterisi)
    Tutar DECIMAL(18, 2) NOT NULL,
    ParaBirimiID INT NOT NULL, -- Sizin mevcut ParaBirimi tablonuza bağlanabilir
    VadeTarihi DATE NOT NULL,
    KesideTarihi DATE NOT NULL,
    OdemeYeri NVARCHAR(100),

    -- Çekin anlık durumu (raporlarda kolaylık sağlar)
    DurumID INT NOT NULL FOREIGN KEY REFERENCES CekDurumlari(DurumID),

    Aciklama NVARCHAR(1000),
    OlusturmaTarihi DATETIME DEFAULT GETDATE()
);
```

#### 5. `CekHareketleri` (İşlem Geçmişi Tablosu - En Önemli Tablo)
Çekin tüm yaşam döngüsünü adım adım kaydeder. Raporlama ve izlenebilirlik için bu tabloyu kullanacaksınız.

```sql
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
    CONSTRAINT CHK_IlgiliID_TekilOlmalı CHECK (
        (CASE WHEN IlgiliMusteriID IS NOT NULL THEN 1 ELSE 0 END +
         CASE WHEN IlgiliTedarikciID IS NOT NULL THEN 1 ELSE 0 END +
         CASE WHEN IlgiliFinansalKurumID IS NOT NULL THEN 1 ELSE 0 END) <= 1
    )
);
```

---

### İş Akışları ve Kodlama Mantığı (ASP.NET Tarafı İçin)

Her işlemde **SQL Transaction** kullanmanız çok önemli!

#### 1. Müşteriden Çek Alındığında
1.  **Transaction Başlat.**
2.  **`Cekler` Tablosuna Ekle:**
    *   `SirketID` = Giriş yapan kullanıcının şirket ID'si.
    *   `AlinanMusteriID` = Çeki veren müşterinin ID'si.
    *   `DurumID` = **1 (Portföyde)**.
    *   Diğer çek bilgilerini forma girildiği gibi kaydet.
    *   Yeni eklenen `CekID`'yi al.
3.  **`CekHareketleri` Tablosuna Ekle:**
    *   `CekID` = Az önce aldığın yeni `CekID`.
    *   `IslemTipiID` = **10 (Müşteriden Çek Alındı)**.
    *   `IlgiliMusteriID` = Çeki veren müşterinin ID'si.
    *   `IlgiliTedarikciID` = NULL.
    *   `IlgiliFinansalKurumID` = NULL.
4.  **Transaction Onayla (Commit).**

#### 2. Çeki Tedarikçiye Ciro Ettiğinizde (Ödeme)
1.  **Transaction Başlat.**
2.  **`Cekler` Tablosunu Güncelle:**
    *   `UPDATE Cekler SET DurumID = 2 WHERE CekID = @CiroEdilecekCekID AND SirketID = @SirketID`
3.  **`CekHareketleri` Tablosuna Ekle:**
    *   `CekID` = Ciro edilen çekin ID'si.
    *   `IslemTipiID` = **20 (Tedarikçiye Ciro Edildi)**.
    *   `IlgiliMusteriID` = NULL.
    *   `IlgiliTedarikciID` = Çekin verildiği tedarikçinin ID'si.
    *   `IlgiliFinansalKurumID` = NULL.
4.  **Transaction Onayla (Commit).**

#### 3. Çeki Faktöringe Verdiğinizde
1.  **Transaction Başlat.**
2.  **`Cekler` Tablosunu Güncelle:**
    *   `UPDATE Cekler SET DurumID = 4 WHERE CekID = @FaktoringeVerilecekCekID AND SirketID = @SirketID`
3.  **`CekHareketleri` Tablosuna Ekle:**
    *   `CekID` = Faktöringe verilen çekin ID'si.
    *   `IslemTipiID` = **40 (Faktöringe Verildi)**.
    *   `IlgiliMusteriID` = NULL.
    *   `IlgiliTedarikciID` = NULL.
    *   `IlgiliFinansalKurumID` = Faktöring şirketinin `FinansalKurumlar` tablosundaki ID'si.
4.  **Transaction Onayla (Commit).**

#### 4. Çeki Bankaya Tahsile Verdiğinizde
1.  **Transaction Başlat.**
2.  **`Cekler` Tablosunu Güncelle:**
    *   `UPDATE Cekler SET DurumID = 3 WHERE CekID = @BankayaVerilecekCekID AND SirketID = @SirketID`
3.  **`CekHareketleri` Tablosuna Ekle:**
    *   `CekID` = Bankaya verilen çekin ID'si.
    *   `IslemTipiID` = **30 (Bankaya Tahsile Verildi)**.
    *   `IlgiliMusteriID` = NULL.
    *   `IlgiliTedarikciID` = NULL.
    *   `IlgiliFinansalKurumID` = Tahsile verdiğiniz bankanın `FinansalKurumlar` tablosundaki ID'si.
4.  **Transaction Onayla (Commit).**

Bu yapı, mevcut sisteminizle sorunsuz bir şekilde bütünleşecek ve size çeklerinizin tüm geçmişini raporlama imkanı sunacaktır. Projenizde başarılar