using NorthwindApiDemo.EFModels;
using NorthwindApiDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindApiDemo.Services
{
    public interface ICustomerRepository
        //Esta interfaz nos va a servir para poder inyectarla a través de los diferentes controladores, por lo que vamos a 
        //declarar todos aquellos métodos que utilizaremos en ellos.
    {
        IEnumerable<Customers> GetCustomers();
        Customers GetCustomer(string customerId, bool includeOrders);
        //El motivo de que customerId es un string y no un int es porqué en la base de datos es de este tipo.
        IEnumerable<Orders> GetOrders(string customerId);
        Orders GetOrder(string customerId, int orderId);
        bool CustomerExists(string customerId);

        void AddOrder(string customerId, Orders order);
        bool Save();
    }
}
