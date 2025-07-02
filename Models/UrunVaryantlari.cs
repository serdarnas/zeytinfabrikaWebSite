namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UrunVaryantlari")]
    public partial class UrunVaryantlari
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UrunVaryantlari()
        {
            UrunVaryantDetays = new HashSet<UrunVaryantDetay>();
        }

        [Key]
        public int UrunVaryantID { get; set; }

        public int? SirketID { get; set; }

        public int? UrunID { get; set; }

        [StringLength(20)]
        public string Barkod { get; set; }

        public decimal? StokMiktari { get; set; }

        public decimal? AlisFiyati { get; set; }

        public decimal? SatisFiyati { get; set; }

        public decimal? PerakendeSatisFiyati { get; set; }

        public bool? Durum { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        public virtual Urunler Urunler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UrunVaryantDetay> UrunVaryantDetays { get; set; }
    }
}
