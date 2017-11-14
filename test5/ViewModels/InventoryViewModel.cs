using System.Collections.Generic;
using test5.Models;


namespace test5.ViewModels
{
    public class InventoryViewModel
    {
        public IEnumerable<Inventory> Inventory { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
