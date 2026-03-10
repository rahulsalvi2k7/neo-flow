using Microsoft.EntityFrameworkCore;
using neo.flow.data.DataContext;

namespace neo.flow.data.Repository
{
    public class GenericRepository<T> where T : class
    {
        internal AppDbContext context;

        internal DbSet<T> dbSet;

        public GenericRepository(AppDbContext appDbContext)
        {
            context = appDbContext;

            dbSet = context.Set<T>();
        }

        public virtual async Task Insert(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            dbSet.Update(entity);
        }
    }
}
