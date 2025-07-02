namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VaryantTurleri")]
    public partial class VaryantTurleri
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VaryantTurleri()
        {
            VaryantDegerleris = new HashSet<VaryantDegerleri>();
        }

        [Key]
        public int VaryantTurID { get; set; }

        public int? SirketID { get; set; }

        [Required]
        [StringLength(100)]
        public string TurAdi { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VaryantDegerleri> VaryantDegerleris { get; set; }
    }
}
