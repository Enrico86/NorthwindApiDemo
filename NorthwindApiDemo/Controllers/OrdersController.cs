using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NorthwindApiDemo.Models;


namespace NorthwindApiDemo.Controllers
{
    [Route("api/customers")]
    public class OrdersController : Controller
    {
        [HttpGet("{customerId}/orders", Name ="GetOrders")]
        public IActionResult GetOrders (int customerId)
        {
            var customer = Repository.Instance.Customers.FirstOrDefault(s => s.ID == customerId);
            if (customer==null)
            {
                return NotFound($"Cliente con id {customerId} no encontrado");
            }
            return Ok(customer.Orders);
        }

        [HttpGet("{customerId}/orders/{orderId}", Name ="GetOrder")]
        //Asigno un nombre a esta ruta, para poderlo utilizar en otros métodos en que lo necesite
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

        [HttpPost("{customerId}/orders")]
        public IActionResult CreateOrder (int customerId, [FromBody]OrdersForCreationDTO order)
            //Hay que especificar que el parametro order será introducio por el usuario en el Body de la página web de 
            //donde se está invocando este método (api/customers/3/orders por ejemplo si se trata del cliente con id 3)
        {
            if (order==null)
            {
                return BadRequest("Petición incorrecta: el pedido está incompleto");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var customer = Repository.Instance.Customers.FirstOrDefault(s => s.ID == customerId);
            if (customer == null)
            {
                return NotFound($"Cliente con id {customerId} no encontrado");
            }
            var maxOrderId = Repository.Instance.Customers.SelectMany(c=>c.Orders).Max(o=>o.OrderId);
            //Capturo en una variable el id max entre todos los pedidos de todos los clientes

            var finalOrder = new OrdersDTO()
            {
                OrderId = maxOrderId++,
                CustomerId = order.CustomerId,
                EmployeeId=order.EmployeeId,
                Freight=order.Freight,
                OrderDate=DateTime.Today,
                RequiredDate=order.RequiredDate,
                ShipAddress=order.ShipAddress,
                ShipCity=order.ShipCity,
                ShipCountry=order.ShipCountry,
                ShipName=order.ShipName,
                ShippedaDate=order.ShippedaDate,
                ShipPostalCode=order.ShipPostalCode,
                ShipRegion=order.ShipRegion,
                ShipVia=order.ShipVia
                //El usuario digamos creará un OrdersForCretionDTO (por el que no tiene que definir el OrderId ni el OrderDate por ejemplo
            };

            customer.Orders.Add(finalOrder);
            return CreatedAtRoute("GetOrder", new { customerId = customerId, orderId = finalOrder.OrderId },finalOrder);
            //Como acción final vamos a crear una nueva ruta para que el navegador nos dirija hacia ella: necesita tres 
            //parametros esta ruta, el primero es el nombre de la ruta (que hemos llemado GetOrder, hubiera sido lo mismo 
            //volver a escribir "{customerId}/orders/{orderId}"), el segundo son los valores de la ruta ya que esta ruta 
            //necesita de dos variables para que podamos navegar hacia ella (el customerId y orderId) por lo que creamos 
            //una nueva instancia de un objeto anonimo con estos dos valores, cogiendolos del pedido que acabamos de crear.
            //Y como tercer parametro le pasamos el objeto del que queremos que coja los datos a mostrar en el navegador.
        }

        [HttpPut("{customerId}/orders/{orderId}")]
        public IActionResult UpdateOrder (int customerId, int orderId, [FromBody] OrdersForCreationDTO orderToUpdate)
        {
            if (orderToUpdate == null)
            {
                return BadRequest("Petición incorrecta: el pedido está incompleto");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

            order.CustomerId = orderToUpdate.CustomerId;
            order.EmployeeId = orderToUpdate.EmployeeId;
            order.Freight = orderToUpdate.Freight;
            order.RequiredDate = orderToUpdate.RequiredDate;
            order.ShipAddress = orderToUpdate.ShipAddress;
            order.ShipCity = orderToUpdate.ShipCity;
            order.ShipCountry = orderToUpdate.ShipCountry;
            order.ShipName = orderToUpdate.ShipName;
            order.ShippedaDate = orderToUpdate.ShippedaDate;
            order.ShipPostalCode = orderToUpdate.ShipPostalCode;
            order.ShipRegion = orderToUpdate.ShipRegion;
            order.ShipVia = orderToUpdate.ShipVia;
            //El usuario digamos creará un OrdersForCretionDTO (por el que no tiene que definir el OrderId ni el OrderDate por ejemplo

            return NoContent();
        }

        [HttpPatch("{customerId}/orders/{orderId}")]
        public IActionResult UpdateOrder (int customerId, int orderId, [FromBody] JsonPatchDocument<OrdersForCreationDTO> patchOrder)
        {
            if (patchOrder == null)
            {
                return BadRequest("Petición incorrecta: el pedido está incompleto");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

            var orderToUpdate = new OrdersForCreationDTO()
            {
                CustomerId = order.CustomerId,
                EmployeeId = order.EmployeeId,
                Freight = order.Freight,
                RequiredDate = order.RequiredDate,
                ShipAddress = order.ShipAddress,
                ShipCity = order.ShipCity,
                ShipCountry = order.ShipCountry,
                ShipName = order.ShipName,
                ShippedaDate = order.ShippedaDate,
                ShipPostalCode = order.ShipPostalCode,
                ShipRegion = order.ShipRegion,
                ShipVia = order.ShipVia,
            };

            patchOrder.ApplyTo(orderToUpdate);


            order.CustomerId = orderToUpdate.CustomerId;
            order.EmployeeId = orderToUpdate.EmployeeId;
            order.Freight = orderToUpdate.Freight;
            order.RequiredDate = orderToUpdate.RequiredDate;
            order.ShipAddress = orderToUpdate.ShipAddress;
            order.ShipCity = orderToUpdate.ShipCity;
            order.ShipCountry = orderToUpdate.ShipCountry;
            order.ShipName = orderToUpdate.ShipName;
            order.ShippedaDate = orderToUpdate.ShippedaDate;
            order.ShipPostalCode = orderToUpdate.ShipPostalCode;
            order.ShipRegion = orderToUpdate.ShipRegion;
            order.ShipVia = orderToUpdate.ShipVia;

            return NoContent();
        }

        [HttpDelete("{customerId}/orders/{orderId}")]
        public IActionResult DeleteOrder (int customerId, int orderId)
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

            customer.Orders.Remove(order);
            return Ok("Registro eliminado correctamente");
        }

    }
}
