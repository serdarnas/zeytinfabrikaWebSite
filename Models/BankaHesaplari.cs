namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BankaHesaplari")]
    public partial class BankaHesaplari
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BankaHesaplari()
        {
            Finans = new HashSet<Finan>();
        }

        [Key]
        public int BankaHesapID { get; set; }

        public int SirketID { get; set; }

        [Required]
        [StringLength(50)]
        public string BankaAdi { get; set; }

        [StringLength(50)]
        public string SubeAdi { get; set; }

        [Required]
        [StringLength(50)]
        public string HesapNo { get; set; }

        [StringLength(50)]
        public string IBAN { get; set; }

        public decimal? Bakiye { get; set; }

        public bool? Durum { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Finan> Finans { get; set; }
    }
}
