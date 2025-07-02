namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ZeytinyagiUretim_islemleri
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ZeytinyagiUretim_islemleri()
        {
            ZeytinyagiUretimleris = new HashSet<ZeytinyagiUretimleri>();
        }

        [Key]
        public int ZeytinyagiUretim_islemID { get; set; }

        [StringLength(50)]
        public string islem_Ad { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ZeytinyagiUretimleri> ZeytinyagiUretimleris { get; set; }
    }
}
