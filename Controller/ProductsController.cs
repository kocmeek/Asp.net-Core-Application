using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication4.Models;
using WebApplication4.ViewModels;

namespace WebApplication4.Controllers
{
    public class ProductsController : Controller
    {
        Context dc = new Context();
        private readonly IHostingEnvironment hostingEnvironment;
        public ProductsController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult AddProducts()
        {
            try
            {
                List<SelectListItem> result = (from x in dc.Categories.ToList()
                                               select new SelectListItem
                                               {
                                                   Text = x.cCategoriesName,
                                                   Value = x.iCategoriesId.ToString()
                                               }).ToList();

                ViewBag.CategoriesList = result;
               
            }
            catch (Exception Ex)
            {
                return View();
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddProducts(ProductViewModel model)
        {
            try
            {

                if (model != null)
                {
                    string uniqueFileName = null;
                    if (model.cPictures != null && model.cPictures.Count > 0)
                    {
                        foreach (IFormFile picture in model.cPictures)
                        {
                            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                            uniqueFileName = picture.FileName;
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                            picture.CopyTo(new FileStream(filePath, FileMode.Create));
                        }

                    }
                    var cat = dc.Categories.Where(x => x.iCategoriesId == model.Categories.iCategoriesId).FirstOrDefault();
                    Products newProduct = new Products
                    {
                        cProductName = model.cProductName,
                        fPrice = model.fPrice,
                        Categories = cat,
                        cPictures = uniqueFileName
                    };
                    dc.Products.Add(newProduct);
                    dc.SaveChanges();
                    return RedirectToAction("List");
                }
            }
            catch (Exception Ex)
            {
                return null;
            }
            return View();
        }

        [HttpGet]
        public IActionResult UpdateProducts(int id)
        {
            try
            {
                List<SelectListItem> result = (from x in dc.Categories.ToList()
                                               select new SelectListItem
                                               {
                                                   Text = x.cCategoriesName,
                                                   Value = x.iCategoriesId.ToString()
                                               }).ToList();

                ViewBag.CategoriesList = result;
                Products products = dc.Products.Find(id);
                ProductUpdateViewModal productUpdateViewModal = new ProductUpdateViewModal
                {
                    Id = products.iProductId,
                    cProductName = products.cProductName,
                    fPrice = products.fPrice,
                    Categories = products.Categories,
                    ExistingPicturePath = products.cPictures
                };
                return View(productUpdateViewModal);
                

            }
            catch (Exception Ex)
            {
                return View();
            }

        }

        [HttpPost]
        public IActionResult UpdateProducts(ProductUpdateViewModal model)
        {
            try
            {

                if (model != null)
                {
                    Products products = dc.Products.Find(model.Id);
                    var cat = dc.Categories.Where(x => x.iCategoriesId == model.Categories.iCategoriesId).FirstOrDefault();
                    products.cProductName = model.cProductName;
                    products.fPrice = model.fPrice;
                    products.Categories = cat;
                    if(model.cPictures != null)
                    {
                        if(model.ExistingPicturePath != null)
                        {
                            string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPicturePath);
                            System.IO.File.Delete(filePath);
                        }
                        products.cPictures = ProcessUploadPictures(model);
                    }                   
                  
                    dc.Products.Update(products);
                    dc.SaveChanges();
                    return RedirectToAction("List");
                }
            }
            catch (Exception Ex)
            {
                return null;
            }
            return View();
        }

        private string ProcessUploadPictures(ProductUpdateViewModal model)
        {
            string uniqueFileName = null;
            if (model.cPictures != null && model.cPictures.Count > 0)
            {
                foreach (IFormFile picture in model.cPictures)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = picture.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    picture.CopyTo(new FileStream(filePath, FileMode.Create));
                }

            }

            return uniqueFileName;
        }//fotoğraf update fonksiyonu 

        public IActionResult List()
        {
            var result = dc.Products.Include(x => x.Categories).ToList();
            return View(result);
        }

        public IActionResult ManageList()
        {
            var result = dc.Products.Include(x => x.Categories).ToList();
            return View(result);
        }//Yönetici ürün listesi

        public IActionResult Delete(int id)
        {
            var del = dc.Products.Find(id);
            dc.Products.Remove(del);
            dc.SaveChanges();
            return RedirectToAction("List");

        }

        public IActionResult FilterProduct(int id)
        {
            var result = dc.Products.Where(x => x.Categories.iCategoriesId == id).ToList();
            return View("List", result);
        }//kategori filtreleme

        public IActionResult Detail(int id)
        {
            var result = dc.Products.Find(id);
            return PartialView("Detail", result);
        }
    }
}
