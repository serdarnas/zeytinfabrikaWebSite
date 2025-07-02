namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Senetler")]
    public partial class Senetler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Senetler()
        {
            SenetHareketleris = new HashSet<SenetHareketleri>();
        }

        [Key]
        public int SenetID { get; set; }

        public int SirketID { get; set; }

        [Required]
        [StringLength(1)]
        public string SenetTipi { get; set; }

        [StringLength(100)]
        public string SeriNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime VadeTarihi { get; set; }

        [Column(TypeName = "date")]
        public DateTime DuzenlemeTarihi { get; set; }

        public decimal Tutar { get; set; }

        public int ParaBirimiID { get; set; }

        [Required]
        [StringLength(250)]
        public string Borclu { get; set; }

        [StringLength(100)]
        public string OdemeYeri { get; set; }

        public int? IlgiliMusteriID { get; set; }

        public int? IlgiliTedarikciID { get; set; }

        public int DurumID { get; set; }

        [StringLength(1000)]
        public string Aciklama { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        public virtual Musteriler Musteriler { get; set; }

        public virtual SenetDurumlari SenetDurumlari { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SenetHareketleri> SenetHareketleris { get; set; }

        public virtual Tedarikciler Tedarikciler { get; set; }

        public virtual Sirketler Sirketler { get; set; }
    }
}
