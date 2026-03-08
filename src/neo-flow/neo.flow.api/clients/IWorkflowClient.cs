using Refit;

namespace neo.flow.api.clients
{
    public interface IWorkflowClient
    {
        [Post("/workflow/{name}/execute")]
        Task ExecuteAsync(string name, [Body] Dictionary<string, object> variables);
    }
}
