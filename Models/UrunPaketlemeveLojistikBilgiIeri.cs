namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UrunPaketlemeveLojistikBilgiIeri")]
    public partial class UrunPaketlemeveLojistikBilgiIeri
    {
        [Key]
        public int UrunPaketlemeveLojistikBilgiID { get; set; }

        public int? UrunID { get; set; }

        public int? SirketID { get; set; }

        public byte? UrunNetAgirlik_gr { get; set; }

        public byte? UrunBurutAgirlik_gr { get; set; }

        public byte? Koli_İci_Urun_Adedi { get; set; }

        public byte? KoliBoyutlariEn_cm { get; set; }

        public byte? KoliBoyutlariBoy_cm { get; set; }

        public byte? KoliBoyutlariYukseklik_cm { get; set; }

        public decimal? KoliBrutAgirligi_kg { get; set; }

        public decimal? KoliNetAgirligi_kg { get; set; }

        [StringLength(10)]
        public string KoliBarkodu { get; set; }

        public int? PaletTipID { get; set; }

        public byte? PalettekiKoliAdet { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        public virtual Urunler Urunler { get; set; }
    }
}
