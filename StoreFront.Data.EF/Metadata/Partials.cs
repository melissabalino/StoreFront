using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace StoreFront.Data.EF.Models//.Metadata
{
    //internal class Partials
    //{
    //}
    #region Category Partial

    [ModelMetadataType(typeof(CategoryMetadata))]
    public partial class Category { }

    #endregion

    #region Merchant Partial

    [ModelMetadataType(typeof(MerchantMetadata))]
    public partial class Merchant { }

    #endregion

    #region Order Partial

    [ModelMetadataType(typeof(OrderMetadata))]
    public partial class Order { }

    #endregion

    #region Product Partial

    [ModelMetadataType(typeof(ProductMetadata))]
    public partial class Product 
    {
        [NotMapped]
        public IFormFile? Image { get; set; }
    }

    #endregion

    #region Product Status Partial

    [ModelMetadataType(typeof(ProductStatusMetadata))]
    public partial class ProductStatus { }

    #endregion

    #region Seasonal Availability Partial

    [ModelMetadataType(typeof(SeasonalAvailabilityMetadata))]
    public partial class SeasonalAvailability { }

    #endregion

    #region User Detail Partial

    [ModelMetadataType(typeof(UserDetailMetadata))]
    public partial class UserDetail
    {
        public string FullName { get => FirstName + ' ' + LastName; }
    }

    #endregion
}
