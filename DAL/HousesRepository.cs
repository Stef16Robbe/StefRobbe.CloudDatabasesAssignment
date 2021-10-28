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

        public async Task<House> GetHousesPaginated(float priceFrom, float priceTo)
        {
            // await _hContext.Database.EnsureDeletedAsync();
            // await _hContext.Database.EnsureCreatedAsync();

            var house = await _hContext.Houses.FindAsync("fc5253c6-05a7-4d52-9e15-26cc9f64904c", "1521VJ");
            return house;
        }
    }
}