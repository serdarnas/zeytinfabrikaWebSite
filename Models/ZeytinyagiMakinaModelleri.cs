namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ZeytinyagiMakinaModelleri")]
    public partial class ZeytinyagiMakinaModelleri
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ZeytinyagiMakinaModelleri()
        {
            SirketZeytinyagiMakinalaris = new HashSet<SirketZeytinyagiMakinalari>();
        }

        [Key]
        public int ZeytinyagiMakinaModelID { get; set; }

        public int? ZeytinyagiMakinaMarkaID { get; set; }

        [StringLength(50)]
        public string ModelAd { get; set; }

        [StringLength(50)]
        public string MaxKapasite { get; set; }

        [StringLength(50)]
        public string EnerjiTuketimi { get; set; }

        [StringLength(150)]
        public string Gorsel { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SirketZeytinyagiMakinalari> SirketZeytinyagiMakinalaris { get; set; }

        public virtual ZeytinyagiMakinaMarkalari ZeytinyagiMakinaMarkalari { get; set; }
    }
}
