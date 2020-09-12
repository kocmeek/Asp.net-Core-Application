using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class Products
    {
        [Key]
        public int iProductId { get; set; }
        public Categories Categories { get; set; }

        [Display(Name = "Ürün Adı")]
        [Required(ErrorMessage = "Lütfen bu alanı doldurun!")]
        public string cProductName { get; set; }

        [Display(Name = "Ürün Fiyatı")]
        [Required(ErrorMessage = "Lütfen bu alanı doldurun!")]
        public float fPrice { get; set; }        
        public string cPictures { get; set; }
        public DateTime dCreateDate { get; set; }
        public DateTime dUpdateDate { get; set; }


    }
}
