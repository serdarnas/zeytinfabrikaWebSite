namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PaletTipleri")]
    public partial class PaletTipleri
    {
        [Key]
        public int PaletTipID { get; set; }

        [StringLength(50)]
        public string PaletAd { get; set; }

        public byte? En_cm { get; set; }

        public byte? Boy_cm { get; set; }

        public byte? Yukseklik_cm { get; set; }

        public byte? Agirlik_kg { get; set; }

        [StringLength(250)]
        public string PaletGorsel { get; set; }
    }
}
