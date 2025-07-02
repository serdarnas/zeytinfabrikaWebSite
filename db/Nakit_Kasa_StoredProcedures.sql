-- =============================================
-- Nakit Kasa Yönetimi - Sadece Stored Procedure'lar
-- Tarih: 2024
-- Not: Tablolar zaten database'de mevcut
-- =============================================

-- Kasa bakiyesini güncelleyen prosedür
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_KasaBakiyeGuncelle' AND xtype='P')
BEGIN
    DROP PROCEDURE sp_KasaBakiyeGuncelle
END
GO

CREATE PROCEDURE sp_KasaBakiyeGuncelle
    @KasaID INT,
    @IslemTipi CHAR(1), -- 'G': Giriş, 'C': Çıkış
    @Tutar DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @IslemTipi = 'G'
        UPDATE Kasalar SET Bakiye = Bakiye + @Tutar WHERE KasaID = @KasaID
    ELSE IF @IslemTipi = 'C'
        UPDATE Kasalar SET Bakiye = Bakiye - @Tutar WHERE KasaID = @KasaID
    
    SELECT Bakiye FROM Kasalar WHERE KasaID = @KasaID
END
GO

-- Kasa virmanı yapan prosedür
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_KasaVirmanYap' AND xtype='P')
BEGIN
    DROP PROCEDURE sp_KasaVirmanYap
END
GO

CREATE PROCEDURE sp_KasaVirmanYap
    @SirketID INT,
    @KaynakKasaID INT,
    @HedefKasaID INT,
    @Tutar DECIMAL(18,2),
    @Aciklama NVARCHAR(500),
    @KullaniciID INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    
    DECLARE @VirmanID INT;
    
    -- Virman kaydı oluştur
    INSERT INTO KasaVirman (SirketID, IslemTarihi, KaynakKasaID, HedefKasaID, Tutar, Aciklama, KullaniciID)
    VALUES (@SirketID, GETDATE(), @KaynakKasaID, @HedefKasaID, @Tutar, @Aciklama, @KullaniciID);
    
    SET @VirmanID = SCOPE_IDENTITY();
    
    -- Kaynak kasadan çıkış hareketi
    INSERT INTO KasaHareketler (SirketID, KasaID, IslemTarihi, IslemTipi, Tutar, ReferansTipi, ReferansID, Aciklama, KullaniciID)
    VALUES (@SirketID, @KaynakKasaID, GETDATE(), 'C', @Tutar, 'Virman', @VirmanID, 'Transfer - ' + @Aciklama, @KullaniciID);
    
    -- Hedef kasaya giriş hareketi
    INSERT INTO KasaHareketler (SirketID, KasaID, IslemTarihi, IslemTipi, Tutar, ReferansTipi, ReferansID, Aciklama, KullaniciID)
    VALUES (@SirketID, @HedefKasaID, GETDATE(), 'G', @Tutar, 'Virman', @VirmanID, 'Transfer - ' + @Aciklama, @KullaniciID);
    
    -- Kasa bakiyelerini güncelle
    UPDATE Kasalar SET Bakiye = Bakiye - @Tutar WHERE KasaID = @KaynakKasaID;
    UPDATE Kasalar SET Bakiye = Bakiye + @Tutar WHERE KasaID = @HedefKasaID;
    
    COMMIT TRANSACTION;
    
    SELECT @VirmanID AS VirmanID, 'İşlem başarılı' AS Mesaj;
END
GO

-- Nakit işlem kaydeden prosedür
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_NakitIslemKaydet' AND xtype='P')
BEGIN
    DROP PROCEDURE sp_NakitIslemKaydet
END
GO

CREATE PROCEDURE sp_NakitIslemKaydet
    @SirketID INT,
    @IslemTuru CHAR(1), -- 'T': Tahsilat, 'O': Ödeme
    @MusteriID INT = NULL,
    @TedarikciID INT = NULL,
    @MustahsilID INT = NULL,
    @Tutar DECIMAL(18,2),
    @ParaBirimiID INT,
    @OdemeTipiID INT,
    @BankaHesapID INT = NULL,
    @ReferansNo NVARCHAR(50) = NULL,
    @ReferansTipi NVARCHAR(20) = NULL,
    @ReferansID INT = NULL,
    @Aciklama NVARCHAR(500) = NULL,
    @KullaniciID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    
    DECLARE @NakitIslemID INT;
    
    -- Nakit işlem kaydı oluştur
    INSERT INTO NakitIslemler (
        SirketID, IslemTuru, IslemTarihi, MusteriID, TedarikciID, MustahsilID,
        Tutar, ParaBirimiID, OdemeTipiID, BankaHesapID, ReferansNo, ReferansTipi,
        ReferansID, Aciklama, KullaniciID
    )
    VALUES (
        @SirketID, @IslemTuru, GETDATE(), @MusteriID, @TedarikciID, @MustahsilID,
        @Tutar, @ParaBirimiID, @OdemeTipiID, @BankaHesapID, @ReferansNo, @ReferansTipi,
        @ReferansID, @Aciklama, @KullaniciID
    );
    
    SET @NakitIslemID = SCOPE_IDENTITY();
    
    -- Eğer nakit ödeme ise (OdemeTipiID = 1), kasaya da hareket kaydı oluştur
    IF @OdemeTipiID = 1
    BEGIN
        DECLARE @KasaID INT;
        
        -- Ana nakit kasayı bul
        SELECT TOP 1 @KasaID = KasaID 
        FROM Kasalar 
        WHERE SirketID = @SirketID AND KasaTipi = 'F' AND AktifMi = 1
        ORDER BY KasaID;
        
        IF @KasaID IS NOT NULL
        BEGIN
            -- Kasa hareketi kaydet
            INSERT INTO KasaHareketler (SirketID, KasaID, IslemTarihi, IslemTipi, Tutar, ReferansTipi, ReferansID, Aciklama, KullaniciID)
            VALUES (@SirketID, @KasaID, GETDATE(), @IslemTuru, @Tutar, 'NakitIslem', @NakitIslemID, @Aciklama, @KullaniciID);
            
            -- Kasa bakiyesini güncelle
            IF @IslemTuru = 'T'
                UPDATE Kasalar SET Bakiye = Bakiye + @Tutar WHERE KasaID = @KasaID;
            ELSE
                UPDATE Kasalar SET Bakiye = Bakiye - @Tutar WHERE KasaID = @KasaID;
        END
    END
    
    COMMIT TRANSACTION;
    
    SELECT @NakitIslemID AS NakitIslemID, 'İşlem başarılı' AS Mesaj;
END
GO

-- Kasa istatistiklerini getiren prosedür
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_KasaIstatistikGetir' AND xtype='P')
BEGIN
    DROP PROCEDURE sp_KasaIstatistikGetir
END
GO

CREATE PROCEDURE sp_KasaIstatistikGetir
    @SirketID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        -- Kasa sayıları
        COUNT(*) AS ToplamKasaSayisi,
        SUM(CASE WHEN KasaTipi = 'F' THEN 1 ELSE 0 END) AS FizikselKasaSayisi,
        SUM(CASE WHEN KasaTipi = 'D' THEN 1 ELSE 0 END) AS DijitalKasaSayisi,
        SUM(CASE WHEN AktifMi = 1 THEN 1 ELSE 0 END) AS AktifKasaSayisi,
        
        -- Bakiye toplamları
        SUM(CASE WHEN KasaTipi = 'F' THEN Bakiye ELSE 0 END) AS ToplamNakitBakiye,
        SUM(CASE WHEN KasaTipi = 'D' THEN Bakiye ELSE 0 END) AS ToplamBankaBakiye,
        SUM(Bakiye) AS GenelToplamBakiye
        
    FROM Kasalar 
    WHERE SirketID = @SirketID;
    
    -- En çok kullanılan kasalar
    SELECT TOP 5
        k.KasaAdi,
        k.KasaTipi,
        k.Bakiye,
        COUNT(kh.HareketID) AS HareketSayisi,
        MAX(kh.IslemTarihi) AS SonIslemTarihi
    FROM Kasalar k
    LEFT JOIN KasaHareketler kh ON k.KasaID = kh.KasaID
    WHERE k.SirketID = @SirketID
    GROUP BY k.KasaID, k.KasaAdi, k.KasaTipi, k.Bakiye
    ORDER BY COUNT(kh.HareketID) DESC;
END
GO

-- Örnek kasa verilerini oluşturan prosedür
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_OrnekKasalarOlustur' AND xtype='P')
BEGIN
    DROP PROCEDURE sp_OrnekKasalarOlustur
END
GO

CREATE PROCEDURE sp_OrnekKasalarOlustur
    @SirketID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Mevcut kasaları kontrol et
    IF NOT EXISTS (SELECT 1 FROM Kasalar WHERE SirketID = @SirketID)
    BEGIN
        DECLARE @ParaBirimiID INT;
        
        -- TL para birimini bul (gerçek sütun adları: ParaBirimiAd, ParaBirimi)
        SELECT @ParaBirimiID = ParaBirimiID 
        FROM ParaBirimileri 
        WHERE ParaBirimi = 'TL' OR ParaBirimiAd LIKE '%Türk%' OR ParaBirimiAd LIKE '%Lira%';
        
        IF @ParaBirimiID IS NULL
            SELECT TOP 1 @ParaBirimiID = ParaBirimiID FROM ParaBirimileri;
        
        -- Örnek kasalar oluştur
        INSERT INTO Kasalar (SirketID, KasaKodu, KasaAdi, KasaTipi, ParaBirimiID, Bakiye, Aciklama, OlusturmaTarihi, AktifMi)
        VALUES 
        (@SirketID, 'KASA001', 'Ana Nakit Kasa', 'F', @ParaBirimiID, 0, 'Günlük nakit işlemler için ana kasa', GETDATE(), 1),
        (@SirketID, 'BANKA001', 'Ana Banka Hesabı', 'D', @ParaBirimiID, 0, 'Ana banka hesabı için dijital kasa', GETDATE(), 1),
        (@SirketID, 'KASA002', 'Yedek Nakit Kasa', 'F', @ParaBirimiID, 0, 'Yedek nakit işlemler için', GETDATE(), 1);
    END
END
GO

-- Örnek banka hesapları oluşturan prosedür
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_OrnekBankaHesaplariOlustur' AND xtype='P')
BEGIN
    DROP PROCEDURE sp_OrnekBankaHesaplariOlustur
END
GO

CREATE PROCEDURE sp_OrnekBankaHesaplariOlustur
    @SirketID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Mevcut banka hesaplarını kontrol et
    IF NOT EXISTS (SELECT 1 FROM BankaHesaplar WHERE SirketID = @SirketID)
    BEGIN
        DECLARE @ParaBirimiID INT;
        
        -- TL para birimini bul
        SELECT @ParaBirimiID = ParaBirimiID 
        FROM ParaBirimileri 
        WHERE ParaBirimi = 'TL' OR ParaBirimiAd LIKE '%Türk%' OR ParaBirimiAd LIKE '%Lira%';
        
        IF @ParaBirimiID IS NULL
            SELECT TOP 1 @ParaBirimiID = ParaBirimiID FROM ParaBirimileri;
        
        -- Örnek banka hesapları oluştur
        INSERT INTO BankaHesaplar (SirketID, HesapAdi, BankaAdi, SubeAdi, HesapNo, IBAN, ParaBirimiID, Aciklama)
        VALUES 
        (@SirketID, 'Ana Hesap', 'Türkiye İş Bankası', 'Merkez Şubesi', '1234567890', 'TR320006400000011234567890', @ParaBirimiID, 'Ticari işlemler için ana banka hesabı'),
        (@SirketID, 'Yedek Hesap', 'Garanti BBVA', 'Merkez Şubesi', '9876543210', 'TR120006200000119876543210', @ParaBirimiID, 'Yedek işlemler için banka hesabı'),
        (@SirketID, 'Döviz Hesap', 'Vakıfbank', 'Merkez Şubesi', '5555666677', 'TR640001500000155556666777', @ParaBirimiID, 'Dövizli işlemler için');
    END
END
GO

-- Tam sistem başlatma prosedürü (yeni şirketler için)
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_SirketNakitSistemiBaslat' AND xtype='P')
BEGIN
    DROP PROCEDURE sp_SirketNakitSistemiBaslat
END
GO

CREATE PROCEDURE sp_SirketNakitSistemiBaslat
    @SirketID INT,
    @TestVerisiEkle BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    PRINT 'Şirket ' + CAST(@SirketID AS VARCHAR(10)) + ' için nakit sistemi başlatılıyor...';
    
    -- 1. Örnek kasalar oluştur
    EXEC sp_OrnekKasalarOlustur @SirketID;
    PRINT '✓ Örnek kasalar oluşturuldu';
    
    -- 2. Örnek banka hesapları oluştur
    EXEC sp_OrnekBankaHesaplariOlustur @SirketID;
    PRINT '✓ Örnek banka hesapları oluşturuldu';
    
    -- 3. Test verisi ekle (istenirse)
    IF @TestVerisiEkle = 1
    BEGIN
        DECLARE @KasaID INT, @BankaHesapID INT, @ParaBirimiID INT;
        
        -- Ana kasayı bul
        SELECT TOP 1 @KasaID = KasaID FROM Kasalar WHERE SirketID = @SirketID AND KasaTipi = 'F';
        
        -- Ana banka hesabını bul
        SELECT TOP 1 @BankaHesapID = BankaHesapID FROM BankaHesaplar WHERE SirketID = @SirketID;
        
        -- TL para birimini bul
        SELECT @ParaBirimiID = ParaBirimiID 
        FROM ParaBirimileri 
        WHERE ParaBirimi = 'TL' OR ParaBirimiAd LIKE '%Türk%';
        
        IF @ParaBirimiID IS NULL
            SELECT TOP 1 @ParaBirimiID = ParaBirimiID FROM ParaBirimileri;
        
        -- Test hareketleri
        IF @KasaID IS NOT NULL
        BEGIN
            -- Başlangıç nakdi
            INSERT INTO KasaHareketler (SirketID, KasaID, IslemTarihi, IslemTipi, Tutar, Aciklama)
            VALUES (@SirketID, @KasaID, GETDATE(), 'G', 1000.00, 'Başlangıç nakdi');
            
            UPDATE Kasalar SET Bakiye = 1000.00 WHERE KasaID = @KasaID;
            
            -- Test gideri
            INSERT INTO KasaHareketler (SirketID, KasaID, IslemTarihi, IslemTipi, Tutar, Aciklama)
            VALUES (@SirketID, @KasaID, GETDATE(), 'C', 150.00, 'Test masraf');
            
            UPDATE Kasalar SET Bakiye = Bakiye - 150.00 WHERE KasaID = @KasaID;
        END
        
        PRINT '✓ Test verileri eklendi';
    END
    
    PRINT '🎉 Şirket nakit sistemi başarıyla kuruldu!';
    
    -- Özet bilgi
    SELECT 
        s.SirketAdi,
        COUNT(k.KasaID) AS OlusturulanKasaSayisi,
        COUNT(bh.BankaHesapID) AS OlusturulanBankaHesapSayisi,
        SUM(k.Bakiye) AS ToplamKasaBakiye
    FROM Sirketler s
    LEFT JOIN Kasalar k ON s.SirketID = k.SirketID
    LEFT JOIN BankaHesaplar bh ON s.SirketID = bh.SirketID
    WHERE s.SirketID = @SirketID
    GROUP BY s.SirketID, s.SirketAdi;
END
GO

-- Tüm şirketler için toplu başlatma
IF EXISTS (SELECT * FROM sysobjects WHERE name='sp_TumSirketlerNakitSistemiBaslat' AND xtype='P')
BEGIN
    DROP PROCEDURE sp_TumSirketlerNakitSistemiBaslat
END
GO

CREATE PROCEDURE sp_TumSirketlerNakitSistemiBaslat
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @SirketID INT;
    DECLARE sirket_cursor CURSOR FOR 
        SELECT SirketID FROM Sirketler WHERE Aktif = 1;
    
    OPEN sirket_cursor;
    FETCH NEXT FROM sirket_cursor INTO @SirketID;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Her şirket için nakit sistemi kur (test verisi olmadan)
        EXEC sp_SirketNakitSistemiBaslat @SirketID, 0;
        
        FETCH NEXT FROM sirket_cursor INTO @SirketID;
    END
    
    CLOSE sirket_cursor;
    DEALLOCATE sirket_cursor;
    
    PRINT 'Tüm aktif şirketler için nakit sistemi kuruldu!';
END
GO

-- Test için örnek data
PRINT '
=============================================
KULLANIM ÖRNEKLERİ:
=============================================

-- Yeni şirket için nakit sistemi kurmak:
EXEC sp_SirketNakitSistemiBaslat @SirketID = 1, @TestVerisiEkle = 1;

-- Sadece kasalar oluşturmak:
EXEC sp_OrnekKasalarOlustur @SirketID = 1;

-- Kasa istatistikleri:
EXEC sp_KasaIstatistikGetir @SirketID = 1;

-- Transfer yapmak:
EXEC sp_KasaVirmanYap @SirketID=1, @KaynakKasaID=1, @HedefKasaID=2, @Tutar=500, @Aciklama=''Transfer'', @KullaniciID=1;

-- Nakit tahsilat:
EXEC sp_NakitIslemKaydet @SirketID=1, @IslemTuru=''T'', @MusteriID=1, @Tutar=1000, @ParaBirimiID=1, @OdemeTipiID=1, @Aciklama=''Tahsilat'';

-- Toplu kurulum:
EXEC sp_TumSirketlerNakitSistemiBaslat;

=============================================
Tüm stored procedure''lar başarıyla oluşturuldu!
'; 