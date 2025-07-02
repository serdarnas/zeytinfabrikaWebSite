CREATE PROCEDURE [dbo].[SP_StokHareket]
    @SirketID INT,
    @HareketTipi NVARCHAR(20),
    @DepoID INT,
    @UrunID INT,
    @Miktar DECIMAL(10,2),
    @ReferansNo NVARCHAR(50) = NULL,
    @ReferansID INT = NULL,
    @ReferansTipi NVARCHAR(20) = NULL,
    @Aciklama NVARCHAR(500) = NULL,
    @KullaniciID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Error INT = 0
    DECLARE @NewMiktar DECIMAL(10,2)
    DECLARE @GercekMiktar DECIMAL(10,2) = @Miktar
    
    BEGIN TRY
        BEGIN TRANSACTION
        
        -- If it's a stock out operation, make the quantity negative
        IF @HareketTipi = 'CIKIS' OR @HareketTipi = 'SATIS'
        BEGIN
            SET @GercekMiktar = -1 * ABS(@Miktar)
        END
        
        -- Insert the stock movement record
        INSERT INTO StokHareketleri (
            SirketID, IslemTarihi, HareketTipi, DepoID, UrunID, Miktar, 
            ReferansNo, ReferansID, ReferansTipi, Aciklama, KullaniciID
        ) VALUES (
            @SirketID, GETDATE(), @HareketTipi, @DepoID, @UrunID, @GercekMiktar,
            @ReferansNo, @ReferansID, @ReferansTipi, @Aciklama, @KullaniciID
        )
        
        -- Check if a stock record exists for this product in this warehouse
        IF EXISTS (SELECT 1 FROM DepoStok WHERE DepoID = @DepoID AND UrunID = @UrunID)
        BEGIN
            -- Update existing stock record
            UPDATE DepoStok 
            SET Miktar = Miktar + @GercekMiktar,
                SonGuncellemeTarihi = GETDATE()
            WHERE DepoID = @DepoID AND UrunID = @UrunID
            
            -- Get the new stock level
            SELECT @NewMiktar = Miktar FROM DepoStok WHERE DepoID = @DepoID AND UrunID = @UrunID
            
            -- Check if stock level is negative after update (only for non-service products)
            IF @NewMiktar < 0 AND EXISTS (SELECT 1 FROM Urunler WHERE UrunID = @UrunID AND UrunTipiStoklu = 1)
            BEGIN
                RAISERROR('Yeterli stok miktarı bulunmamaktadır. Stok miktarı negatif olamaz.', 16, 1)
            END
        END
        ELSE
        BEGIN
            -- Create a new stock record if it doesn't exist
            INSERT INTO DepoStok (SirketID, DepoID, UrunID, Miktar, SonGuncellemeTarihi)
            VALUES (@SirketID, @DepoID, @UrunID, @GercekMiktar, GETDATE())
            
            -- Check if stock level is negative for out transactions
            IF @GercekMiktar < 0 AND EXISTS (SELECT 1 FROM Urunler WHERE UrunID = @UrunID AND UrunTipiStoklu = 1)
            BEGIN
                RAISERROR('Yeterli stok miktarı bulunmamaktadır. Ürün depoda mevcut değil.', 16, 1)
            END
        END
        
        -- Update the total stock amount in the Urunler table
        UPDATE Urunler
        SET StokMiktari = (SELECT SUM(Miktar) FROM DepoStok WHERE UrunID = @UrunID)
        WHERE UrunID = @UrunID
        
        COMMIT TRANSACTION
        
        -- Return the new stock level
        SELECT @NewMiktar AS YeniStokMiktari
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        SET @Error = ERROR_NUMBER()
        
        -- Return error information
        SELECT 
            ERROR_NUMBER() AS ErrorNumber,
            ERROR_MESSAGE() AS ErrorMessage
            
        RETURN @Error
    END CATCH
    
    RETURN 0
END 