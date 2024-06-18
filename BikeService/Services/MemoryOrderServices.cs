using System.Collections.Generic;
using System.Linq;
using BikeService.Data;
using BikeService.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeService.Services
{
    public class MemoryOrderServices : IOrderService
    {
        private readonly AppDbContext _context;

        public MemoryOrderServices(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders
                .Include(o => o.Bike)
                .Include(o => o.Customer)
                .ToList();
        }

        public Order GetOrderById(int id)
        {
            return _context.Orders
                .Include(o => o.Bike)
                .Include(o => o.Customer)
                .FirstOrDefault(o => o.OrderId == id);
        }

        public int CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order.OrderId;
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public void DeleteOrder(Order order)
        {
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }
    }
}
