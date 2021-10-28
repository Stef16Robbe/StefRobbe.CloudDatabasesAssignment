using System.Threading.Tasks;
using Domain;

namespace DAL
{
    public interface IHousesRepository
    {
        Task<House> GetHousesPaginated(float priceFrom, float priceTo);
        Task<House> CreateHouse(House house);
    }
}