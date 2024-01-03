using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YEMEKCI.Models;
using YEMEKCI.Security;
using YEMEKCI.Views.ViewModels;

namespace YEMEKCI.Controllers
{
    public class SepetController : Controller
    {
        YemekciEntities model = new YemekciEntities();

        // GET: Sepet
        [MyAuthorization(Roles = "A")]
        public ActionResult Index()
        {
            List<Cart> carts =model.Cart.ToList();

            return View(carts);
        }

        [HttpGet]
        [MyAuthorization(Roles = "A")]
        public ActionResult SepetUrunleri(int id)
        {
            List<CartItem> sepetUrunleri = model.CartItem.Where(x => x.cartID == id).ToList();
            List<SepetItemViewModel> data = sepetUrunleri.Select(x => new SepetItemViewModel
            {
                ID = x.ID,
                cartID =x.cartID,
                cartCustomer = x.Cart.Customer.name,
                dishName = x.Dish.name,
                quantity = x.quantity,

            }).ToList();

            return Json(data,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult GetSepetUrunleri(int id)
        {
            Customer c = model.Customer.FirstOrDefault(x => x.ID == id);
            Cart cart = model.Cart.FirstOrDefault(x=>x.customerID == c.ID);
            List<CartItem> sepetUrunleri = model.CartItem.Where(x => x.cartID == cart.ID).ToList();

            ViewBag.idcan = cart.ID;
            ViewBag.cusID = id;

            return View(sepetUrunleri);

        }

        [HttpPost]
        [MyAuthorization(Roles = "A,C,R")]
        public int SepetiBosalt(int id)
        {
            try
            {
                Cart cart = model.Cart.FirstOrDefault(x => x.ID == id);
                List<CartItem> sepetUrunleri = model.CartItem.Where(x => x.cartID == cart.ID).ToList();

                foreach (CartItem item in sepetUrunleri)
                {
                    model.CartItem.Remove(item);
                }
                model.SaveChanges();
                return 1;
            }
            catch(Exception e)
            {
                return 0;
            }
        }


        [HttpPost]
        [MyAuthorization(Roles = "A,C,R")]
        public int SepeteUrunEkleMusteri(int id,int dishID)
        {
            Customer customer = model.Customer.Find(id);
            Cart sepet = model.Cart.FirstOrDefault(x=>x.customerID == customer.ID); // Kullanıcının sepetini buldum
            CartItem item = new CartItem();
            try
            {
                List<CartItem> items = sepet.CartItem.ToList();
                bool isFound = false;
                int cartItemId = 0;

                foreach (CartItem itemd in items)
                {
                    if (itemd.dishID == dishID)
                    {
                        cartItemId = itemd.ID;
                        isFound = true;
                    }
                }

                if (isFound == false)
                {
                    item.quantity = 1;
                    item.cartID = sepet.ID;
                    item.dishID = dishID;
                    model.CartItem.Add(item);
                    model.SaveChanges();
                    return 1;
                }
                else
                {
                    CartItem sepetItem = model.CartItem.FirstOrDefault(x => x.ID == cartItemId);
                    sepetItem.quantity = sepetItem.quantity+1;
                    model.SaveChanges();
                    return 1;
                }
            }catch (Exception ex)
            {
                return 0;
            }
        }


        [HttpGet]
        [MyAuthorization(Roles = "A")]
        public ActionResult SepeteUrunEkle(int id)
        {
            CartItem item = new CartItem();
            List<Dish> dishes = model.Dish.ToList();
            ViewBag.dishes = dishes;
            item.cartID = id;
            return View(item);
        }

        [HttpPost]
        [MyAuthorization(Roles = "A")]
        public ActionResult SepeteUrunEkle(CartItem item)
        {
            Cart cart = model.Cart.FirstOrDefault(x=>x.ID == item.cartID);
            List<CartItem> ctsitems = model.CartItem.Where(x=>x.cartID == cart.ID).ToList();
            bool isAvaible = false;
            CartItem founded = null;

            foreach(CartItem itemss in ctsitems){

                if(itemss.dishID == item.dishID)
                {
                    isAvaible=true;
                    founded = itemss;
                    break;
                }
            }

            Customer customer = model.Customer.FirstOrDefault(x=>x.ID==cart.customerID);

            if(isAvaible)
            {
                founded.quantity++;
                model.SaveChanges();
            }
            else
            {
                model.CartItem.Add(item);
                model.SaveChanges();
            }
            return RedirectToAction("GetSepetUrunleri/" + customer.ID);
        }

        [HttpPost]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult SepetiGuncelle()
        {
            int cartItemId = int.Parse(Request.Form["cartItemId"]);
            int newQuantity = int.Parse(Request.Form["newQuantity"]);

            CartItem item = model.CartItem.FirstOrDefault(x =>x.ID == cartItemId);
            Cart cart = model.Cart.FirstOrDefault(x=>x.ID == item.cartID);
            Customer cus = model.Customer.FirstOrDefault(x=>x.ID == cart.customerID);
            if (newQuantity == 0)
            {
                model.CartItem.Remove(item);
            }
            else
            {
                item.quantity = newQuantity;
            }            
            model.SaveChanges();
            return RedirectToAction("GetSepetUrunleri/" + cus.ID);
        }

    }
}