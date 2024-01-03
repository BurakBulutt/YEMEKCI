using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YEMEKCI.Models;

namespace YEMEKCI.ViewModels
{
    public class RestaurantEkleViewModel
    {
        public Restaurant restaurant { get; set; }
        public Restaurant_Address Raddress { get; set; }
    }
}