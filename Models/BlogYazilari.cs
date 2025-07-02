namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BlogYazilari")]
    public partial class BlogYazilari
    {
        [Key]
        public int YazıID { get; set; }

        public int SirketID { get; set; }

        [Required]
        [StringLength(200)]
        public string Baslik { get; set; }

        [Required]
        public string Icerik { get; set; }

        public int? KategoriID { get; set; }

        public int? YazarID { get; set; }

        public int? Goruntulenme { get; set; }

        public bool? Durum { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        public DateTime? GuncellemeTarihi { get; set; }

        public virtual BlogKategorileri BlogKategorileri { get; set; }

        public virtual Kullanicilar Kullanicilar { get; set; }

        public virtual Sirketler Sirketler { get; set; }
    }
}
