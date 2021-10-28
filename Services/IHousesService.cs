using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Services
{
    public interface IHousesService
    {
        IEnumerable<House> GetHousesPaginated(int index, int maxItems, float priceFrom, float priceTo);
        Task<House> CreateHouse(House house);
    }
}