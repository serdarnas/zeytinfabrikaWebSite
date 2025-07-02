namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Alislar")]
    public partial class Alislar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Alislar()
        {
            AlisDetaylaris = new HashSet<AlisDetaylari>();
        }

        [Key]
        public int AlisID { get; set; }

        public int SirketID { get; set; }

        [Required]
        [StringLength(50)]
        public string AlisNo { get; set; }

        public DateTime AlisTarihi { get; set; }

        public int? TedarikciID { get; set; }

        public int? MustahsilID { get; set; }

        public decimal? ToplamTutar { get; set; }

        public decimal? KDVToplam { get; set; }

        public decimal? GenelToplam { get; set; }

        [StringLength(20)]
        public string OdemeDurumu { get; set; }

        public string Notlar { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlisDetaylari> AlisDetaylaris { get; set; }

        public virtual Mustahsiller Mustahsiller { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        public virtual Tedarikciler Tedarikciler { get; set; }
    }
}
