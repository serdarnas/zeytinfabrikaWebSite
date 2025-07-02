namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Mustahsiller")]
    public partial class Mustahsiller
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Mustahsiller()
        {
            Alislars = new HashSet<Alislar>();
            MustahsilCKSBelgeleris = new HashSet<MustahsilCKSBelgeleri>();
            MustahsilTarlalars = new HashSet<MustahsilTarlalar>();
            ZeytinyagiUretimleris = new HashSet<ZeytinyagiUretimleri>();
        }

        [Key]
        public int MustahsilID { get; set; }

        public int SirketID { get; set; }

        [Required]
        [StringLength(50)]
        public string Ad { get; set; }

        [Required]
        [StringLength(50)]
        public string Soyad { get; set; }

        [StringLength(20)]
        public string Telefon { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(200)]
        public string Adres { get; set; }

        [StringLength(11)]
        public string TCKimlikNo { get; set; }

        public decimal? Bakiyesi { get; set; }

        public bool? Durum { get; set; }

        public string Notlar { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        [StringLength(500)]
        public string BankaBilgileri { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Alislar> Alislars { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MustahsilCKSBelgeleri> MustahsilCKSBelgeleris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MustahsilTarlalar> MustahsilTarlalars { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ZeytinyagiUretimleri> ZeytinyagiUretimleris { get; set; }
    }
}
