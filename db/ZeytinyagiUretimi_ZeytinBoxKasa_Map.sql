-- Drop table if it exists
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ZeytinyagiUretimi_ZeytinBoxKasa_Map]') AND type in (N'U'))
DROP TABLE [dbo].[ZeytinyagiUretimi_ZeytinBoxKasa_Map]
GO

-- Create mapping table for many-to-many relationship between ZeytinyagiUretimleri and ZeytinBoxKasalari
CREATE TABLE [dbo].[ZeytinyagiUretimi_ZeytinBoxKasa_Map](
    [MapID] [int] IDENTITY(1,1) NOT NULL,
    [ZeytinyagiUretimID] [int] NOT NULL,
    [ZeytinBoxKasaID] [int] NOT NULL,
    [EklenmeTarihi] [datetime] NOT NULL DEFAULT (GETDATE()),
    CONSTRAINT [PK_ZeytinyagiUretimi_ZeytinBoxKasa_Map] PRIMARY KEY CLUSTERED 
    (
        [MapID] ASC
    ),
    CONSTRAINT [FK_ZeytinBoxMap_ZeytinyagiUretim] FOREIGN KEY([ZeytinyagiUretimID])
        REFERENCES [dbo].[ZeytinyagiUretimleri] ([ZeytinyagiUretimID]) ON DELETE CASCADE,
    CONSTRAINT [FK_ZeytinBoxMap_ZeytinBoxKasa] FOREIGN KEY([ZeytinBoxKasaID])
        REFERENCES [dbo].[ZeytinBoxKasalari] ([ZeytinBoxKasaID]),
    CONSTRAINT [UQ_ZeytinyagiUretim_ZeytinBoxKasa] UNIQUE NONCLUSTERED 
    (
        [ZeytinyagiUretimID] ASC,
        [ZeytinBoxKasaID] ASC
    )
)
GO

CREATE NONCLUSTERED INDEX [IX_ZeytinyagiUretim_ZeytinBoxKasa_Map_ZeytinyagiUretimID] ON [dbo].[ZeytinyagiUretimi_ZeytinBoxKasa_Map]
(
    [ZeytinyagiUretimID] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_ZeytinyagiUretim_ZeytinBoxKasa_Map_ZeytinBoxKasaID] ON [dbo].[ZeytinyagiUretimi_ZeytinBoxKasa_Map]
(
    [ZeytinBoxKasaID] ASC
)
GO

-- Add explanation comment
EXEC sp_addextendedproperty @name = N'MS_Description', 
    @value = N'This table establishes a many-to-many relationship between ZeytinyagiUretimleri and ZeytinBoxKasalari tables.', 
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = N'TABLE',  @level1name = N'ZeytinyagiUretimi_ZeytinBoxKasa_Map'
GO 