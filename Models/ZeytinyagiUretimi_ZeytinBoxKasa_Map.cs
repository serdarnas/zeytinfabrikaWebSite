namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ZeytinyagiUretimi_ZeytinBoxKasa_Map
    {
        [Key]
        public int MapID { get; set; }

        public int ZeytinyagiUretimID { get; set; }

        public int ZeytinBoxKasaID { get; set; }

        public DateTime? EklenmeTarihi { get; set; }

        public virtual ZeytinBoxKasalari ZeytinBoxKasalari { get; set; }

        public virtual ZeytinyagiUretimleri ZeytinyagiUretimleri { get; set; }
    }
}
