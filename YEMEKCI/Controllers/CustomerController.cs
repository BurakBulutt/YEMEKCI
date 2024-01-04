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
    public class CustomerController : Controller
    {
        YemekciEntities model = new YemekciEntities();

        // GET: Customer
        [MyAuthorization(Roles = "A")]
        public ActionResult Index()
        {
            List<Customer> list = model.Customer.ToList();

            return View(list);
        }

        [HttpGet]
        [MyAuthorization(Roles = "A")]
        public ActionResult CustomerEkle()
        {
            Customer customer = new Customer();
            return View(customer);
        }

        [HttpPost]
        [MyAuthorization(Roles = "A")]
        public ActionResult CustomerEkle(Customer customer)
        {
            
            model.Customer.Add(customer);

            //Sisteme müşteri eklendiğinde bu müşteriye ait bir sepet te oluşmalıdır.
            Cart cart = new Cart();
            cart.customerID = customer.ID;
            Discount d = model.Discount.FirstOrDefault(x => x.discount_rate == 0);

            model.Cart.Add(cart);
            model.Discount.Add(d);
            model.SaveChanges();
            return RedirectToAction("Index");                 
        }


        [HttpPost]
        [MyAuthorization(Roles = "A")]
        public int CustomerSil(int id)
        {
            Customer c = model.Customer.FirstOrDefault(x => x.ID==id);
            Cart cart = model.Cart.FirstOrDefault(x => x.customerID == c.ID);
            List<Address> addresses = model.Address.Where(x=>x.customerID == c.ID).ToList();
            List<Discount> discounts = model.Discount.ToList();
            try
            {
                foreach (Address address in addresses) 
                {
                    model.Address.Remove(address);
                }
                foreach (Discount d in discounts)
                {
                    c.Discount.Remove(d);
                }
                model.Cart.Remove(cart);    
                model.Customer.Remove(c);
                model.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        [HttpGet]
        [MyAuthorization(Roles = "A,C")]
        public ActionResult CustomerGuncelle(int id)
        {
            Customer customer = model.Customer.FirstOrDefault(x =>x.ID==id);

            return View(customer);
        }

        [HttpPost]
        [MyAuthorization(Roles = "A,C")]
        public ActionResult CustomerGuncelle(Customer c)
        {
            Customer customer = model.Customer.Find(c.ID);

            customer.ID = c.ID;
            customer.name = c.name;
            customer.surname = c.surname;
            customer.birthdate = c.birthdate;
            customer.email = c.email;
            customer.password = c.password;

            model.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CustomerProfile(int id)
        {
            Customer customer = model.Customer.FirstOrDefault(c =>c.ID==id);
            return View(customer);
        }

        [HttpGet]
        public ActionResult CustomerSifreDegistir(int id)
        {
            ViewBag.CustomerID = id;
            return View("CustomerSifreDegistirOnayla");
        }

        [HttpPost]
        public ActionResult CustomerSifreDegistirOnayla(int id, string yeniSifre, string yeniSifreTekrar)
        {
            Customer customer = model.Customer.FirstOrDefault(c => c.ID == id);

            if (yeniSifre == yeniSifreTekrar)
            {
                customer.password = yeniSifre;
                model.SaveChanges();
                return RedirectToAction("CustomerProfile/" + customer.ID);
            }
            else
            {
                ViewBag.HataMesaji = "Şifre Değiştirme İşlemi Başarisiz.";
                ViewBag.CustomerID = id;
                return View("CustomerSifreDegistirOnayla");
            }
        }

    }
}