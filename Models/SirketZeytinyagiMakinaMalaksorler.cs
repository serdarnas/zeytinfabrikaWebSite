namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SirketZeytinyagiMakinaMalaksorler")]
    public partial class SirketZeytinyagiMakinaMalaksorler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SirketZeytinyagiMakinaMalaksorler()
        {
            MalaksorBakimKayitlaris = new HashSet<MalaksorBakimKayitlari>();
            ZeytinyagiUretimi_MalaksorKullanim = new HashSet<ZeytinyagiUretimi_MalaksorKullanim>();
        }

        [Key]
        public int SirketZeytinyagiMakinaMalaksorID { get; set; }

        public int? SirketZeytinyagiMakinaID { get; set; }

        public int? SirketID { get; set; }

        public byte? MalaksorSiraNo { get; set; }

        public int? MalaksorKaparistesi_kg { get; set; }

        public bool? Durum { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MalaksorBakimKayitlari> MalaksorBakimKayitlaris { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        public virtual SirketZeytinyagiMakinalari SirketZeytinyagiMakinalari { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ZeytinyagiUretimi_MalaksorKullanim> ZeytinyagiUretimi_MalaksorKullanim { get; set; }
    }
}
