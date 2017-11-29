using System;
using test5.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace test5.ViewModels
{
    public class ReportingViewModel
    {
        public List<PurchaseOrder> outstandingPO;
        public List<PurchaseOrder> shippedPO;
        public List<PurchaseOrder> shippingTodayPO;
        public string listAccsessor { get; set; }

    }
}
