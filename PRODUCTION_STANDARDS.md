# PRODUCTION STANDARTLARI

## ✅ YAPILDI: Debug.WriteLine Temizliği

### Düzeltilen Dosyalar:
- ✅ `WebSite/fabrika/FabrikaMasterPage.master.cs` - 8 adet Debug.WriteLine kaldırıldı
- ✅ `WebSite/giris.aspx.cs` - 2 adet Debug.WriteLine kaldırıldı  
- ✅ `WebSite/App_Code/MessageHelper.cs` - Debug.WriteLine → EventLog ile değiştirildi
- ✅ `WebSite/App_Code/SessionHelper.cs` - 5 adet Debug.WriteLine kaldırıldı
- ✅ `WebSite/fabrika/Fabrika_customErrors.aspx.cs` - Hata yönetimi iyileştirildi

## 📋 KALAN GÖREVLER

### Critical - Hemen Yapılması Gerekenler:
1. **Diğer dosyalardaki Debug.WriteLine temizliği**:
   - `WebSite/Mobil/MalKabul.aspx.cs` (45+ adet Debug.WriteLine)
   - `WebSite/Mobil/Login.aspx.cs`
   - `WebSite/Mobil/MobilMasterPage.master.cs`
   - `WebSite/yonetim/` klasöründeki dosyalar

2. **ErrorLogs Tablosu Oluşturma**:
```sql
CREATE TABLE ErrorLogs (
    ID int IDENTITY(1,1) PRIMARY KEY,
    ErrorDate datetime NOT NULL,
    UserName nvarchar(100),
    ErrorMessage nvarchar(MAX),
    StackTrace nvarchar(MAX),
    PageUrl nvarchar(500),
    UserAgent nvarchar(500)
);
```

### Genel Standartlar:

#### ✅ KULLANILMALI:
- **MessageHelper.ShowSuccessMessage()** - Başarı mesajları için
- **MessageHelper.ShowErrorMessage()** - Hata mesajları için
- **MessageHelper.ShowWarningMessage()** - Uyarı mesajları için
- **MessageHelper.LogError()** - Hata loglama için

#### ❌ KULLANILMAMALI:
- ~~System.Diagnostics.Debug.WriteLine()~~ - Production'da görünmez
- ~~alert()~~ - Eski ve kullanıcı dostu değil
- ~~ScriptManager.RegisterStartupScript~~ (doğrudan) - MessageHelper kullan

#### 🔧 C# 4.0 Uyumluluğu:
- ❌ String interpolation ($"") kullanma
- ❌ Null-conditional operators (?.) kullanma  
- ❌ Expression-bodied members (=>) kullanma
- ✅ string.Format() veya + operatörü kullan
- ✅ Geleneksel if null kontrolleri kullan

## 📊 İstatistikler

### Debug.WriteLine Kullanım Durumu:
- **Temizlenen**: 22 adet (temel authentication dosyaları)
- **Kalan**: ~100+ adet (mobil ve yönetim modülleri)
- **Öncelik**: Kritik olan dosyalar temizlendi

### Sonraki Adımlar:
1. Mobil modülündeki Debug.WriteLine temizliği
2. Yönetim panelindeki Debug.WriteLine temizliği  
3. ErrorLogs tablosu kurulumu ve test
4. Production deployment öncesi final kontrol 