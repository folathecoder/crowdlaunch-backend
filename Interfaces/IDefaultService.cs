namespace MARKETPLACEAPI.Interfaces;


public interface IDefaultService<T> {
  Task<IList<T>> GetAsync();
  Task<T?> GetAsync(string id);
  Task CreateAsync(T t);
  Task UpdateAsync(string id, T updatedT);
  Task RemoveAsync(string id);  
}