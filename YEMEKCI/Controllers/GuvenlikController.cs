using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YEMEKCI.Models;
using YEMEKCI.Security;

namespace YEMEKCI.Controllers
{
    [Authorize]
    public class GuvenlikController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(Customer c)
        {
            YemekciEntities model = new YemekciEntities();

            Customer cus = model.Customer.FirstOrDefault(x => x.email == c.email && x.password == c.password);

            if (cus == null)
            {
                ViewBag.hata = "Kullanıcı Adı veya Kullanıcı Şifre Hatalı!";
                return View();
            }
            else
            {
                // Diğer bilgileri de ekleyebilirsiniz
                var userData = $"{cus.name}|{cus.surname}|{cus.ID}|{cus.email}";
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1,
                    cus.email, // Kullanıcı adı
                    DateTime.Now,
                    DateTime.Now.AddMinutes(60), // Süre sonu
                    false, // Hatırlatma
                    userData); // Kullanıcı bilgileri

                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(authCookie);

                return RedirectToAction("Index", "AnaSayfa");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            Customer customer = new Customer();
            customer.rolName = "C";
            return View(customer);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(Customer customer)
        {
            YemekciEntities model = new YemekciEntities();

            try
            {
                model.Customer.Add(customer);

                //Sisteme müşteri eklendiğinde bu müşteriye ait bir sepet te oluşmalıdır.
                Cart cart = new Cart();
                cart.customerID = customer.ID;
                model.Cart.Add(cart);

                model.SaveChanges();
                return RedirectToAction("Login");
            }
            catch(DbEntityValidationException ex)
            {
                return View("KayitBasarisiz");
            }

        }
        public ActionResult KayitBasarisiz()
        {
            return View();
        }

        [Authorize]
        public ActionResult LogOut()
        {

            FormsAuthentication.SignOut();

            return RedirectToAction("Index","AnaSayfa");
        }

        public ActionResult Hata()
        {
            return View();
        }

    }
}