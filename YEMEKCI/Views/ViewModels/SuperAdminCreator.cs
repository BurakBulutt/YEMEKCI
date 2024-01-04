using System;
using System.Linq;
using System.Data.Entity;
using YEMEKCI.Models;
using System.Collections.Generic; // Entity Framework kullanıyorsanız, gerekebilir

public class SuperAdminCreator
{
    private static YemekciEntities model = new YemekciEntities();
    public static void OnApplicationStart()
    {
        CheckAndInsertRecord();
    }

    private static void CheckAndInsertRecord()
    {
        var user = model.Customer.FirstOrDefault(x => x.rolName == "ACR");
        if (user == null)
        {
            Customer customer = new Customer();
            customer.name = "SUPER";
            customer.surname = "ADMIN";
            customer.email = "super_admin";
            customer.password = "12345";
            customer.rolName = "ACR";
            model.Customer.Add(customer);
            model.SaveChanges();
            Cart cart = new Cart();
            cart.Customer = customer;
            model.Cart.Add(cart);
        }

        var defaultDiscount = model.Discount.FirstOrDefault(x => x.discount_rate == 0);
        if (defaultDiscount == null)
        {
            Discount discount = new Discount();
            discount.name = "KUPONSUZ DEVAM ET";
            discount.discount_rate = 0;
            model.Discount.Add(discount);
            model.SaveChanges();

        }
        model.SaveChanges();
    }
}

