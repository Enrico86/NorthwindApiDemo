using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindApiDemo.Controllers
{
    [Route("api/customers")]
    public class CustomerController : Controller
    {
        [HttpGet()]
        public IActionResult GetCustomers()
        {
            //return new JsonResult(Repository.Instance.Customers);

            return Ok(Repository.Instance.Customers);

            //return new JsonResult(new List<object>()
            //{
            //    new {CustomerID=1, ContactName="Enrico"},
            //    new {CustomerID=2, ContactName="Anais"},
            //    new {CustomerID=3, ContactName="Angelo"},
            //    new {CustomerID=4, ContactName="Lise"},
            //    new {CustomerID=5, ContactName="Arturo"},
            //    new {CustomerID=6, ContactName="Lidia"},
            //}); 
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomer (int id)
        {
            
            var customer = Repository.Instance.Customers.Where(s => s.ID == id);
            if (customer!=null&&customer.GetEnumerator().MoveNext())
            {
                return Ok(customer);
            }
            return NotFound();
        }

    }
}
