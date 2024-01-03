using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YEMEKCI.Models;
using YEMEKCI.Security;

namespace YEMEKCI.Controllers
{
    public class DishController : Controller
    {
        YemekciEntities model = new YemekciEntities();

        // GET: Dish
        [MyAuthorization(Roles = "A")]
        public ActionResult Index()
        {
            List<Dish> dishList = model.Dish.ToList();
            return View(dishList);
        }

        [HttpGet]
        [MyAuthorization(Roles = "A")]
        public ActionResult YemekEkle()
        {
            Dish dish = new Dish();
            List<Category> categories = model.Category.ToList();
            List<Restaurant> restaurants = model.Restaurant.ToList();
            ViewBag.categories = categories;
            ViewBag.restaurants = restaurants;

            return View(dish);
        }

        [HttpPost]
        [MyAuthorization(Roles = "A")]
        public ActionResult YemekEkle(Dish dish)
        {
            model.Dish.Add(dish);
            model.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        [MyAuthorization(Roles = "A")]
        public ActionResult YemekGuncelle(int id)
        {
            Dish dish = model.Dish.FirstOrDefault(x =>x.ID == id);
            List<Category> categories = model.Category.ToList();
            List<Restaurant> restaurants = model.Restaurant.ToList();
            ViewBag.categories = categories;
            ViewBag.restaurants = restaurants;
            return View(dish);
        }

        [HttpPost]
        [MyAuthorization(Roles = "A")]
        public ActionResult YemekGuncelle(Dish dish)
        {
            Dish d = model.Dish.Find(dish.ID);

            d.name = dish.name;
            d.restaurantID = dish.restaurantID;
            d.categoryID = dish.categoryID;
            d.unit_price = dish.unit_price;

            model.Dish.AddOrUpdate(d);
            model.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [MyAuthorization(Roles = "A")]
        public int DishSil(int id)
        {
            Dish d = model.Dish.FirstOrDefault(x => x.ID == id);
            List<CartItem> items = model.CartItem.Where(x => x.dishID == d.ID).ToList();
            List<OrderItem> orderritems = model.OrderItem.Where(x => x.dishID == d.ID).ToList();
            try
            {
                foreach (var item in items)
                {
                    model.CartItem.Remove(item);
                }
                foreach (var itemcan in orderritems) 
                {
                    model.OrderItem.Remove(itemcan);
                }   
                model.Dish.Remove(d);
                model.SaveChanges();
                return 1;
            }catch (Exception ex)
            {
                return 0;
            }
        }

    }
}