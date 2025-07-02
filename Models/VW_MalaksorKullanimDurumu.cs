namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_MalaksorKullanimDurumu
    {
        public int? SirketID { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SirketZeytinyagiMakinaMalaksorID { get; set; }

        public byte? MalaksorSiraNo { get; set; }

        public int? MalaksorKaparistesi_kg { get; set; }

        public bool? MalaksorDurumu { get; set; }

        public int? Son30GunKullanimSayisi { get; set; }

        public decimal? Son30GunToplamKg { get; set; }

        public int? Son30GunOrtalamaSure { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? SonKullanimTarihi { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(26)]
        public string KullanimDurumu { get; set; }
    }
}
