namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SirketZeytinyagiMakinalari")]
    public partial class SirketZeytinyagiMakinalari
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SirketZeytinyagiMakinalari()
        {
            SirketZeytinyagiMakinaBakimDefteris = new HashSet<SirketZeytinyagiMakinaBakimDefteri>();
            SirketZeytinyagiMakinaMalaksorlers = new HashSet<SirketZeytinyagiMakinaMalaksorler>();
        }

        [Key]
        public int SirketZeytinyagiMakinaID { get; set; }

        public int SirketID { get; set; }

        public int ZeytinyagiMakinaModelID { get; set; }

        public DateTime? AlimTarihi { get; set; }

        public byte? YikamaYaprakAyiklama_kg_dk { get; set; }

        public byte? Kirma_kg_dk { get; set; }

        public byte? Malaksasyon_kg_dk { get; set; }

        public byte? Dekantasyon_Santrifuj_kg_dk { get; set; }

        public bool? Durumu { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SirketZeytinyagiMakinaBakimDefteri> SirketZeytinyagiMakinaBakimDefteris { get; set; }

        public virtual ZeytinyagiMakinaModelleri ZeytinyagiMakinaModelleri { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SirketZeytinyagiMakinaMalaksorler> SirketZeytinyagiMakinaMalaksorlers { get; set; }
    }
}
