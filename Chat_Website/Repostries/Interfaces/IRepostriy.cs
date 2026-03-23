public interface IRepostriy<T>
{
    IQueryable<T> GetAll();
    List<T> GetAllList();
    T? GetByID(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    void Save();
}