using BicycleStore.DbContext;
using BicycleStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using BicycleStore.Services;

namespace BicycleStore.Services
{
    public class MemorySupplierService : ISupplierService
    {
        private readonly AppDbContext _context;

        public MemorySupplierService(AppDbContext context)
        {
            _context = context;
        }

        public int Add(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
            return supplier.SupplierId;
        }

        public void DeleteById(Supplier supplier)
        {
            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();
        }

        public List<Supplier> FindAll()
        {
            return _context.Suppliers.ToList();
        }

        public Supplier? FindById(int id)
        {
            return _context.Suppliers.FirstOrDefault(s => s.SupplierId == id);
        }

        public void Update(Supplier supplier)
        {
            var actualSupplier = _context.Suppliers.Find(supplier.SupplierId);
            if (actualSupplier != null)
            {
                _context.Entry(actualSupplier).CurrentValues.SetValues(supplier);
                _context.SaveChanges();
            }
        }
    }
}
