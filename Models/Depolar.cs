namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Depolar")]
    public partial class Depolar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Depolar()
        {
            DepoStoks = new HashSet<DepoStok>();
            StokHareketleris = new HashSet<StokHareketleri>();
            Uretims = new HashSet<Uretim>();
        }

        [Key]
        public int DepoID { get; set; }

        public int SirketID { get; set; }

        [Required]
        [StringLength(50)]
        public string DepoAdi { get; set; }

        [Required]
        [StringLength(20)]
        public string DepoKodu { get; set; }

        public decimal? Kapasite { get; set; }

        public decimal? DoluMiktar { get; set; }

        public bool? Durum { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DepoStok> DepoStoks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StokHareketleri> StokHareketleris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Uretim> Uretims { get; set; }
    }
}
