using System.Collections.Generic;
using Undefined.JobSystem.Jobs;
using Undefined.JobSystem.Jobs.Instructions;

namespace Undefined.JobSystem;

public interface IJobManager
{
    public double DeltaTime { get; }
    public double TotalTime { get; }
    public IJob ExecuteJob(IEnumerable<IInstruction?> job);
}