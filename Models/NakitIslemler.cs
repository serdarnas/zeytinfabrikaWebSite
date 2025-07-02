namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NakitIslemler")]
    public partial class NakitIslemler
    {
        [Key]
        public int IslemID { get; set; }

        public int SirketID { get; set; }

        public DateTime IslemTarihi { get; set; }

        [Required]
        [StringLength(20)]
        public string IslemTuru { get; set; }

        public decimal Tutar { get; set; }

        [StringLength(200)]
        public string Aciklama { get; set; }

        public bool? Durum { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        public virtual Sirketler Sirketler { get; set; }
    }
}
