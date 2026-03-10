using neo.flow.data.DataContext;
using neo.flow.data.Models;
using neo.flow.data.Repository;

namespace neo.flow.data
{
    public class UnitOfWork : IDisposable
    {
        private readonly AppDbContext context = new();

        private GenericRepository<ProcessExecutionInstance>? _processExecutionInstanceRepository;
        private GenericRepository<BusinessStepExecutionInstance>? _businessStepExecutionInstanceRepository;

        public GenericRepository<ProcessExecutionInstance> ProcessExecutionInstanceRepository
        {
            get
            {
                _processExecutionInstanceRepository ??= new GenericRepository<ProcessExecutionInstance>(context);

                return _processExecutionInstanceRepository;
            }
        }

        public GenericRepository<BusinessStepExecutionInstance> BusinessStepExecutionInstanceRepository
        {
            get
            {
                _businessStepExecutionInstanceRepository ??= new GenericRepository<BusinessStepExecutionInstance>(context);

                return _businessStepExecutionInstanceRepository;
            }
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}
