namespace Chat_Website.Repostries.Impelemtation
{
    public class GenericIRepostriy<T> : IRepostriy<T> where T : class
    {
        private readonly ChatDbContext _chatDbContext;
        private readonly DbSet<T> _dbSet;
        public GenericIRepostriy(ChatDbContext chatDbContext)
        {
            _chatDbContext = chatDbContext;
            _dbSet = _chatDbContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public List<T> GetAllList()
        {
            return _dbSet.ToList();
        }

        public T? GetByID(int id)
        {
            return _dbSet.Find(id);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Save()
        {
            _chatDbContext.SaveChanges();
        }
    }
}
