namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Satislar")]
    public partial class Satislar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Satislar()
        {
            Faturalars = new HashSet<Faturalar>();
            SatisDetaylaris = new HashSet<SatisDetaylari>();
        }

        [Key]
        public int SatisID { get; set; }

        public int SirketID { get; set; }

        public int? ProjeID { get; set; }

        public int MusteriID { get; set; }

        public int? PazarlamaciID { get; set; }

        [StringLength(50)]
        public string SatisTipi { get; set; }

        [Required]
        [StringLength(50)]
        public string SatisBelgeNo { get; set; }

        public DateTime SatisTarihi { get; set; }

        public DateTime? VadeTarihi { get; set; }

        [StringLength(50)]
        public string IrsaliyeNo { get; set; }

        public DateTime? SevkTarihi { get; set; }

        public decimal ToplamTutar { get; set; }

        public decimal? indirimOrani { get; set; }

        public decimal? indirimTutari { get; set; }

        public decimal KDVToplam { get; set; }

        public decimal GenelToplam { get; set; }

        public string Notlar { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Faturalar> Faturalars { get; set; }

        public virtual Musteriler Musteriler { get; set; }

        public virtual Pazarlamacilar Pazarlamacilar { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SatisDetaylari> SatisDetaylaris { get; set; }

        public virtual Sirketler Sirketler { get; set; }
    }
}
