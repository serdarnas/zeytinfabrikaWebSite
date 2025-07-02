namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Teklifler")]
    public partial class Teklifler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Teklifler()
        {
            TeklifDetaylaris = new HashSet<TeklifDetaylari>();
        }

        [Key]
        public int TeklifID { get; set; }

        public int SirketID { get; set; }

        [Required]
        [StringLength(50)]
        public string TeklifNo { get; set; }

        public DateTime TeklifTarihi { get; set; }

        public int MusteriID { get; set; }

        public DateTime? GecerlilikTarihi { get; set; }

        public decimal ToplamTutar { get; set; }

        public decimal KDVToplam { get; set; }

        public decimal GenelToplam { get; set; }

        [StringLength(20)]
        public string Durum { get; set; }

        public string Notlar { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        public virtual Musteriler Musteriler { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TeklifDetaylari> TeklifDetaylaris { get; set; }
    }
}
