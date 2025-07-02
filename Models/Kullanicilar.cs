namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kullanicilar")]
    public partial class Kullanicilar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kullanicilar()
        {
            BlogYazilaris = new HashSet<BlogYazilari>();
            StokHareketleris = new HashSet<StokHareketleri>();
            ZeytinyagiUretimleris = new HashSet<ZeytinyagiUretimleri>();
        }

        [Key]
        public int KullaniciID { get; set; }

        public int SirketID { get; set; }

        [Required]
        [StringLength(50)]
        public string AdSoyad { get; set; }

        [Required]
        [StringLength(100)]
        public string Sifre { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Telefon { get; set; }

        public int YetkiID { get; set; }

        public bool? Aktif { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        public DateTime? SonGirisTarihi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlogYazilari> BlogYazilaris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StokHareketleri> StokHareketleris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ZeytinyagiUretimleri> ZeytinyagiUretimleris { get; set; }
    }
}
