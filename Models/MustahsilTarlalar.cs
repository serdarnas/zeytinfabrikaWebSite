namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MustahsilTarlalar")]
    public partial class MustahsilTarlalar
    {
        [Key]
        public int TarlaID { get; set; }

        public int? MustahsilID { get; set; }

        [StringLength(50)]
        public string AdaParsel { get; set; }

        public double? TapuAlan { get; set; }

        public double? KullanilanAlan { get; set; }

        [StringLength(100)]
        public string Urun { get; set; }

        [StringLength(20)]
        public string Sulama { get; set; }

        [StringLength(10)]
        public string MulkTipi { get; set; }

        [StringLength(50)]
        public string Il { get; set; }

        [StringLength(50)]
        public string Ilce { get; set; }

        [StringLength(50)]
        public string Mahalle { get; set; }

        public bool? Durumu { get; set; }

        public virtual Mustahsiller Mustahsiller { get; set; }
    }
}
