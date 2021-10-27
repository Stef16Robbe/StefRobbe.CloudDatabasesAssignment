using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Services
{
    public interface IHousesService
    {
        Task<House> GetHousesPaginated(float priceFrom, float priceTo);
        Task<House> CreateHouse(House house);
    }
}