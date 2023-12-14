namespace AutoPartsStore.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Order = new HashSet<Order>();
        }

        public long ID { get; set; }

        [StringLength(2147483647)]
        public string Name { get; set; }

        [StringLength(2147483647)]
        public string LastName { get; set; }

        [StringLength(2147483647)]
        public string Login { get; set; }

        [StringLength(2147483647)]
        public string Password { get; set; }

        public long IDRole { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Order { get; set; }

        public virtual Role Role { get; set; }
    }
}
