namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Musteriler")]
    public partial class Musteriler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Musteriler()
        {
            CekHareketleris = new HashSet<CekHareketleri>();
            Ceklers = new HashSet<Cekler>();
            Faturalars = new HashSet<Faturalar>();
            SenetHareketleris = new HashSet<SenetHareketleri>();
            Senetlers = new HashSet<Senetler>();
            MusteriSubelers = new HashSet<MusteriSubeler>();
            Satislars = new HashSet<Satislar>();
            Tekliflers = new HashSet<Teklifler>();
        }

        [Key]
        public int MusteriID { get; set; }

        public int SirketID { get; set; }

        [StringLength(50)]
        public string MüsteriKodu { get; set; }

        [StringLength(250)]
        public string MusteriResim { get; set; }

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

        public int? KategoriID { get; set; }

        public bool? Durum { get; set; }

        public string Notlar { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        public decimal? Bakiyesi { get; set; }

        public int? ParaBirimiID { get; set; }

        [StringLength(500)]
        public string BankaBilgileri { get; set; }

        public decimal? Sabitİskonto { get; set; }

        public byte? Vadesi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CekHareketleri> CekHareketleris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cekler> Ceklers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Faturalar> Faturalars { get; set; }

        public virtual MusteriKategorileri MusteriKategorileri { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SenetHareketleri> SenetHareketleris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Senetler> Senetlers { get; set; }

        public virtual ParaBirimileri ParaBirimileri { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MusteriSubeler> MusteriSubelers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Satislar> Satislars { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Teklifler> Tekliflers { get; set; }
    }
}
