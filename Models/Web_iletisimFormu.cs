namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Web_iletisimFormu
    {
        [Key]
        public int İletisimID { get; set; }

        [StringLength(150)]
        public string AdSoyad { get; set; }

        [StringLength(150)]
        public string EPostaAdresi { get; set; }

        [StringLength(50)]
        public string Telefon { get; set; }

        [StringLength(250)]
        public string Konu { get; set; }

        public string Mesaj { get; set; }

        public DateTime? GonderimTarihi { get; set; }

        public DateTime? OkunmaTarihi { get; set; }
    }
}
