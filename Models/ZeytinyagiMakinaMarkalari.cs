namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ZeytinyagiMakinaMarkalari")]
    public partial class ZeytinyagiMakinaMarkalari
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ZeytinyagiMakinaMarkalari()
        {
            ZeytinyagiMakinaModelleris = new HashSet<ZeytinyagiMakinaModelleri>();
        }

        [Key]
        public int ZeytinyagiMakinaMarkaID { get; set; }

        [StringLength(50)]
        public string Ad { get; set; }

        [StringLength(500)]
        public string Adres { get; set; }

        [StringLength(250)]
        public string Logo { get; set; }

        [StringLength(50)]
        public string Telefon { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ZeytinyagiMakinaModelleri> ZeytinyagiMakinaModelleris { get; set; }
    }
}
