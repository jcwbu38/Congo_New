using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Congo.Models
{
    public class Product
    {
        public string image { set; get; }
        public string description { set; get; }
        public string price { set; get; }
        public string discountPrice { set; get; }
        public DateTime EntryDate;
    }
}