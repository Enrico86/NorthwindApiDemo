﻿using Microsoft.EntityFrameworkCore;
using NorthwindApiDemo.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace NorthwindApiDemo.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private NorthwindContext _context;
        public CustomerRepository(NorthwindContext context)
        {
            _context = context;
        }

        public bool CustomerExists(string customerId)
        {
            return _context.Customers.Any(c => c.CustomerId == customerId);
        }

        public Customers GetCustomer(string customerId, bool includeOrders)
        {
            if (includeOrders)
            {
                return _context.Customers.Include(c => c.Orders).Where(c => c.CustomerId == customerId).FirstOrDefault();
            }
            return _context.Customers.Where(c => c.CustomerId == customerId).FirstOrDefault();
        }

        public IEnumerable<Customers> GetCustomers()
        {
            return _context.Customers.OrderBy(c=>c.CompanyName).ToList();
        }

        public Orders GetOrder(string customerId, int orderId)
        {
            return _context.Orders.Where(o => o.CustomerId == customerId && o.OrderId == orderId).FirstOrDefault();
        }

        public IEnumerable<Orders> GetOrders(string customerId)
        {
            return _context.Orders.Where(o => o.CustomerId == customerId).ToList();
        }

        public void AddOrder(string customerId, Orders order)
        {
            var customer = GetCustomer(customerId, false);
            customer.Orders.Add(order);
        }
        public bool Save()
        {
            return _context.SaveChanges()>=0; 
        }

        public void DeleteOrder(string customerId, int orderId)
        {
            var order = GetOrder(customerId,orderId);
            _context.Remove(order);
        }
    }
}
