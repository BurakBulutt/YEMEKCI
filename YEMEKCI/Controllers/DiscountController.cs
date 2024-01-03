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
    [MyAuthorization(Roles = "A")]
    public class DiscountController : Controller
    {
        YemekciEntities model = new YemekciEntities();

        // GET: Discount
        public ActionResult Index()
        {
            List<Discount> discountList = model.Discount.ToList();
            return View(discountList);
        }

        public ActionResult CustomerDiscounts(int id)
        {
            List<Discount> discounts = model.Customer.Where(c =>c.ID ==id).SelectMany(c =>c.Discount).ToList();
            return View(discounts);
        }

        public ActionResult DiscountEkle()
        {
            Discount discount = new Discount();
            return View(discount);
        }

        [HttpPost]
        public ActionResult DiscountEkle(Discount c)
        {
            model.Discount.Add(c);
            model.SaveChanges();    
            return RedirectToAction("Index");   
        }

        public ActionResult DiscountGuncelle(int id)
        {
            Discount discount = model.Discount.FirstOrDefault(x =>x.ID == id);
            return View(discount);
        }

        [HttpPost]
        public ActionResult DiscountGuncelle(Discount c)
        {
            Discount dt = model.Discount.Find(c.ID);
            dt.name = c.name;
            dt.discount_rate = c.discount_rate;
            model.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DiscountTanimlaKullanici()
        {
            List<Customer> customers = model.Customer.ToList();
            List<Discount> discounts = model.Discount.ToList();
            ViewBag.Customer = customers;
            ViewBag.Discount = discounts;
            return View();
        }

        [HttpPost]
        public ActionResult DiscountTanimlaKullanici(List<int> customerIDList,List<int> discountIDList)
        {
            foreach (int id in customerIDList)
            {
                Customer customer = model.Customer.FirstOrDefault(x => x.ID == id);
                foreach (int dId in discountIDList)
                {
                    try
                    {
                        Discount discount = model.Discount.FirstOrDefault(x => x.ID == dId);
                        customer.Discount.Add(discount);
                    }
                    catch (Exception ex)
                    {
                        ex.GetBaseException();
                    }
                }
            }
            model.SaveChanges();
            return Json(new { success = true});
        }

        [HttpPost]
        public int DiscountSil(int id)
        {
            Discount d = model.Discount.FirstOrDefault(x=>x.ID == id);
            List<Customer> customers = d.Customer.ToList();

            try
            {
                foreach (Customer c in customers)
                {
                    c.Discount.Remove(d);
                }
                model.Discount.Remove(d);
                model.SaveChanges();
                return 1;
            }catch (Exception ex)
            {
                return 0;
            }
            
        }


    }
}