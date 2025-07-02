namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MalaksorBakimKayitlari")]
    public partial class MalaksorBakimKayitlari
    {
        [Key]
        public int BakimKayitID { get; set; }

        public int SirketZeytinyagiMakinaMalaksorID { get; set; }

        [Column(TypeName = "date")]
        public DateTime BakimTarihi { get; set; }

        [Required]
        [StringLength(100)]
        public string BakimTuru { get; set; }

        [StringLength(1000)]
        public string Aciklama { get; set; }

        public decimal? Maliyet { get; set; }

        [StringLength(200)]
        public string BakimYapanPersonel { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SonrakiBakimTarihi { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? KayitTarihi { get; set; }

        public virtual SirketZeytinyagiMakinaMalaksorler SirketZeytinyagiMakinaMalaksorler { get; set; }
    }
}
