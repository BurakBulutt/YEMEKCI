
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
    public class OrderrController : Controller
    {
        YemekciEntities model = new YemekciEntities();

        // GET: Order
        [MyAuthorization(Roles = "A")]
        public ActionResult Index()
        {
            List<Orderr> orderrs = model.Orderr.ToList();
            return View(orderrs);
            
        }

        [HttpGet]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult OrderrItemGetir(int id)
        {
            List<OrderItem> orderItems = model.OrderItem.Where(x =>x.orderID == id).ToList();
            ViewBag.OrderrNO = id;
            return View(orderItems);
        }

        [HttpGet]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult OrderrItemGetirRestorant(int id)
        {
            List<OrderItem> data = model.OrderItem.Where(x => x.orderID == id).ToList();
            ViewBag.OrderrNO = id;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult OrderrItemEkle(int id)
        {
            OrderItem orderItem = new OrderItem();
            orderItem.orderID = id;
            List<Dish> dishes =model.Dish.ToList();
            ViewBag.DishOT = dishes;
            return View(orderItem);
        }

        [HttpPost]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult OrderrItemEkle(OrderItem orderItem)
        {
            List<OrderItem> orderItemss = model.OrderItem.ToList();

            OrderItem item = orderItemss.FirstOrDefault(x =>x.dishID == orderItem.dishID);

            if(item == null)
            {
                model.OrderItem.Add(orderItem);
                model.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                item.quantity = item.quantity + orderItem.quantity;
                model.SaveChanges();
                return RedirectToAction("Index");
            }

        }

        [HttpGet]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult RestoranSiparisleriGiris()
        {
            return View();
        }

        [HttpGet]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult RestoranSiparisleri(int id)
        {
            Restaurant r = model.Restaurant.FirstOrDefault(x =>x.ID == id);

            if(r != null)
            {
                List<Dish> dishes = model.Dish.Where(x => x.restaurantID == id).ToList();
                List<int> dishIds = dishes.Select(d => d.ID).ToList();

                List<OrderItem> orderItems = model.OrderItem.Where(z => dishIds.Contains(z.dishID)).ToList();

                List<int> orderItemsIds = orderItems.Select(z => z.orderID).ToList();

                List<Orderr> orderrs = model.Orderr.Where(z => orderItemsIds.Contains(z.ID)).ToList();

                ViewBag.SiparisItemleri = orderItems;

                return View(orderrs);
            }
            else
            {
                return RedirectToAction("RestoranSiparisleriGiris");
            }


        }

        [HttpGet]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult RestoranSiparisDurumuGuncelle(int id)
        {
            Orderr order = model.Orderr.FirstOrDefault(x =>x.ID == id);
            List<String> siparisDurumlari = new List<String>
            {
                "HAZIRLANIYOR",
                "YOLDA",
                "TESLIM EDILDI"
            };
            ViewBag.SiparisDurumlari = siparisDurumlari;
            return View(order);
        }

        [HttpPost]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult RestoranSiparisDurumuGuncelle(int id, string order_status)
        {
            Orderr orderr = model.Orderr.FirstOrDefault(x => x.ID == id);
            OrderItem item = model.OrderItem.FirstOrDefault(x => x.orderID == orderr.ID);

            int idd = item.Dish.restaurantID;

            if (orderr != null)
            {
                orderr.order_status = order_status;
                model.SaveChanges();
            }
            if (User.IsInRole("A"))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("RestoranSiparisleri/" + idd);
            }
            
        }

        [HttpGet]
        public ActionResult FisOlustur(int id)
        {
            Orderr order = model.Orderr.FirstOrDefault(x => x.ID == id);

            List<OrderItem> orderItems = model.OrderItem.Where(x=> x.orderID == order.ID).ToList();

            ViewBag.CustomerName = order.Customer.name + " " + order.Customer.surname;
            ViewBag.Address = order.Address.address1 + " " + order.Address.city + "/" + order.Address.district;
            ViewBag.TotalAmount = order.total_Amaount;
            ViewBag.OrderItems = orderItems;

            return View();
        }

        [HttpGet]
        public ActionResult SiparisGetirMusteri(int id)
        {
            List<Orderr> orderrs = model.Orderr.Where(x => x.customerID == id).ToList();

            return View(orderrs);       
        }

        [HttpGet]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult OrderrOlustur(decimal totalPrice,int sepetId)
        {
            Orderr order = new Orderr();
            Cart cart = model.Cart.Find(sepetId);
            Customer c = model.Customer.FirstOrDefault(x =>x.ID == cart.customerID);
            order.Customer = c;
            order.total_Amaount = totalPrice;
            
            List<CartItem> cartItems = model.CartItem.Where(x =>x.cartID == sepetId).ToList();
            List<Discount> discounts = c.Discount.ToList();
            List<Payment> payments = model.Payment.ToList();


            CartItem cartItem = model.CartItem.FirstOrDefault(x=>x.cartID == cart.ID);
            Dish dish = model.Dish.FirstOrDefault(x=>x.ID == cartItem.dishID);
            Restaurant res = model.Restaurant.FirstOrDefault(x=>x.ID == dish.restaurantID);
            Restaurant_Address radress = model.Restaurant_Address.FirstOrDefault(x=>x.ID == res.restaurant_address_id);
            string city = radress.city;
            string distrcit = radress.district;

            List<Address> addresses = model.Address.Where(x =>x.customerID == c.ID && x.city == city && x.district == distrcit).ToList();
            ViewBag.cItems = cartItems;
            ViewBag.paymentsO = payments;
            ViewBag.discountO = discounts;
            ViewBag.addressesO = addresses;

            return View(order);
        }

        [HttpPost]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult OrderrOlustur(Orderr orderr, int customerID,int selectedAddressID, int selectedPaymentID, int? selectedDiscountID)
        {
            decimal indirimOrani = 0;
            Customer customer = model.Customer.Find(customerID);
            if (selectedDiscountID != null)
            {
                Discount discount = model.Discount.FirstOrDefault(x => x.ID == selectedDiscountID);
                indirimOrani = discount.discount_rate;
                if (indirimOrani > orderr.total_Amaount)
                {
                    indirimOrani = orderr.total_Amaount;
                }
                customer.Discount.Remove(discount);
            }

            orderr.paymentID = selectedPaymentID;
            orderr.addressID = selectedAddressID;

            orderr.total_Amaount = orderr.total_Amaount - indirimOrani;
            orderr.order_date = DateTime.Now;
            orderr.order_status = "HAZIRLANIYOR";

           
            Cart cart = model.Cart.FirstOrDefault(x =>x.customerID == customerID);
            List<CartItem> cartItems = model.CartItem.Where(x => x.cartID == cart.ID).ToList();
            

            model.Orderr.Add(orderr);

            foreach (CartItem item in cartItems)
            {
                OrderItem orderItem = new OrderItem();
                orderItem.ID = orderr.ID;
                orderItem.dishID = item.dishID;
                orderItem.quantity = item.quantity;
                model.OrderItem.Add(orderItem);
                model.CartItem.Remove(item);
            }
            model.SaveChanges();
            return RedirectToAction("Index","AnaSayfa");
        }

        [HttpPost]
        public int OrderrSil(int id)
        {
            try
            {
                Orderr orderr = model.Orderr.FirstOrDefault(x=>x.ID == id);
                List<OrderItem> orderItems = model.OrderItem.Where(x=>x.orderID == orderr.ID).ToList();

                foreach (OrderItem item in orderItems)
                {
                    model.OrderItem.Remove(item);
                }
                model.Orderr.Remove(orderr);
                model.SaveChanges();   
                return 1;

            }catch (Exception ex)
            {
                return 0;
            }
        }

        [HttpPost]
        public int OrderrItemSil(int id)
        {
            try
            {
                OrderItem item = model.OrderItem.FirstOrDefault(x=>x.ID == id);

                model.OrderItem.Remove(item);
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