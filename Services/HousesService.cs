using System.Collections.Generic;
using System.Threading.Tasks;
using DAL;
using Domain;

namespace Services
{
    public class HousesService : IHousesService
    {
        private readonly IHousesRepository _housesRepository;

        public HousesService(IHousesRepository housesRepository)
        {
            _housesRepository = housesRepository;
        }

        public IEnumerable<House> GetHousesPaginated(int index, int maxItems, float priceFrom, float priceTo)
        {
            return _housesRepository.GetHousesPaginated(index, maxItems, priceFrom, priceTo);
        }

        public async Task<House> CreateHouse(House house)
        {
            return await _housesRepository.CreateHouse(house);
        }
    }
}