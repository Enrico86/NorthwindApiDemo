using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindApiDemo.Models
{
    public class OrdersForCreationDTO
    {
        public string CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedaDate { get; set; }
        public int ShipVia { get; set; }
        public decimal Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
    }
}
