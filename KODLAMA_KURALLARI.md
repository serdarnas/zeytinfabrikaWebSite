# 📋 Kodlama Kuralları - Zeytin Fabrikası Projesi

## 🔧 Teknik Ortam
- **Microsoft .NET Framework Sürümü**: 4.0.30319
- **ASP.NET Sürümü**: 4.8.9282.0
- **C# Sürümü**: 4.0 (C# 6.0 özelliklerini desteklemez)

## ⚠️ KULLANILMAYACAK ÖZELLİKLER

### 1. String Interpolation (C# 6.0+)
```csharp
// ❌ YANLIŞ - Derleme hatası verir
string mesaj = $"Kullanıcı: {kullaniciAdi}, Yaş: {yas}";

// ✅ DOĞRU - String concatenation kullan
string mesaj = "Kullanıcı: " + kullaniciAdi + ", Yaş: " + yas.ToString();

// ✅ DOĞRU - String.Format kullan
string mesaj = string.Format("Kullanıcı: {0}, Yaş: {1}", kullaniciAdi, yas);
```

### 2. Null-Conditional Operators (C# 6.0+)
```csharp
// ❌ YANLIŞ - Derleme hatası verir
string uzunluk = user?.Name?.Length.ToString();

// ✅ DOĞRU - Geleneksel null kontrol
string uzunluk = "";
if (user != null && user.Name != null)
{
    uzunluk = user.Name.Length.ToString();
}
```

### 3. Expression-bodied Members (C# 6.0+)
```csharp
// ❌ YANLIŞ - Derleme hatası verir
public string GetFullName() => FirstName + " " + LastName;

// ✅ DOĞRU - Geleneksel metod
public string GetFullName()
{
    return FirstName + " " + LastName;
}
```

### 4. Auto-property Initializers (C# 6.0+)
```csharp
// ❌ YANLIŞ - Derleme hatası verir
public string Name { get; set; } = "Varsayılan";

// ✅ DOĞRU - Constructor'da initialize et
public string Name { get; set; }

public MyClass()
{
    Name = "Varsayılan";
}
```

## ✅ KULLANILACAK YÖNTEMLERİ

### 1. String İşlemleri
```csharp
// String birleştirme
string sonuc = "Merhaba " + isim + "!";

// Format kullanımı
string sonuc = string.Format("Merhaba {0}!", isim);

// StringBuilder (çok string birleştirme için)
StringBuilder sb = new StringBuilder();
sb.AppendLine("Satır 1");
sb.AppendLine("Satır 2");
string sonuc = sb.ToString();
```

### 2. Sayı Formatları
```csharp
// Ondalık formatı
decimal fiyat = 123.456m;
string formatliFiyat = fiyat.ToString("F2"); // 123.46

// Para formatı
string paraFormati = fiyat.ToString("C"); // ₺123.46

// Yüzde formatı
decimal oran = 0.1534m;
string yuzde = oran.ToString("P2"); // %15.34
```

### 3. Null Kontrolleri
```csharp
// String null kontrolü
if (!string.IsNullOrEmpty(degisken))
{
    // güvenli kullanım
}

// Object null kontrolü
if (nesne != null)
{
    // güvenli kullanım
}
```

### 4. Collection İşlemleri
```csharp
// LINQ kullanımı (.NET 3.5+)
var filtreliListe = liste.Where(x => x.Durum == true).ToList();

// Foreach döngü
foreach (var item in liste)
{
    // işlem
}

// For döngü
for (int i = 0; i < liste.Count; i++)
{
    var item = liste[i];
    // işlem
}
```

## 📝 Önerilen Kullanım Şablonları

### MessageHelper Kullanımı
```csharp
// Başarı mesajı
MessageHelper.ShowSuccessMessage(this, "Başlık", "Mesaj içeriği");

// Hata mesajı
MessageHelper.ShowErrorMessage(this, "Hata", "Hata açıklaması");

// Uyarı mesajı
MessageHelper.ShowWarningMessage(this, "Uyarı", "Uyarı mesajı");

// Bilgi mesajı
MessageHelper.ShowInfoMessage(this, "Bilgi", "Bilgi mesajı");
```

### Veritabanı İşlemleri
```csharp
try
{
    using (FabrikaDataClassesDataContext db = new FabrikaDataClassesDataContext())
    {
        // Veritabanı işlemleri
        db.SubmitChanges();
    }
}
catch (Exception ex)
{
    MessageHelper.ShowErrorMessage(this, "Veritabanı Hatası", "İşlem sırasında hata oluştu: " + ex.Message);
}
```

### Exception Handling
```csharp
try
{
    // Riskli kod
}
catch (Exception ex)
{
    // Log yazma
    System.Diagnostics.Debug.WriteLine("Hata: " + ex.Message);
    
    // Kullanıcıya mesaj
    MessageHelper.ShowErrorMessage(this, "Hata", "İşlem başarısız: " + ex.Message);
}
```

## 🚫 Yaygın Hatalar ve Çözümleri

### 1. CS1056: Beklenmeyen karakter '$'
```csharp
// ❌ HATA YAPAN KOD
string mesaj = $"Değer: {deger}";

// ✅ ÇÖZÜM
string mesaj = "Değer: " + deger.ToString();
```

### 2. CS1525: Geçersiz expression term
```csharp
// ❌ HATA YAPAN KOD
public int Toplam => a + b;

// ✅ ÇÖZÜM
public int Toplam 
{
    get { return a + b; }
}
```

### 3. CS1061: '?' does not contain a definition
```csharp
// ❌ HATA YAPAN KOD
int? uzunluk = text?.Length;

// ✅ ÇÖZÜM
int? uzunluk = null;
if (text != null)
{
    uzunluk = text.Length;
}
```

## 📚 Faydalı Kaynaklar

- [.NET Framework 4.0 Özellikleri](https://docs.microsoft.com/en-us/dotnet/framework/whats-new/whats-new-in-dotnet-framework-4)
- [C# 4.0 Dil Özellikleri](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-4)
- [ASP.NET Web Forms](https://docs.microsoft.com/en-us/aspnet/web-forms/)

---
**⚠️ DİKKAT**: Bu kurallar mutlaka takip edilmelidir. Aksi halde CS1056, CS1525 gibi derleme hataları alırsınız! 