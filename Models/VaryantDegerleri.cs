namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VaryantDegerleri")]
    public partial class VaryantDegerleri
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VaryantDegerleri()
        {
            UrunVaryantDetays = new HashSet<UrunVaryantDetay>();
        }

        [Key]
        public int VaryantDegerID { get; set; }

        public int? SirketID { get; set; }

        public int? VaryantTurID { get; set; }

        [Required]
        [StringLength(100)]
        public string DegerAdi { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UrunVaryantDetay> UrunVaryantDetays { get; set; }

        public virtual VaryantTurleri VaryantTurleri { get; set; }
    }
}
