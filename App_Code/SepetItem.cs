using System;

/// <summary>
/// Sepet öğelerini temsil eden sınıf
/// </summary>
public class SepetItem
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public int UrunID { get; set; }

    /// <summary>
    /// Ürün adı
    /// </summary>
    public string UrunAdi { get; set; }

    /// <summary>
    /// Ürün miktarı
    /// </summary>
    public decimal Miktar { get; set; }

    /// <summary>
    /// Birim fiyat
    /// </summary>
    public decimal BirimFiyat { get; set; }

    /// <summary>
    /// Toplam fiyat
    /// </summary>
    public decimal ToplamFiyat { get; set; }

    /// <summary>
    /// Depo ID'si
    /// </summary>
    public int DepoID { get; set; }
} 