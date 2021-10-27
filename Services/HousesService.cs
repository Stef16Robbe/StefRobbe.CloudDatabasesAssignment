using System.Collections;
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
        
        public async Task<House> CreateHouse(House house)
        {
            return await _housesRepository.CreateHouse(house);
        }
        
        public async Task<House> GetHousesPaginated(float priceFrom, float priceTo)
        {
            return await _housesRepository.GetHousesPaginated(priceFrom, priceTo);
        }
    }
}