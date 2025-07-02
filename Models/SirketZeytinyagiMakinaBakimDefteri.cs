namespace 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SirketZeytinyagiMakinaBakimDefteri")]
    public partial class SirketZeytinyagiMakinaBakimDefteri
    {
        [Key]
        public int SirketZeytinyagiMakinaBakimDefterID { get; set; }

        public int SirketID { get; set; }

        public int SirketZeytinyagiMakinaID { get; set; }

        public DateTime? BakimTarihi { get; set; }

        public string BakimNotlari { get; set; }

        public virtual Sirketler Sirketler { get; set; }

        public virtual SirketZeytinyagiMakinalari SirketZeytinyagiMakinalari { get; set; }
    }
}
