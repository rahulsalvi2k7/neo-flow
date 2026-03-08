using neo.flow.core.Interfaces;
using neo.flow.data.Models;

namespace neo.flow.logger.db.sql;

public class BusinessStepDbLogger : IDbLogger<IBusinessStep>
{
    private readonly AppDbContext _appDbContext;

    private BusinessStepExecutionInstance _businessStepExecutionInstance;

    public BusinessStepDbLogger(AppDbContext appDbContext)
    {
        this._appDbContext = appDbContext;

        _businessStepExecutionInstance = new BusinessStepExecutionInstance()
        {
            Id = Guid.NewGuid().ToString()
        };
    }

    public async Task LogStartExecutionAsync(IBusinessStep t, IExecutionContext context)
    {
        _businessStepExecutionInstance.ProcessExecutionInstanceId = context.Get<string>("processExecutionInstanceId") ?? string.Empty;
        _businessStepExecutionInstance.BusinessStepId = t.Name;
        _businessStepExecutionInstance.StartTime = context.DateTimeProvider.UtcNow();

        await _appDbContext.BusinessStepExecutionInstances.AddAsync(_businessStepExecutionInstance);
    }

    public async Task LogEndExecutionAsync(IBusinessStep t, IExecutionContext context)
    {
        _businessStepExecutionInstance.EndTime = context.DateTimeProvider.UtcNow();

        await _appDbContext.SaveChangesAsync();
    }

    public async Task LogExecutionAsync(IBusinessStep t, IExecutionContext context)
    {
        await _appDbContext.SaveChangesAsync();
    }
}
