using System;
using System.Collections.Generic;

namespace StoreFront.Data.EF.Models
{
    public partial class ProductStatus
    {
        public ProductStatus()
        {
            Products = new HashSet<Product>();
        }

        public int ProductStatusId { get; set; }
        public string StatusName { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
