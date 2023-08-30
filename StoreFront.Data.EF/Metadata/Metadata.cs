using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFront.Data.EF.Models//.Metadata
{
    //internal class Metadata
    //{
    //}

    #region Category Metadata

    public class CategoryMetadata
    {
        //Primary Key
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "* Category Required")]
        [StringLength(50, ErrorMessage = "* Must be 50 characters or less")]
        [Display(Name = "Category")]
        public string CategoryName { get; set; } = null!;
    }

    #endregion

    #region Merchant Metadata

    public class MerchantMetadata
    {
        //Primary Key
        public int MerchantId { get; set; }

        
        [Required(ErrorMessage = "* Merchant Required")]
        [StringLength(100, ErrorMessage = "* Must be 100 characters or less")]
        [Display(Name = "Merchant")]
        public string MerchantName { get; set; } = null!;


        [Required(ErrorMessage = "* Street Address Required")]
        [StringLength(150, ErrorMessage = "* Must be 150 characters or less")]
        [Display(Name = "Street Address 1")]
        public string MerchantAddress1 { get; set; } = null!;


        [StringLength(150, ErrorMessage = "* Must be 150 characters or less")]
        [Display(Name = "Street Address 2")]
        public string? MerchantAddress2 { get; set; }


        [Required(ErrorMessage = "* City Required")]
        [StringLength(50)]
        [Display(Name = "City")]
        public string MerchantCity { get; set; } = null!;


        [StringLength(2)]
        [Display(Name = "State")]
        public string? MerchantState { get; set; }


        [StringLength(5)]
        [Display(Name = "Zip Code")]
        [DataType(DataType.PostalCode)]
        public string? MerchantZip { get; set; }


        [StringLength(24)]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string? MerchantPhone { get; set; }
    }

    #endregion

    #region Order Metadata

    public class OrderMetadata
    {
        //Primary Key
        public int OrderId { get; set; }


        //no data annotation needed
        public string UserId { get; set; } = null!;


        [Required(ErrorMessage = "* Order Date Required")]
        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }


        [Required(ErrorMessage = "* Ship To Required")]
        [StringLength(100, ErrorMessage = "* Must be 100 characters or less")]
        [Display(Name = "Ship To")]
        public string ShipToName { get; set; } = null!;


        [Required(ErrorMessage = "* City Required")]
        [StringLength(50, ErrorMessage = "* Must be 50 characters or less")]
        [Display(Name = "City")]
        public string ShipToCity { get; set; } = null!;


        [Required(ErrorMessage = "* State Required")]
        [StringLength(2, ErrorMessage = "* Must be 2 characters or less")]
        public string ShipToState { get; set; } = null!;


        [Required(ErrorMessage = "* Zip Code Required")]
        [StringLength(5, ErrorMessage = "* Must be 5 characters or less")]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Zip Code")]
        public string ShipToZip { get; set; } = null!;
    }

    #endregion

    #region Product Metadata

    public class ProductMetadata
    {
        //Primary Key
        public int ProductId { get; set; }


        [Required(ErrorMessage = "* Product Name is Required")]
        [StringLength(100, ErrorMessage = "* Must be 100 characters or less")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = null!;


        [Required(ErrorMessage = "* Price is Required")]
        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = false)]
        [Display(Name = "Price")]
        public decimal ProductPrice { get; set; }


        [Required(ErrorMessage = "* Description is Required")]
        [StringLength(500, ErrorMessage = "* Must be 500 characters or less")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string ProductDescription { get; set; } = null!;


        //Foreign Key
        [Display(Name = "Status")]
        public int? ProductStatusId { get; set; }


        //Foreign Key
        [Display(Name = "Seasonal Availability")]
        public int? SeasonId { get; set; }


        //Foreign Key
        [Display(Name = "Merchant Name")]
        public int? MerchantId { get; set; }


        [StringLength(75, ErrorMessage = "* Must be 75 characters or less")]
        [Display(Name = "Image")]
        public string? ProductImage { get; set; }


        //Foreign Key
        [Display(Name = "Category Name")]
        public int? CategoryId { get; set; }
    }

    #endregion

    #region Product Status Metadata

    public class ProductStatusMetadata
    {
        //Primary Key
        public int ProductStatusId { get; set; }


        [Required(ErrorMessage = "* Status Required")]
        [StringLength(15, ErrorMessage = "* Must be 15 characters or less")]
        [Display(Name = "Status")]
        public string StatusName { get; set; } = null!;
    }

    #endregion

    #region Seasonal Availability Metadata

    public class SeasonalAvailabilityMetadata
    {
        //Primary Key
        public int SeasonId { get; set; }


        [Required(ErrorMessage = "* Season")]
        [StringLength(50, ErrorMessage = "* Must be 50 characters or less")]
        [Display(Name = "Season")]
        public string SeasonCategory { get; set; } = null!;


        [Required(ErrorMessage = "* Description Required")]
        [StringLength(500, ErrorMessage = "* Must be 500 characters or less")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string SeasonDescription { get; set; } = null!;
    }

    #endregion

    #region User Detail Metadata

    public class UserDetailMetadata
    {
        //Primary Key
        public string UserId { get; set; } = null!;


        [Required(ErrorMessage = "* First Name is required")]
        [StringLength(50, ErrorMessage = "* Must be 50 characters or less")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;


        [Required(ErrorMessage = "* Last Name is required")]
        [StringLength(50, ErrorMessage = "* Must be 50 characters or less")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;


        [Required(ErrorMessage = "* Street Address Required")]
        [StringLength(150, ErrorMessage = "* Must be 150 characters or less")]
        [Display(Name = "Street Address 1")]
        public string Address1 { get; set; } = null!;


        [StringLength(150, ErrorMessage = "* Must be 150 characters or less")]
        [Display(Name = "Street Address 2")]
        public string? Address2 { get; set; }


        [Required(ErrorMessage = "* City Required")]
        [StringLength(50)]
        [Display(Name = "City")]
        public string City { get; set; } = null!;


        [Required(ErrorMessage = "* State Required")]
        [StringLength(2)]
        [Display(Name = "State")]
        public string State { get; set; } = null!;


        [Required(ErrorMessage = "* Zip Code Required")]
        [StringLength(5)]
        [Display(Name = "Zip Code")]
        [DataType(DataType.PostalCode)]
        public string Zip { get; set; } = null!;


        [StringLength(24)]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;
    }

    #endregion
}
