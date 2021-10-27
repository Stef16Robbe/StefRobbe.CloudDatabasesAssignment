using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace DAL
{
    public interface ICosmosDbService<T>
    {
        Task<IEnumerable<T>> GetMultipleAsync(string query);
        Task<T> GetAsync(string id);
        Task<T> AddAsync(T item);
        Task<T> UpdateAsync(string id, T item);
        Task<T> DeleteAsync(string id);
        Task<PaginationItem<T>> GetMultiplePaginationAsync(string query, int maxItemCount, string continuationToken);
        Task<IEnumerable<T>> DetectDuplicate(string queryString);
    }
}