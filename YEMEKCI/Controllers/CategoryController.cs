using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YEMEKCI.Models;
using YEMEKCI.Security;

namespace YEMEKCI.Controllers
{
    public class CategoryController : Controller
    {
        YemekciEntities model = new YemekciEntities();

        // GET: Category
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult Index()
        {
            List<Category> categories = model.Category.ToList();
            return View(categories);
        }

        [HttpGet]
        [MyAuthorization(Roles = "A")]
        public ActionResult CategoryEkle()
        {
            Category category = new Category();

            return View(category);
        }

        [HttpPost]
        [MyAuthorization(Roles = "A")]
        public ActionResult CategoryEkle(Category c)
        {
            model.Category.Add(c);
            model.SaveChanges();

            return RedirectToAction("Index");

        }

        [HttpPost]
        [MyAuthorization(Roles = "A")]
        public int CategorySil(int id)
        {
            Category c = model.Category.FirstOrDefault(x => x.ID == id);

            try
            {
                model.Category.Remove(c);
                model.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        [HttpGet]
        [MyAuthorization(Roles = "A")]
        public ActionResult CategoryGuncelle(int id)
        {
            Category category = model.Category.FirstOrDefault(x => x.ID == id);

            return View(category);
        }

        [HttpPost]
        [MyAuthorization(Roles = "A")]
        public ActionResult CategoryGuncelle(Category c)
        {
            Category category = model.Category.Find(c.ID);

            category.ID = c.ID;
            category.name = c.name;
            model.SaveChanges();

            return RedirectToAction("Index");
        }



    }
}
