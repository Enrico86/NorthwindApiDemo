﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NorthwindApiDemo.Models;
using NorthwindApiDemo.Services;
using AutoMapper;
using NorthwindApiDemo.EFModels;

namespace NorthwindApiDemo.Controllers
{
    [Route("api/customers")]
    public class OrdersController : Controller
    {
        private ICustomerRepository _customerRepository;
        public OrdersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }


        [HttpGet("{customerId}/orders", Name ="GetOrders")]
        public IActionResult GetOrders (string customerId)
        {
            var orders = _customerRepository.GetOrders(customerId);
            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound("Cliente no encontrado");
            }
            if (orders==null)
            {
                return NotFound("No hay pedidos para este cliente");
            }
            var results = Mapper.Map<IEnumerable<OrdersDTO>>(orders);
            return Ok(results);

            //var customer = Repository.Instance.Customers.FirstOrDefault(s => s.ID == customerId);
            //if (customer==null)
            //{
            //    return NotFound($"Cliente con id {customerId} no encontrado");
            //}
            //return Ok(customer.Orders);
        }

        [HttpGet("{customerId}/orders/{orderId}", Name ="GetOrder")]
        //Asigno un nombre a esta ruta, para poderlo utilizar en otros métodos en que lo necesite
        public IActionResult GetOrder (string customerId, int orderId)
        {
            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound("Cliente no encontrado en la bbdd");
            }
            var order = _customerRepository.GetOrder(customerId, orderId);
            if (order==null)
            {
                return NotFound($"No existe el pedido {orderId} del cliente {customerId}");
            }
            var result = Mapper.Map<OrdersDTO>(order);
            return Ok(result);


            //var customer = Repository.Instance.Customers.FirstOrDefault(s => s.ID == customerId);
            //if (customer == null)
            //{
            //    return NotFound($"Cliente con id {customerId} no encontrado");
            //}
            //var order = customer.Orders.FirstOrDefault(o => o.OrderId == orderId);
            //if (order == null)
            //{
            //    return NotFound($"Pedido {orderId} no encontrado para el cliente {customer.ContactName}");
            //}
            //return Ok(order);
        }

        [HttpPost("{customerId}/orders")]
        public IActionResult CreateOrder (string customerId, [FromBody]OrdersForCreationDTO order)
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

            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound($"Cliente con id {customerId} no encontrado en la bbdd");
            }


            //var maxOrderId = Repository.Instance.Customers.SelectMany(c=>c.Orders).Max(o=>o.OrderId);
            ////Capturo en una variable el id max entre todos los pedidos de todos los clientes

            var finalOrder = Mapper.Map<Orders>(order);
            finalOrder.OrderDate = DateTime.Now;
            _customerRepository.AddOrder(customerId, finalOrder);
            if (!_customerRepository.Save())
            {
                return StatusCode(500, "Por favor, revise los datos introducidos");
            }

            var createdOrder = Mapper.Map<OrdersDTO>(finalOrder);
            return CreatedAtRoute("GetOrder", new { customerId = customerId, orderId = createdOrder.OrderId }, createdOrder);


            //    new OrdersDTO()
            //{
            //    OrderId = maxOrderId++,
            //    CustomerId = order.CustomerId,
            //    EmployeeId=order.EmployeeId,
            //    Freight=order.Freight,
            //    OrderDate=DateTime.Today,
            //    RequiredDate=order.RequiredDate,
            //    ShipAddress=order.ShipAddress,
            //    ShipCity=order.ShipCity,
            //    ShipCountry=order.ShipCountry,
            //    ShipName=order.ShipName,
            //    ShippedaDate=order.ShippedaDate,
            //    ShipPostalCode=order.ShipPostalCode,
            //    ShipRegion=order.ShipRegion,
            //    ShipVia=order.ShipVia
            //    //El usuario digamos creará un OrdersForCretionDTO (por el que no tiene que definir el OrderId ni el OrderDate por ejemplo
            //};

            //customer.Orders.Add(finalOrder);
            //return CreatedAtRoute("GetOrder", new { customerId = customerId, orderId = finalOrder.OrderId },finalOrder);
            ////Como acción final vamos a crear una nueva ruta para que el navegador nos dirija hacia ella: necesita tres 
            ////parametros esta ruta, el primero es el nombre de la ruta (que hemos llemado GetOrder, hubiera sido lo mismo 
            ////volver a escribir "{customerId}/orders/{orderId}"), el segundo son los valores de la ruta ya que esta ruta 
            ////necesita de dos variables para que podamos navegar hacia ella (el customerId y orderId) por lo que creamos 
            ////una nueva instancia de un objeto anonimo con estos dos valores, cogiendolos del pedido que acabamos de crear.
            ////Y como tercer parametro le pasamos el objeto del que queremos que coja los datos a mostrar en el navegador.
        }

        [HttpPut("{customerId}/orders/{orderId}")]
        public IActionResult UpdateOrder (string customerId, int orderId, [FromBody] OrdersForCreationDTO customerUpdatedOrder)
        {
            if (customerUpdatedOrder == null)
            {
                return BadRequest("Petición incorrecta: el pedido está incompleto");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound($"Cliente con id {customerId} no encontrado en la bbdd");
            }

            var existingOrder = _customerRepository.GetOrder(customerId, orderId);
            if (existingOrder==null)
            {
                return NotFound($"Pedido con id {orderId} no encontrado en la bbdd");
            }

            Mapper.Map(customerUpdatedOrder, existingOrder);
           
            if (!_customerRepository.Save())
            {
                return StatusCode(500, "Por favor, revise los datos introducidos");
            }

            var updatedOrder = Mapper.Map<OrdersDTO>(existingOrder);
            return CreatedAtRoute("GetOrder", new { customerId = customerId, orderId = updatedOrder.OrderId }, updatedOrder);



            //var customer = Repository.Instance.Customers.FirstOrDefault(s => s.ID == customerId);
            //if (customer == null)
            //{
            //    return NotFound($"Cliente con id {customerId} no encontrado");
            //}

            //var order = customer.Orders.FirstOrDefault(o => o.OrderId == orderId);
            //if (order == null)
            //{
            //    return NotFound($"Pedido {orderId} no encontrado para el cliente {customer.ContactName}");
            //}

            //order.CustomerId = orderToUpdate.CustomerId;
            //order.EmployeeId = orderToUpdate.EmployeeId;
            //order.Freight = orderToUpdate.Freight;
            //order.RequiredDate = orderToUpdate.RequiredDate;
            //order.ShipAddress = orderToUpdate.ShipAddress;
            //order.ShipCity = orderToUpdate.ShipCity;
            //order.ShipCountry = orderToUpdate.ShipCountry;
            //order.ShipName = orderToUpdate.ShipName;
            //order.ShippedaDate = orderToUpdate.ShippedaDate;
            //order.ShipPostalCode = orderToUpdate.ShipPostalCode;
            //order.ShipRegion = orderToUpdate.ShipRegion;
            //order.ShipVia = orderToUpdate.ShipVia;
            ////El usuario digamos creará un OrdersForCretionDTO (por el que no tiene que definir el OrderId ni el OrderDate por ejemplo

            //return NoContent();
        }

        [HttpPatch("{customerId}/orders/{orderId}")]
        public IActionResult UpdateOrder (string customerId, int orderId, [FromBody] JsonPatchDocument<OrdersForCreationDTO> patchOrder)
        {
            if (patchOrder == null)
            {
                return BadRequest("Petición incorrecta: el pedido está incompleto");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound($"Cliente con id {customerId} no encontrado en la bbdd");
            }

            //var customer = Repository.Instance.Customers.FirstOrDefault(s => s.ID == customerId);
            //if (customer == null)
            //{
            //    return NotFound($"Cliente con id {customerId} no encontrado");
            //}

            var existingOrder = _customerRepository.GetOrder(customerId, orderId);
            if (existingOrder == null)
            {
                return NotFound($"Pedido con id {orderId} no encontrado en la bbdd");
            }

            //var order = customer.Orders.FirstOrDefault(o => o.OrderId == orderId);
            //if (order == null)
            //{
            //    return NotFound($"Pedido {orderId} no encontrado para el cliente {customer.ContactName}");
            //}

            var finalOrder = Mapper.Map<OrdersForCreationDTO>(existingOrder);

            //var orderToUpdate = new OrdersForCreationDTO()
            //{
            //    CustomerId = order.CustomerId,
            //    EmployeeId = order.EmployeeId,
            //    Freight = order.Freight,
            //    RequiredDate = order.RequiredDate,
            //    ShipAddress = order.ShipAddress,
            //    ShipCity = order.ShipCity,
            //    ShipCountry = order.ShipCountry,
            //    ShipName = order.ShipName,
            //    ShippedaDate = order.ShippedaDate,
            //    ShipPostalCode = order.ShipPostalCode,
            //    ShipRegion = order.ShipRegion,
            //    ShipVia = order.ShipVia,
            //};

            patchOrder.ApplyTo(finalOrder, ModelState);
            TryValidateModel(finalOrder);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Mapper.Map(finalOrder, existingOrder);

            //order.CustomerId = orderToUpdate.CustomerId;
            //order.EmployeeId = orderToUpdate.EmployeeId;
            //order.Freight = orderToUpdate.Freight;
            //order.RequiredDate = orderToUpdate.RequiredDate;
            //order.ShipAddress = orderToUpdate.ShipAddress;
            //order.ShipCity = orderToUpdate.ShipCity;
            //order.ShipCountry = orderToUpdate.ShipCountry;
            //order.ShipName = orderToUpdate.ShipName;
            //order.ShippedaDate = orderToUpdate.ShippedaDate;
            //order.ShipPostalCode = orderToUpdate.ShipPostalCode;
            //order.ShipRegion = orderToUpdate.ShipRegion;
            //order.ShipVia = orderToUpdate.ShipVia;
            if (!_customerRepository.Save())
            {
                return StatusCode(500, "Por favor, revise los datos introducidos");
            }

            return NoContent();
        }

        [HttpDelete("{customerId}/orders/{orderId}")]
        public IActionResult DeleteOrder (string customerId, int orderId)
        {
            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound($"Cliente con id {customerId} no encontrado en la bbdd");
            }

            //var customer = Repository.Instance.Customers.FirstOrDefault(s => s.ID == customerId);
            //if (customer == null)
            //{
            //    return NotFound($"Cliente con id {customerId} no encontrado");
            //}

            var existingOrder = _customerRepository.GetOrder(customerId, orderId);
            if (existingOrder == null)
            {
                return NotFound($"Pedido con id {orderId} no encontrado en la bbdd");
            }

            //var order = customer.Orders.FirstOrDefault(o => o.OrderId == orderId);
            //if (order == null)
            //{
            //    return NotFound($"Pedido {orderId} no encontrado para el cliente {customer.ContactName}");
            //}

            _customerRepository.DeleteOrder(customerId,orderId);

            if (!_customerRepository.Save())
            {
                return StatusCode(500, "Por favor, revise los datos introducidos");
            }
            return Ok("Registro eliminado correctamente");
        }
    }
}
