namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MusteriKategorileri")]
    public partial class MusteriKategorileri
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MusteriKategorileri()
        {
            Musterilers = new HashSet<Musteriler>();
        }

        [Key]
        public int KategoriID { get; set; }

        public int SirketID { get; set; }

        [Required]
        [StringLength(50)]
        public string KategoriAdi { get; set; }

        [StringLength(200)]
        public string Aciklama { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Musteriler> Musterilers { get; set; }
    }
}
