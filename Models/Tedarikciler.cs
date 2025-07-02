namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tedarikciler")]
    public partial class Tedarikciler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tedarikciler()
        {
            Alislars = new HashSet<Alislar>();
            CekHareketleris = new HashSet<CekHareketleri>();
            SenetHareketleris = new HashSet<SenetHareketleri>();
            Senetlers = new HashSet<Senetler>();
        }

        [Key]
        public int TedarikciID { get; set; }

        public int SirketID { get; set; }

        [StringLength(50)]
        public string TedarikciKodu { get; set; }

        [Required]
        [StringLength(100)]
        public string FirmaAdi { get; set; }

        [StringLength(50)]
        public string YetkiliAdi { get; set; }

        [StringLength(20)]
        public string Telefon { get; set; }

        [StringLength(20)]
        public string CepTelefonu { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(200)]
        public string Adres { get; set; }

        [StringLength(50)]
        public string VergiDairesi { get; set; }

        [StringLength(20)]
        public string VergiNo { get; set; }

        public bool? Durum { get; set; }

        public string Notlar { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        public decimal? Bakiyesi { get; set; }

        public int? ParaBirimiID { get; set; }

        [StringLength(500)]
        public string BankaBilgileri { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Alislar> Alislars { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CekHareketleri> CekHareketleris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SenetHareketleri> SenetHareketleris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Senetler> Senetlers { get; set; }

        public virtual Sirketler Sirketler { get; set; }
    }
}
