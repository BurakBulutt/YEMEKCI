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
    public class AddressController : Controller
    {
        YemekciEntities model = new YemekciEntities();

        // GET: Address
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult Index()
        {
            List<Address> addresses = model.Address.ToList();
            return View(addresses);
        }

        [HttpGet]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult AddressEkle()
        {
            Address address = new Address();
            List<Customer> customers = model.Customer.ToList();
            ViewBag.customersForAddress = customers;
            return View(address);
        
        }

        [HttpPost]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult AddressEkle(Address a)
        {
            model.Address.Add(a);
            model.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult AddressEkleCustomer(int id)
        {
            Address address = new Address();
            address.customerID = id;
            return View(address);

        }

        [HttpPost]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult AddressEkleCustomer(Address a)
        {
            model.Address.Add(a);
            model.SaveChanges();
            return RedirectToAction("AddressGetir/" + a.customerID);
        }




        [HttpGet]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult AddressGuncelle(int id)
        {
            Address a = model.Address.FirstOrDefault(x =>x.ID == id);
            return View(a);
        }

        [HttpPost]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult AddressGuncelle(Address a)
        {
            Address address = model.Address.Find(a.ID);

            address.city = a.city;
            address.district = a.district;
            address.postalCode = a.postalCode;
            address.address1 = a.address1;

            model.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [MyAuthorization(Roles = "A,C,R")]
        public int AddressSil(int id)
        {
            Address a = model.Address.FirstOrDefault(y =>y.ID == id);

            try
            {
                model.Address.Remove(a);
                model.SaveChanges();
                return 1;

            }catch (Exception ex)
            {
                return 0;
            }

        }

        [HttpGet]
        [MyAuthorization(Roles = "A,C,R")]
        public ActionResult AddressGetir(int id)
        {
            List<Address> aList = model.Address.Where(x =>x.customerID == id).ToList();
            ViewBag.customerId = id;
            return View(aList);
        }
            

    }
}