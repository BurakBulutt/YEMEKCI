//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace YEMEKCI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Orderr
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Orderr()
        {
            this.OrderItem = new HashSet<OrderItem>();
        }
    
        public int ID { get; set; }
        public int customerID { get; set; }
        public int paymentID { get; set; }
        public System.DateTime order_date { get; set; }
        public decimal total_Amaount { get; set; }
        public string order_status { get; set; }
        public int addressID { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderItem> OrderItem { get; set; }
        public virtual Payment Payment { get; set; }
    }
}