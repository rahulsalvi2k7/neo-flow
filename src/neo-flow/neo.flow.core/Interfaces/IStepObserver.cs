namespace neo.flow.core.Interfaces
{
    public interface IStepObserver
    {
        Task OnStepStarted(string stepName);

        Task OnStepCompleted(string stepName);

        Task OnStepFailed(string stepName);
    }
}
