using System;
using System.Collections.Generic;

namespace StoreFront.Data.EF.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal ProductPrice { get; set; }
        public string ProductDescription { get; set; } = null!;
        public int? ProductStatusId { get; set; }
        public int? SeasonId { get; set; }
        public int? MerchantId { get; set; }
        public string? ProductImage { get; set; }
        public int CategoryId { get; set; }//removed ? from int?

        public virtual Category? Category { get; set; }
        public virtual Merchant? Merchant { get; set; }
        public virtual ProductStatus? ProductStatus { get; set; }
        public virtual SeasonalAvailability? Season { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
