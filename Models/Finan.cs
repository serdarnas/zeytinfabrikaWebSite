namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Finan
    {
        [Key]
        public int FinansID { get; set; }

        public int SirketID { get; set; }

        public DateTime IslemTarihi { get; set; }

        [Required]
        [StringLength(20)]
        public string IslemTuru { get; set; }

        public int? KategoriID { get; set; }

        public decimal Tutar { get; set; }

        [StringLength(200)]
        public string Aciklama { get; set; }

        public int? BankaHesapID { get; set; }

        public bool? Durum { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        public virtual BankaHesaplari BankaHesaplari { get; set; }

        public virtual Sirketler Sirketler { get; set; }
    }
}
