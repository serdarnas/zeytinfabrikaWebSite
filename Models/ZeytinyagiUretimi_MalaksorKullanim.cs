namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ZeytinyagiUretimi_MalaksorKullanim
    {
        [Key]
        public int MalaksorKullanimID { get; set; }

        public int ZeytinyagiUretimID { get; set; }

        public int SirketZeytinyagiMakinaMalaksorID { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime BaslamaTarihi { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? BitisTarihi { get; set; }

        public decimal? KullanilanKg { get; set; }

        public int? MalaksasyonSuresi_Dakika { get; set; }

        public decimal? SicaklikOrtalama_C { get; set; }

        public int? DevirSayisiOrtalama_RPM { get; set; }

        [StringLength(500)]
        public string Notlar { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? KayitTarihi { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? GuncellemeTarihi { get; set; }

        public virtual SirketZeytinyagiMakinaMalaksorler SirketZeytinyagiMakinaMalaksorler { get; set; }

        public virtual ZeytinyagiUretimleri ZeytinyagiUretimleri { get; set; }
    }
}
