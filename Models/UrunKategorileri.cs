namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UrunKategorileri")]
    public partial class UrunKategorileri
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UrunKategorileri()
        {
            Urunlers = new HashSet<Urunler>();
        }

        [Key]
        public int KategoriID { get; set; }

        public int SirketID { get; set; }

        [Required]
        [StringLength(50)]
        public string Ad { get; set; }

        [StringLength(200)]
        public string Aciklama { get; set; }

        public bool? Durumu { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Urunler> Urunlers { get; set; }
    }
}
