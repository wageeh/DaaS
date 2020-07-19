using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Repository
{
    public interface ICosmosDBRepository<T> where T : class
    {
        Task<T> CreateItemAsync(T item,string partitionKeyValue);
        Task<T> DeleteItemAsync(string id, string partitionKeyValue);
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllItemsAsync();
        Task<T> UpdateItemAsync(string id, T item);
    }
}