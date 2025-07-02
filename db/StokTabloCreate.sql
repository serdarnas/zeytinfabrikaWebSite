-- Create DepoStok (Warehouse Stock) table
CREATE TABLE dbo.DepoStok (
    DepoStokID INT IDENTITY(1,1) PRIMARY KEY,
    SirketID INT NOT NULL,
    DepoID INT NOT NULL,
    UrunID INT NOT NULL,
    Miktar DECIMAL(10,2) NOT NULL DEFAULT 0,
    MinimumMiktar DECIMAL(10,2) NULL,
    SonGuncellemeTarihi DATETIME NULL,
    CONSTRAINT FK_DepoStok_Sirketler FOREIGN KEY (SirketID) REFERENCES dbo.Sirketler(SirketID),
    CONSTRAINT FK_DepoStok_Depolar FOREIGN KEY (DepoID) REFERENCES dbo.Depolar(DepoID),
    CONSTRAINT FK_DepoStok_Urunler FOREIGN KEY (UrunID) REFERENCES dbo.Urunler(UrunID),
    CONSTRAINT UQ_DepoStok_Depo_Urun UNIQUE (DepoID, UrunID)
);

-- Create StokHareketleri (Stock Movements) table
CREATE TABLE dbo.StokHareketleri (
    HareketID INT IDENTITY(1,1) PRIMARY KEY,
    SirketID INT NOT NULL,
    IslemTarihi DATETIME NOT NULL DEFAULT GETDATE(),
    HareketTipi NVARCHAR(20) NOT NULL, -- 'GIRIS', 'CIKIS', 'TRANSFER' vb.
    DepoID INT NOT NULL,
    UrunID INT NOT NULL,
    Miktar DECIMAL(10,2) NOT NULL,
    ReferansNo NVARCHAR(50) NULL,
    ReferansID INT NULL,
    ReferansTipi NVARCHAR(20) NULL, -- 'SATIS', 'ALIS', 'URETIM' vb.
    Aciklama NVARCHAR(500) NULL,
    KullaniciID INT NULL,
    CONSTRAINT FK_StokHareketleri_Sirketler FOREIGN KEY (SirketID) REFERENCES dbo.Sirketler(SirketID),
    CONSTRAINT FK_StokHareketleri_Depolar FOREIGN KEY (DepoID) REFERENCES dbo.Depolar(DepoID),
    CONSTRAINT FK_StokHareketleri_Urunler FOREIGN KEY (UrunID) REFERENCES dbo.Urunler(UrunID),
    CONSTRAINT FK_StokHareketleri_Kullanicilar FOREIGN KEY (KullaniciID) REFERENCES dbo.Kullanicilar(KullaniciID)
);

-- Create an index for faster queries on ReferansID and ReferansTipi
CREATE INDEX IX_StokHareketleri_Referans ON dbo.StokHareketleri(ReferansID, ReferansTipi); 