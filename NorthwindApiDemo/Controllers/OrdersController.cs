using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NorthwindApiDemo.Controllers
{
    [Route("api/customers")]
    public class OrdersController : Controller
    {
        [HttpGet("{customerId}/orders")]
        public IActionResult GetOrders (int customerId)
        {
            var customer = Repository.Instance.Customers.FirstOrDefault(s => s.ID == customerId);
            if (customer==null)
            {
                return NotFound($"Cliente con id {customerId} no encontrado");
            }
            return Ok(customer.Orders);
        }

        [HttpGet("{customerId}/orders/{orderId}")]
        public IActionResult GetOrder (int customerId, int orderId)
        {
            var customer = Repository.Instance.Customers.FirstOrDefault(s => s.ID == customerId);
            if (customer == null)
            {
                return NotFound($"Cliente con id {customerId} no encontrado");
            }
            var order = customer.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return NotFound($"Pedido {orderId} no encontrado para el cliente {customer.ContactName}");
            }
            return Ok(order);
        }
    }
}
