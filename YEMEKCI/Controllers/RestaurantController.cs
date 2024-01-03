using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YEMEKCI.Models;
using YEMEKCI.Security;
using YEMEKCI.ViewModels;

namespace YEMEKCI.Controllers
{
    public class RestaurantController : Controller
    {
        YemekciEntities model = new YemekciEntities();

        // GET: Restaurant
        [MyAuthorization(Roles = "A")]
        public ActionResult Index()
        {
            List<Restaurant> list = model.Restaurant.ToList();
            List<Restaurant_Address> alist = model.Restaurant_Address.ToList();
            ViewBag.RestoranAdresleri = alist;

            return View(list);
        }

        [HttpGet]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult RestaurantGetir(int adresId, int categoryId)
        {
            Address a = model.Address.Find(adresId);
            string il = a.city;
            string ilce = a.district;

            List<Restaurant> list;
            if (categoryId !=0)
            {
                List<Dish> dishes = model.Dish.Where(x => x.categoryID == categoryId).ToList();
                List<int> dishcategoryId = dishes.Select(x => x.categoryID).ToList();
                list = model.Restaurant.Where(x => x.Restaurant_Address.city == il && x.Restaurant_Address.district == ilce)
                .Where(x => x.Dish.Any(d => dishcategoryId.Contains(d.categoryID))).ToList();
            }
            else
            {
                list = model.Restaurant.Where(x => x.Restaurant_Address.city == il && x.Restaurant_Address.district == ilce).ToList();
            }            
            return View(list);
        }
        [HttpGet]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult RestaurantYemekleri(int id)
        {            
            Restaurant res = model.Restaurant.Find(id);
            List<Dish> dishes = model.Dish.Where(x => x.restaurantID == id).ToList();
            ViewBag.yemeksler = dishes;
            return View(res);                    
        }

        [HttpGet]
        [MyAuthorization(Roles = "A")]
        public ActionResult RestaurantEkle()
        {
            var viewModel = new RestaurantEkleViewModel
            {
                restaurant = new Restaurant(),
                Raddress = new Restaurant_Address()
            };

            return View(viewModel);
        }

        [HttpPost]
        [MyAuthorization(Roles = "A")]
        public ActionResult RestaurantEkle(RestaurantEkleViewModel viewModel)
        {
            Restaurant_Address restaurant_Address = new Restaurant_Address();
            restaurant_Address.city = viewModel.Raddress.city;
            restaurant_Address.district = viewModel.Raddress.district;
            restaurant_Address.postalCode = viewModel.Raddress.postalCode;
            restaurant_Address.address = viewModel.Raddress.address;

            model.Restaurant_Address.Add(restaurant_Address);
            model.SaveChanges();

            int fk = restaurant_Address.ID;

            Restaurant restaurant = new Restaurant();
            restaurant.name = viewModel.restaurant.name;
            restaurant.restaurant_address_id = fk;
    
            model.Restaurant.Add(restaurant);
            model.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        [MyAuthorization(Roles = "A")]
        public ActionResult RestaurantGuncelle(int id)
        {
            Restaurant res = model.Restaurant.FirstOrDefault(x => x.ID == id);
            int fk = res.restaurant_address_id;
            var viewModel = new RestaurantEkleViewModel
            {
                restaurant = model.Restaurant.FirstOrDefault(x => x.ID == id),
                Raddress = model.Restaurant_Address.FirstOrDefault(x => x.ID == fk ),
            };

            return View(viewModel);
        }

        [HttpPost]
        [MyAuthorization(Roles = "A")]
        public ActionResult RestaurantGuncelle(RestaurantEkleViewModel viewModel)
        {
            Restaurant restaurant = model.Restaurant.Find(viewModel.restaurant.ID);
            Restaurant_Address address = model.Restaurant_Address.Find(viewModel.Raddress.ID);

            address.city = viewModel.Raddress.city;
            address.district = viewModel.Raddress.district;
            address.postalCode = viewModel.Raddress.postalCode;
            address.address = viewModel.Raddress.address;

            model.SaveChanges();

            restaurant.name = viewModel.restaurant.name;
            model.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [MyAuthorization(Roles = "A")]
        public int RestaurantSil(int id)
        {
            Restaurant r = model.Restaurant.FirstOrDefault(x =>x.ID == id);
            Restaurant_Address restaurant_Address = model.Restaurant_Address.FirstOrDefault(z => z.ID == r.restaurant_address_id);

            try
            {
                model.Restaurant.Remove(r);
                model.Restaurant_Address.Remove(restaurant_Address);
                model.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }



    }
}