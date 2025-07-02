namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tanklar")]
    public partial class Tanklar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tanklar()
        {
            Uretims = new HashSet<Uretim>();
            ZeytinyagiUretimleris = new HashSet<ZeytinyagiUretimleri>();
        }

        [Key]
        public int TankID { get; set; }

        public int SirketID { get; set; }

        [Required]
        [StringLength(50)]
        public string TankAdi { get; set; }

        [Required]
        [StringLength(20)]
        public string TankKodu { get; set; }

        public decimal? Kapasite { get; set; }

        public decimal? DoluMiktar { get; set; }

        public bool? Durum { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Uretim> Uretims { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ZeytinyagiUretimleri> ZeytinyagiUretimleris { get; set; }
    }
}
