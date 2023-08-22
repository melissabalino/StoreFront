using System;
using System.Collections.Generic;

namespace StoreFront.Data.EF.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string ShipToName { get; set; } = null!;
        public string ShipToCity { get; set; } = null!;
        public string ShipToState { get; set; } = null!;
        public string ShipToZip { get; set; } = null!;

        public virtual UserDetail User { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
