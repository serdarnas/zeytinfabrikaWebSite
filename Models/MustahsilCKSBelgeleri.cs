namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MustahsilCKSBelgeleri")]
    public partial class MustahsilCKSBelgeleri
    {
        [Key]
        public int BelgeID { get; set; }

        public int? MustahsilID { get; set; }

        [StringLength(500)]
        public string DosyaYolu { get; set; }

        public int? UretimYili { get; set; }

        [Column(TypeName = "date")]
        public DateTime? GecerlilikBitis { get; set; }

        public DateTime? EklenmeTarihi { get; set; }

        public virtual Mustahsiller Mustahsiller { get; set; }
    }
}
