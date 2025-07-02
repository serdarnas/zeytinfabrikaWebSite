namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ZeytinBoxKasalari")]
    public partial class ZeytinBoxKasalari
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ZeytinBoxKasalari()
        {
            ZeytinyagiUretimi_ZeytinBoxKasa_Map = new HashSet<ZeytinyagiUretimi_ZeytinBoxKasa_Map>();
        }

        [Key]
        public int ZeytinBoxKasaID { get; set; }

        public int? SirketID { get; set; }

        public int? ZeytinBoxNo { get; set; }

        public bool? Durumu { get; set; }

        public DateTime? AlimTarihi { get; set; }

        public int? KulananMustahsilID { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ZeytinyagiUretimi_ZeytinBoxKasa_Map> ZeytinyagiUretimi_ZeytinBoxKasa_Map { get; set; }
    }
}
