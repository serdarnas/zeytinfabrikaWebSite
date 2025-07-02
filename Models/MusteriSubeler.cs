namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MusteriSubeler")]
    public partial class MusteriSubeler
    {
        [Key]
        public int MusteriSubeID { get; set; }

        public int MusteriID { get; set; }

        public int SirketID { get; set; }

        [StringLength(250)]
        public string SubeAdi { get; set; }

        [StringLength(50)]
        public string MüsteriSubeKodu { get; set; }

        [StringLength(200)]
        public string Adres { get; set; }

        public virtual Musteriler Musteriler { get; set; }

        public virtual Sirketler Sirketler { get; set; }
    }
}
