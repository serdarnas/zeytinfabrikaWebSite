namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FinansalKurumlar")]
    public partial class FinansalKurumlar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FinansalKurumlar()
        {
            CekHareketleris = new HashSet<CekHareketleri>();
            SenetHareketleris = new HashSet<SenetHareketleri>();
        }

        [Key]
        public int FinansalKurumID { get; set; }

        public int SirketID { get; set; }

        [Required]
        [StringLength(1)]
        public string KurumTipi { get; set; }

        [Required]
        [StringLength(250)]
        public string KurumAdi { get; set; }

        [StringLength(150)]
        public string SubeAdi { get; set; }

        [StringLength(34)]
        public string Iban { get; set; }

        [StringLength(150)]
        public string YetkiliAdi { get; set; }

        [StringLength(20)]
        public string Telefon { get; set; }

        public bool? AktifMi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CekHareketleri> CekHareketleris { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SenetHareketleri> SenetHareketleris { get; set; }
    }
}
