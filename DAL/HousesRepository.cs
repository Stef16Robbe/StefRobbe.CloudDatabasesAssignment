using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace DAL
{
    public class HousesRepository : IHousesRepository
    {
        private readonly HousesContext _hContext;

        public HousesRepository(HousesContext hContext)
        {
            _hContext = hContext;
        }

        public async Task<House> CreateHouse(House house)
        {
            await _hContext.Database.EnsureDeletedAsync();
            await _hContext.Database.EnsureCreatedAsync();

            _hContext.Add(
                new House
                {
                    id = "fc5253c6-05a7-4d52-9e15-26cc9f64904c",
                    Address = "",
                    Price = 100,
                    Title = "",
                    PictureUrl = "",
                    ZipCode = "1521VJ"
                });

            await _hContext.SaveChangesAsync();
            return house;
        }

        public IEnumerable<House> GetHousesPaginated(int index, int maxItems, float priceFrom, float priceTo)
        {
            return priceTo == -1f
                ? _hContext.Houses.Where(house => priceFrom < house.Price)
                : _hContext.Houses
                    .Where(house => priceFrom < house.Price && house.Price < priceTo)
                    .Skip(index)
                    .Take(maxItems);
        }
    }
}