using System;
using System.Collections.Generic;

namespace StoreFront.Data.EF.Models
{
    public partial class SeasonalAvailability
    {
        public SeasonalAvailability()
        {
            Products = new HashSet<Product>();
        }

        public int SeasonId { get; set; }
        public string SeasonCategory { get; set; } = null!;
        public string SeasonDescription { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
