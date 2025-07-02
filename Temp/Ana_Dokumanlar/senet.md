
### Senet Yönetiminin Temel Farkı: "Kendi Senedimiz" ve "Müşteri Senedi"

Çeklerden farklı olarak senetlerde iki temel tür bulunur ve veritabanı bu ayrımı net bir şekilde yapabilmelidir:

1.  **Alınan Senetler (Müşteri Senetleri):** Müşterilerinizden alacaklarınıza karşılık aldığınız senetlerdir. Bunlar sizin için birer alacak belgesidir. Portföyünüzde durur, ciro edilebilir veya tahsile verilebilir.
2.  **Verilen Senetler (Kendi Senetlerimiz / Borç Senetleri):** Tedarikçilerinize olan borçlarınıza karşılık sizin düzenleyip verdiğiniz senetlerdir. Bunlar sizin için birer borç belgesidir.

Bu ayrımı yönetmek için ana `Senetler` tablosuna bir `SenetTipi` kolonu ekleyeceğiz.

---

### Yeni Eklenecek Tablolar

Mevcut yapınıza ek olarak aşağıdaki 4 tabloyu oluşturmanız gerekecek. `FinansalKurumlar` tablosunu çekler için zaten oluşturduysanız tekrar oluşturmanıza gerek yoktur.

#### 1. `Senetler` (Ana Senet Tablosu)
Bu tablo, hem kendi verdiğiniz hem de müşteriden aldığınız tüm senetleri tek bir yerde tutar.

```sql
CREATE TABLE Senetler (
    SenetID INT PRIMARY KEY IDENTITY(1,1),
    SirketID INT NOT NULL FOREIGN KEY REFERENCES Sirketler(SirketID),

    -- 'A': Alınan Senet (Müşteri Senedi)
    -- 'V': Verilen Senet (Kendi Senedimiz/Borç Senedi)
    SenetTipi CHAR(1) NOT NULL,

    -- Senet Bilgileri
    SeriNo NVARCHAR(100), -- Kendi senedimizde seri no olmayabilir.
    VadeTarihi DATE NOT NULL,
    DuzenlemeTarihi DATE NOT NULL,
    Tutar DECIMAL(18, 2) NOT NULL,
    ParaBirimiID INT NOT NULL, -- Mevcut ParaBirimi tablonuza bağlanmalı

    -- Senet üzerindeki taraflar
    Borclu NVARCHAR(250) NOT NULL, -- Senedi ödeyecek olan kişi/kurumun adı
    OdemeYeri NVARCHAR(100), -- Genellikle bir şehir adı

    -- İlişkili Taraf (Senet tipine göre anlamı değişir)
    -- SenetTipi='A' ise, senedi aldığımız MüşteriID'si
    IlgiliMusteriID INT NULL FOREIGN KEY REFERENCES Musteriler(MusteriID),
    -- SenetTipi='V' ise, senedi verdiğimiz TedarikciID'si
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
```

#### 2. `SenetDurumlari` (Yardımcı Tablo)
Senetlerin durumlarını standartlaştırmak için.

```sql
CREATE TABLE SenetDurumlari (
    DurumID INT PRIMARY KEY,
    DurumAdi NVARCHAR(50) NOT NULL
);

-- Örnek Veriler
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
```

#### 3. `SenetIslemTipleri` (Yardımcı Tablo)
Senet hareketlerini standartlaştırmak için.

```sql
CREATE TABLE SenetIslemTipleri (
    IslemTipiID INT PRIMARY KEY,
    IslemTipiAdi NVARCHAR(100) NOT NULL
);

-- Örnek Veriler
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
```

#### 4. `SenetHareketleri` (İşlem Geçmişi Tablosu)
Senedin tüm yaşam döngüsünü kaydeder. Raporlama ve izlenebilirlik için bu tabloyu kullanacaksınız.

```sql
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

---

### İş Akışları ve Kodlama Mantığı (ASP.NET Tarafı İçin)

Her işlemde mutlaka **SQL Transaction** kullanın.

#### Senaryo 1: Müşteriden Senet Almak
1.  **Transaction Başlat.**
2.  **`Senetler` Tablosuna Ekle:**
    *   `SenetTipi` = **'A' (Alınan)**.
    *   `IlgiliMusteriID` = Senedi veren müşterinin ID'si.
    *   `IlgiliTedarikciID` = NULL.
    *   `Borclu` = Formdan girilen borçlu adı (genellikle müşteri firması).
    *   `DurumID` = **10 (Portföyde)**.
    *   Yeni eklenen `SenetID`'yi al.
3.  **`SenetHareketleri` Tablosuna Ekle:**
    *   `SenetID` = Yeni `SenetID`.
    *   `IslemTipiID` = **100 (Müşteriden Senet Alındı)**.
    *   `IlgiliMusteriID` = Senedi veren müşterinin ID'si.
4.  **Transaction Onayla (Commit).**

#### Senaryo 2: Tedarikçiye Kendi Senedimizi Vermek
1.  **Transaction Başlat.**
2.  **`Senetler` Tablosuna Ekle:**
    *   `SenetTipi` = **'V' (Verilen)**.
    *   `IlgiliMusteriID` = NULL.
    *   `IlgiliTedarikciID` = Senedi verdiğimiz tedarikçinin ID'si.
    *   `Borclu` = Sizin şirketinizin adı (`Sirketler` tablosundan çekilebilir).
    *   `DurumID` = **20 (Tedarikçide)**.
    *   Yeni eklenen `SenetID`'yi al.
3.  **`SenetHareketleri` Tablosuna Ekle:**
    *   `SenetID` = Yeni `SenetID`.
    *   `IslemTipiID` = **200 (Tedarikçiye Senet Verildi)**.
    *   `IlgiliTedarikciID` = Senedi verdiğimiz tedarikçinin ID'si.
4.  **Transaction Onayla (Commit).**

#### Senaryo 3: Müşteri Senedini Tedarikçiye Ciro Etmek
1.  **Transaction Başlat.**
2.  **`Senetler` Tablosunu Güncelle:**
    *   `UPDATE Senetler SET DurumID = 11 WHERE SenetID = @CiroEdilecekSenetID`
3.  **`SenetHareketleri` Tablosuna Ekle:**
    *   `SenetID` = Ciro edilen senedin ID'si.
    *   `IslemTipiID` = **110 (Tedarikçiye Ciro Edildi)**.
    *   `IlgiliTedarikciID` = Senedin ciro edildiği tedarikçinin ID'si.
4.  **Transaction Onayla (Commit).**

#### Senaryo 4: Kendi Senedimizi Ödemek
1.  **Transaction Başlat.**
2.  **`Senetler` Tablosunu Güncelle:**
    *   `UPDATE Senetler SET DurumID = 21 WHERE SenetID = @OdenecekSenetID`
3.  **`SenetHareketleri` Tablosuna Ekle:**
    *   `SenetID` = Ödenen senedin ID'si.
    *   `IslemTipiID` = **210 (Kendi Senedimiz Ödendi)**.
    *   İşlem kasa veya bankadan yapıldıysa `Ilgili` ID'ler NULL kalabilir veya ödemenin yapıldığı banka hesabı `IlgiliFinansalKurumID` olarak eklenebilir.
4.  **Transaction Onayla (Commit).**

Bu yapı, senetlerin iki farklı doğasını (alacak ve borç) yönetmenizi sağlayacak, tüm hareket geçmişini tutarak tam izlenebilirlik sunacaktır.