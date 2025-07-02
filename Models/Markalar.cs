namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Markalar")]
    public partial class Markalar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Markalar()
        {
            Urunlers = new HashSet<Urunler>();
        }

        [Key]
        public int MarkaID { get; set; }

        public int? SirketID { get; set; }

        [StringLength(50)]
        public string Ad { get; set; }

        [StringLength(250)]
        public string Logo { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Urunler> Urunlers { get; set; }
    }
}
