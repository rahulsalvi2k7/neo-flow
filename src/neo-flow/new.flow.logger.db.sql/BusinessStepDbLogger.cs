using neo.flow.core.Interfaces;
using neo.flow.data;
using neo.flow.data.DataContext;
using neo.flow.data.Models;

namespace neo.flow.logger.db.sql;

public class BusinessStepDbLogger : IDbLogger<IBusinessStep>
{
    private readonly AppDbContext _appDbContext;
    private readonly UnitOfWork _unitOfWork;

    private BusinessStepExecutionInstance _businessStepExecutionInstance;

    public BusinessStepDbLogger(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        _unitOfWork = new UnitOfWork();

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

        await _unitOfWork.BusinessStepExecutionInstanceRepository.Insert(_businessStepExecutionInstance);
    }

    public Task LogEndExecutionAsync(IBusinessStep t, IExecutionContext context)
    {
        _businessStepExecutionInstance.EndTime = context.DateTimeProvider.UtcNow();

        _unitOfWork.BusinessStepExecutionInstanceRepository.Update(_businessStepExecutionInstance);

        return Task.CompletedTask;
    }

    public async Task LogExecutionAsync(IBusinessStep t, IExecutionContext context)
    {
        await _unitOfWork.SaveAsync();
    }
}
