namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cekler")]
    public partial class Cekler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cekler()
        {
            CekHareketleris = new HashSet<CekHareketleri>();
        }

        [Key]
        public int CekID { get; set; }

        public int SirketID { get; set; }

        public int AlinanMusteriID { get; set; }

        [Column(TypeName = "date")]
        public DateTime AlisTarihi { get; set; }

        [Required]
        [StringLength(100)]
        public string SeriNo { get; set; }

        [Required]
        [StringLength(150)]
        public string BankaAdi { get; set; }

        [Required]
        [StringLength(150)]
        public string SubeAdi { get; set; }

        [Required]
        [StringLength(50)]
        public string HesapNo { get; set; }

        [Required]
        [StringLength(250)]
        public string Kesideci { get; set; }

        public decimal Tutar { get; set; }

        public int ParaBirimiID { get; set; }

        [Column(TypeName = "date")]
        public DateTime VadeTarihi { get; set; }

        [Column(TypeName = "date")]
        public DateTime KesideTarihi { get; set; }

        [StringLength(100)]
        public string OdemeYeri { get; set; }

        public int DurumID { get; set; }

        [StringLength(1000)]
        public string Aciklama { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        public virtual CekDurumlari CekDurumlari { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CekHareketleri> CekHareketleris { get; set; }

        public virtual Musteriler Musteriler { get; set; }

        public virtual Sirketler Sirketler { get; set; }
    }
}
