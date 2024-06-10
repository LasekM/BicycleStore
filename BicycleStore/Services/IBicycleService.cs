using BicycleStore.Models;

namespace BicycleStore.Services
{
    public interface IBicycleService
    {
        int Add(Bike bike);
        void Update(Bike bike);
        void DeleteById(Bike bike);
        Bike? FindById(int id);
        List<Bike> FindAll();
    }
}
