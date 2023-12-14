namespace AutoPartsStore.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        public long ID { get; set; }

        public long ID_User { get; set; }

        public long ID_AutoPart { get; set; }

        public string Date { get; set; }

        [StringLength(2147483647)]
        public string Address { get; set; }

        public virtual AutoPart AutoPart { get; set; }

        public virtual User User { get; set; }
    }
}
