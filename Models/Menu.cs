namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Menu")]
    public partial class Menu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Menu()
        {
            Menu1 = new HashSet<Menu>();
        }

        public int MenuID { get; set; }

        public int? UstMenuID { get; set; }

        [StringLength(100)]
        public string MenuAdi { get; set; }

        [StringLength(50)]
        public string Ikon { get; set; }

        [StringLength(500)]
        public string SayfaURL { get; set; }

        public int? Sira { get; set; }

        [StringLength(50)]
        public string YetkiKodu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Menu> Menu1 { get; set; }

        public virtual Menu Menu2 { get; set; }
    }
}
