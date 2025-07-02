namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Yetkiler")]
    public partial class Yetkiler
    {
        [Key]
        public int YetkiID { get; set; }

        public int SirketID { get; set; }

        [Required]
        [StringLength(50)]
        public string YetkiAdi { get; set; }

        [StringLength(200)]
        public string Aciklama { get; set; }

        public virtual Sirketler Sirketler { get; set; }
    }
}
