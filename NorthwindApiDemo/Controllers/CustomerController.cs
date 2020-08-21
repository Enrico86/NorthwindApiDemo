using Microsoft.AspNetCore.Mvc;
using NorthwindApiDemo.Models;
using NorthwindApiDemo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindApiDemo.Controllers
{
    [Route("api/customers")]
    public class CustomerController : Controller
    {
        private ICustomerRepository _customerRepository;
        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }



        [HttpGet()]
        public IActionResult GetCustomers()
        {

            var customers = _customerRepository.GetCustomers();

            var results = AutoMapper.Mapper.Map<IEnumerable<CustomerWithoutOrders>>(customers);
            //var results = new List<CustomerWithoutOrders>();
            //foreach (var customer in customers)
            //{
            //    results.Add(new CustomerWithoutOrders()
            //    {
            //        CustomerID=customer.CustomerId,
            //        Address =customer.Address,
            //        City=customer.City ,
            //        CompanyName =customer.CompanyName,
            //        ContactName =customer.ContactName,
            //        ContactTitle=customer.ContactTitle,
            //        Country=customer.Country,
            //        Fax=customer.Fax,
            //        Phone=customer.Phone,
            //        PostalCode=customer.PostalCode,
            //        Region=customer.Region
            //    });
            //}
            return new JsonResult(results);

            ////return new JsonResult(Repository.Instance.Customers);

            //return Ok(Repository.Instance.Customers);

            ////return new JsonResult(new List<object>()
            ////{
            ////    new {CustomerID=1, ContactName="Enrico"},
            ////    new {CustomerID=2, ContactName="Anais"},
            ////    new {CustomerID=3, ContactName="Angelo"},
            ////    new {CustomerID=4, ContactName="Lise"},
            ////    new {CustomerID=5, ContactName="Arturo"},
            ////    new {CustomerID=6, ContactName="Lidia"},
            ////}); 
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomer (string id, bool includeOrders=false)
        {
            var customer = _customerRepository.GetCustomer(id, includeOrders);
            if (customer==null)
            {
                return NotFound("Cliente no encontrado en la bbdd");
            }
            if (!includeOrders)
            {
                var result = AutoMapper.Mapper.Map<CustomerWithoutOrders>(customer);
                return Ok(result);
            }
            else
            {
                var result = AutoMapper.Mapper.Map<CustomerDTO>(customer);
                return Ok(result);
            }

            //var customer = Repository.Instance.Customers.Where(s => s.ID == id);
            //if (customer!=null&&customer.GetEnumerator().MoveNext())
            //{
            //    return Ok(customer);
            //}
            //return NotFound();
        }

    }
}
