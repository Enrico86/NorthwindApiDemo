using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NorthwindApiDemo.Models
{
    public class OrdersForCreationDTO
    {
        public string CustomerId { get; set; }
        [System.ComponentModel.DataAnnotations.Range(1, 100, ErrorMessage = "EmployeeId tiene que ser entre 1 y 100")]
        public int EmployeeId { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedaDate { get; set; }
        public int ShipVia { get; set; }
        public decimal Freight { get; set; }


        public string ShipName { get; set; }

        [MinLength(10, ErrorMessage = "ShipAddress demasiado corto, mínimo 10 caracteres")]
        public string ShipAddress { get; set; }
        
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
    }
}
