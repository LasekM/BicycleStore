﻿using BicycleStoreAPI.Models;

namespace BicycleStoreAPI.Services
{
    public interface ISupplierService
    {
        int Add(Supplier supplier);
        void Update(Supplier supplier);
        void DeleteById(Supplier supplier);
        Supplier? FindById(int id);
        List<Supplier> FindAll();
        
    }
}