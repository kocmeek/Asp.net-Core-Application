using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class CategoriesController : Controller
    {
        Context dc = new Context();

        [HttpGet]
        public IActionResult AddCategories(int id)
        {
            try
            {
                if (id > 0)
                {
                    var result = dc.Categories.Find(id);
                    return View(result);
                }
            }
            catch (Exception Ex)
            {
                return null;
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddCategories(Categories categories)
        {
            try
            {
                if (categories.iCategoriesId > 0)// id 0'dan büyük ise güncelleme demektir
                {
                    if (categories != null)
                    {

                        var update = dc.Categories.Find(categories.iCategoriesId);
                        update.cCategoriesName = categories.cCategoriesName;
                        update.dUpdateDate = DateTime.Now;
                        dc.SaveChanges();
                        return RedirectToAction("List");
                    }
                }
                else
                {
                    if (categories != null)
                    {
                        dc.Categories.Add(categories);
                        dc.SaveChanges();
                        return RedirectToAction("List");
                    }
                }
            }
            catch (Exception Ex)
            {
                return null;
            }

            return View();
        }

        public IActionResult List()
        {
            var list = dc.Categories.ToList();
            return View(list);
        }

        public IActionResult Delete(int id)
        {
            var control = dc.Products.Where(x => x.Categories.iCategoriesId == id).FirstOrDefault();
            if(control == null)
            {

                var del = dc.Categories.Find(id);
                dc.Categories.Remove(del);
                dc.SaveChanges();
            }

            return RedirectToAction("List");
        }
    }
}
