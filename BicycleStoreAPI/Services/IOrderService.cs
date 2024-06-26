﻿using BicycleStoreAPI.Models;
using System.Collections.Generic;

namespace BicycleStoreAPI.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(int id);
        int CreateOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(Order order);
    }
}
