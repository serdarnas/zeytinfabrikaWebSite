namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KullaniciYetki")]
    public partial class KullaniciYetki
    {
        public int ID { get; set; }

        public int? KullaniciID { get; set; }

        [StringLength(50)]
        public string YetkiKodu { get; set; }

        public bool? Aktif { get; set; }
    }
}
