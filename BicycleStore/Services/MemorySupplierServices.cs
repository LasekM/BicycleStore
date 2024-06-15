using BicycleStore.DbContext;
using BicycleStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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
            return supplier.Id;
        }

        public void Update(Supplier supplier)
        {
            var actualSupplier = _context.Suppliers.Find(supplier.Id);
            if (actualSupplier != null)
            {
                _context.Entry(actualSupplier).CurrentValues.SetValues(supplier);
                _context.SaveChanges();
            }
        }

        public void DeleteById(Supplier supplier)
        {
            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();
        }

        public Supplier? FindById(int id)
        {
            return _context.Suppliers.Include(a => a.Bikes).FirstOrDefault(a => a.Id == id);
        }

        public List<Supplier> FindAll()
        {
            return _context.Suppliers.Include(a => a.Bikes).ToList();
        }
    }
}
