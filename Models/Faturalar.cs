namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Faturalar")]
    public partial class Faturalar
    {
        [Key]
        public int FaturaID { get; set; }

        public int SirketID { get; set; }

        [Required]
        [StringLength(50)]
        public string FaturaNo { get; set; }

        public DateTime FaturaTarihi { get; set; }

        public int MusteriID { get; set; }

        public int? SatisID { get; set; }

        public decimal ToplamTutar { get; set; }

        public decimal KDVToplam { get; set; }

        public decimal GenelToplam { get; set; }

        [StringLength(20)]
        public string OdemeDurumu { get; set; }

        public string Notlar { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        public virtual Musteriler Musteriler { get; set; }

        public virtual Satislar Satislar { get; set; }

        public virtual Sirketler Sirketler { get; set; }
    }
}
