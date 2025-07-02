namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Uretim")]
    public partial class Uretim
    {
        public int UretimID { get; set; }

        public int SirketID { get; set; }

        [Required]
        [StringLength(50)]
        public string UretimNo { get; set; }

        public DateTime UretimTarihi { get; set; }

        public int UrunID { get; set; }

        public decimal Miktar { get; set; }

        public int BirimID { get; set; }

        public int? DepoID { get; set; }

        public int? TankID { get; set; }

        [StringLength(20)]
        public string Durum { get; set; }

        public string Notlar { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        public virtual Birimler Birimler { get; set; }

        public virtual Depolar Depolar { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        public virtual Tanklar Tanklar { get; set; }

        public virtual Urunler Urunler { get; set; }
    }
}
