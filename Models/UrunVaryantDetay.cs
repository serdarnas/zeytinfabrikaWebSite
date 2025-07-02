namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UrunVaryantDetay")]
    public partial class UrunVaryantDetay
    {
        public int UrunVaryantDetayID { get; set; }

        public int? UrunVaryantID { get; set; }

        public int? VaryantDegerID { get; set; }

        public virtual UrunVaryantlari UrunVaryantlari { get; set; }

        public virtual VaryantDegerleri VaryantDegerleri { get; set; }
    }
}
