# Menü Yönetimi Modülü

Bu modül, Zeytin Fabrikası sistemindeki dinamik menü yapısını yönetmenizi sağlar. Kullanıcı yetkilerine göre menülerin gösterilmesini kontrol eder ve tüm menü öğelerini düzenleme imkanı sunar.

## Kurulum

1. Veritabanı tablolarını oluşturmak için 'Menu_Tablo_Olustur.sql' scriptini SQL Server Management Studio'da çalıştırın.
2. Bu script aşağıdaki işlemleri gerçekleştirir:
   - Menu tablosunu oluşturur
   - KullaniciYetki tablosunu oluşturur (yoksa)
   - Örnek menü verilerini ekler

## Menü Yönetimi Sayfası Kullanımı

### Menü Görünümleri
- **Ağaç Görünümü**: Menüleri hiyerarşik yapıda gösterir
- **Tablo Görünümü**: Tüm menüleri liste halinde gösterir

### Menü Ekleme
1. "Yeni Menü Ekle" formunu kullanın
2. Üst menüyü seçin (Ana Menü = üst menü yok)
3. Menü adı, ikon, URL ve sıra bilgilerini girin
4. Yetki kodunu belirleyin - Bu kod kullanıcıların menüye erişim yetkisini kontrol eder
5. Kaydet butonuna tıklayın

### Menü Düzenleme
1. Düzenlemek istediğiniz menüyü ağaç veya tablo görünümünden seçin
2. Bilgileri düzenleyin ve Kaydet butonuna tıklayın

### Menü Silme
- Menüyü silmek için tablo görünümündeki çöp kutusu ikonuna tıklayın
- **Dikkat**: Alt menüsü olan bir menüyü silemezsiniz, önce alt menüleri silmelisiniz

## İkonlar

Menü ikonları için Font Awesome kütüphanesi kullanılmaktadır. Bazı yaygın ikonlar:

- fa-home: Ana sayfa
- fa-users: Kullanıcılar
- fa-file-text-o: Belgeler
- fa-money: Ödemeler
- fa-cubes: Stok
- fa-bar-chart-o: Raporlar
- fa-cogs: Ayarlar
- fa-list: Listeler
- fa-plus-square: Yeni ekle

Daha fazla ikon için: [Font Awesome](https://fontawesome.com/v4/icons/)

## Menü Yetkilendirme Sistemi

1. Her menü öğesi bir YetkiKodu ile ilişkilendirilir
2. Kullanıcıların yetkileri KullaniciYetki tablosunda saklanır
3. Kullanıcı, sadece yetkisi olan menüleri görür
4. Yeni kullanıcı oluşturulduğunda, YetkiHelper.TumMenuYetkileriniVer metodu çağrılarak tüm menü yetkileri verilebilir
5. Kullanıcı Yönetimi sayfasından kullanıcının menü yetkileri düzenlenebilir 