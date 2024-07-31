using Undefined.Events;
using Undefined.JobSystem.Jobs;

namespace Undefined.JobSystem.Events;

public class JobStatusChangedEventArgs : IEventArgs
{
    public IJob Job { get; }

    public JobStatusChangedEventArgs(IJob job)
    {
        Job = job;
    }
}