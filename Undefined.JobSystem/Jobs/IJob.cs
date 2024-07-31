using Undefined.Events;
using Undefined.JobSystem.Events;
using Undefined.JobSystem.Jobs.Instructions;

namespace Undefined.JobSystem.Jobs;

public interface IJob : IInstruction
{
    public IEventAccess<JobStatusChangedEventArgs> OnStatusChanged { get; }
}