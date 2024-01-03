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
    [MyAuthorization(Roles = "A")]
    public class PaymentController : Controller
    {
        YemekciEntities model = new YemekciEntities();

        // GET: Payment
        public ActionResult Index()
        {
            List<Payment> payments =model.Payment.ToList();
            return View(payments);
        }

        public ActionResult PaymentEkle()
        {
            Payment payment = new Payment();
            return View(payment);
        }

        [HttpPost]
        public ActionResult PaymentEkle(Payment payment)
        {
            model.Payment.Add(payment);
            model.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public int PaymentSil(int id)
        {
            Payment p = model.Payment.FirstOrDefault(x => x.ID == id);
            try
            {
                model.Payment.Remove(p);
                model.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        [HttpGet]
        public ActionResult PaymentGuncelle(int id)
        {
            Payment p = model.Payment.FirstOrDefault(x => x.ID == id);

            return View(p);
        }

        [HttpPost]
        public ActionResult PaymentGuncelle(Payment p)
        {
            Payment py = model.Payment.Find(p.ID);

            py.name = p.name;

            model.SaveChanges();

            return RedirectToAction("Index");
        }






    }
}