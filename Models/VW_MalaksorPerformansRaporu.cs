namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_MalaksorPerformansRaporu
    {
        public int? SirketID { get; set; }

        public byte? MalaksorSiraNo { get; set; }

        public int? MalaksorKaparistesi_kg { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ZeytinyagiUretimID { get; set; }

        [StringLength(50)]
        public string PartiNo { get; set; }

        public int? UretimeAlinanKg { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "datetime2")]
        public DateTime BaslamaTarihi { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? BitisTarihi { get; set; }

        public decimal? KullanilanKg { get; set; }

        public int? MalaksasyonSuresi_Dakika { get; set; }

        public decimal? SicaklikOrtalama_C { get; set; }

        public int? DevirSayisiOrtalama_RPM { get; set; }

        public int? ToplamCalismaZamani_Dakika { get; set; }

        public decimal? SaatlikKapasite_kg { get; set; }

        public decimal? KapasiteKullanimi_Yuzde { get; set; }
    }
}
