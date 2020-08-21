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
    }
}