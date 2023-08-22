using System;
using System.Collections.Generic;

namespace StoreFront.Data.EF.Models
{
    public partial class Merchant
    {
        public Merchant()
        {
            Products = new HashSet<Product>();
        }

        public int MerchantId { get; set; }
        public string MerchantName { get; set; } = null!;
        public string MerchantAddress1 { get; set; } = null!;
        public string? MerchantAddress2 { get; set; }
        public string MerchantCity { get; set; } = null!;
        public string? MerchantState { get; set; }
        public string? MerchantZip { get; set; }
        public string? MerchantPhone { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
