using BicycleStore.Models;
using BicycleStore.Services;
namespace BicycleStore.Services
{
    public interface IBikeService
    {
        int Add(Bike bike);
        void Update(Bike bike);
        void DeleteById(Bike bike);
        Bike? FindById(int id);
        List<Bike> FindAll();



    }

}
