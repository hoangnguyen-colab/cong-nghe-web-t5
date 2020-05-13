namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ORDERSTATUS")]
    public partial class ORDERSTATU
    {
        public int ID { get; set; }

        public int StatusID { get; set; }

        [StringLength(255)]
        public string StatusName { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
