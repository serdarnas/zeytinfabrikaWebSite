namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SirketAyarlari")]
    public partial class SirketAyarlari
    {
        [Key]
        public int AyarID { get; set; }

        public int SirketID { get; set; }

        [Required]
        [StringLength(50)]
        public string AyarAdi { get; set; }

        public string AyarDegeri { get; set; }

        [StringLength(200)]
        public string Aciklama { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        public DateTime? GuncellemeTarihi { get; set; }

        public virtual Sirketler Sirketler { get; set; }
    }
}
