# Stok Modülü Kullanım Kılavuzu

Bu README dosyası, Zeytin Fabrikası Yönetim Sistemi'ndeki stok modülünün nasıl kullanılacağını ve geliştirileceğini açıklamaktadır.

## İçindekiler

1. [Genel Bakış](#genel-bakış)
2. [Dosyalar ve Yapı](#dosyalar-ve-yapı)
3. [Stok İşlemleri](#stok-işlemleri)
4. [Satış Entegrasyonu](#satış-entegrasyonu)
5. [Geliştirme Kılavuzu](#geliştirme-kılavuzu)

## Genel Bakış

Stok modülü, fabrika yönetim sisteminde ürünlerin depolar bazında stok takibini yapmak ve stok hareketlerini kaydetmek için geliştirilmiştir. Bu modül sayesinde:

- Ürünlerin güncel stok miktarlarını depolar bazında takip edebilirsiniz
- Satış işlemlerinde otomatik stok çıkışı yapılır
- Stok giriş, çıkış, transfer işlemlerini yapabilirsiniz
- Stok hareketlerinin tam kaydını tutabilirsiniz

## Dosyalar ve Yapı

Stok modülü aşağıdaki dosyalardan oluşmaktadır:

1. **Veritabanı Tabloları**
   - `DepoStok` - Güncel stok durumlarını tutar
   - `StokHareketleri` - Stok hareket geçmişini tutar

2. **Stored Procedure**
   - `SP_StokHareket.sql` - Stok hareketlerini işleyen prosedür

3. **C# Kodları**
   - `StokHelper.cs` - Stok işlemlerini kolaylaştıran yardımcı sınıf
   - `StokOrnekleri.cs` - Örnek kullanımlar

4. **Dokümantasyon**
   - `StokIslemleriDokumantasyon.txt` - Detaylı teknik dokümantasyon
   - `StokYonetimOzeti.md` - Kısa özet bilgiler
   - `README_StokModulu.md` - Bu dosya

## Stok İşlemleri

### 1. Stok Durumu Sorgulama

Bir ürünün stok durumunu sorgulamak için `StokHelper.GetStokMiktari` metodunu kullanabilirsiniz:

```csharp
decimal mevcutStok = StokHelper.GetStokMiktari(depoID, urunID);
```

### 2. Stok Girişi Yapma

Manuel stok girişi için `StokHelper.StokHareketiYap` metodunu kullanabilirsiniz:

```csharp
bool sonuc = StokHelper.StokHareketiYap(
    sirketID,
    "GIRIS",
    depoID,
    urunID,
    miktar,     // Pozitif değer
    "MANUEL",   // Referans tipi
    null,       // Referans ID
    aciklama,
    kullaniciID
);
```

### 3. Stok Çıkışı Yapma

Manuel stok çıkışı için yine `StokHelper.StokHareketiYap` metodunu kullanabilirsiniz:

```csharp
bool sonuc = StokHelper.StokHareketiYap(
    sirketID,
    "CIKIS",
    depoID,
    urunID,
    miktar * -1,   // Negatif değer (çıkış işlemi)
    "MANUEL",
    null,
    aciklama,
    kullaniciID
);
```

### 4. Depolar Arası Transfer

Ürünleri depolar arasında transfer etmek için `StokOrnekleri.DepoTransferi` metodunu örnek alabilirsiniz:

```csharp
bool sonuc = DepoTransferi(
    sirketID,
    kaynakDepoID, 
    hedefDepoID, 
    urunID, 
    miktar, 
    aciklama
);
```

## Satış Entegrasyonu

Satış işlemi sırasında stok çıkışları otomatik olarak oluşturulmaktadır. `MusteriSatis.aspx.cs` içindeki `btnFaturaKaydet_Click` metodunda satış kaydedildikten sonra stok çıkışı yapılır:

```csharp
// Veritabanı işlemleri tamamlandıktan sonra stok hareketi oluştur
bool stokSonuc = StokHelper.SatisStokCikisiYap(
    sirketID, 
    yeniSatis.SatisID, 
    satisDetaylari, 
    GetKullaniciID()
);
```

## Geliştirme Kılavuzu

### Yeni Stok Hareket Tipi Ekleme

Yeni bir stok hareket tipi eklemek için:

1. `SP_StokHareket` prosedüründe kontrol listesine yeni tipleri ekleyin
2. `StokHelper` sınıfına yeni hareket tipi için yardımcı metod ekleyin

### Stok Kontrolü Ekleme

Satış öncesi stok kontrolü eklemek için `SatisOncesiStokKontrolu` metodunu örnek alarak `MusteriSatis.aspx.cs` içindeki `btnFaturaKaydet_Click` metoduna stok kontrolü ekleyebilirsiniz:

```csharp
// Satış öncesi stok kontrolü
if (!StokOrnekleri.SatisOncesiStokKontrolu((List<SepetItem>)Session["Sepet"]))
{
    ScriptManager.RegisterStartupScript(this, this.GetType(), "stokYetersiz", 
        "alert('Bazı ürünler için stok yetersiz. Lütfen miktarları kontrol ediniz.');", true);
    return;
}
```

### Stok Raporları Ekleme

Stok raporları oluşturmak için `StokOrnekleri.GetStokDurumuRaporu` metodunu örnek alabilirsiniz. Yeni raporlar için benzer SQL sorguları hazırlayabilirsiniz.

---

## Geliştirme Tavsiyeleri

- Kritik stok seviyesi kontrolü ve bildirim sistemi eklenebilir
- Toplu stok girişi/çıkışı için arayüz geliştirilebilir
- Stok hareketlerini görüntülemek için rapor sayfası eklenebilir
- Ürün bazlı stok geçmişi görüntüleme eklenebilir
- Stok maliyeti hesaplama ve raporlama eklenebilir

---

Son güncelleme: 30.07.2024 