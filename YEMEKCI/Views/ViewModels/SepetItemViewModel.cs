using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YEMEKCI.Views.ViewModels
{
    public class SepetItemViewModel
    {
        public int ID { get; set; }
        public int cartID { get; set; }
        public string cartCustomer { get; set; }
        public string dishName { get; set; }
        public int quantity { get; set; }


    }
}