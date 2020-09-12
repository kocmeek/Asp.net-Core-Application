using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class Menu
    {
        [Key]
        public int iCategoriesId { get; set; }

        [Display(Name = "Kategori Adı")]
        [Required(ErrorMessage = "Lütfen bu alanı doldurun!")]
        public string cCategoriesName { get; set; }
        public DateTime dCreateDate { get; set; }
        public DateTime dUpdateDate { get; set; }

        public IList<Products> Products { get; set; }
    }
}
