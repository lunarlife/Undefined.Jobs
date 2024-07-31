using System.Collections.Generic;
using Undefined.Events;
using Undefined.JobSystem.Events;
using Undefined.JobSystem.Jobs;
using Undefined.JobSystem.Jobs.Instructions;

namespace Undefined.JobSystem;

public sealed class JobManager : IJobManager
{
    private readonly List<Job> _jobs = [];
    private readonly object _jobsLock = new();

    public double DeltaTime { get; private set; }

    public double TotalTime { get; private set; }

    private JobManager()
    {
    }

    public IJob ExecuteJob(IEnumerable<IInstruction?> job)
    {
        lock (_jobsLock)
        {
            var item = new Job(job);
            _jobs.Add(item);
            return item;
        }
    }

    private void TickHandler<T>(T args) where T : ITickEventArgs
    {
        DeltaTime = args.DeltaTime;
        TotalTime = args.TotalTime;
        lock (_jobsLock)
        {
            for (var i = 0; i < _jobs.Count; i++)
            {
                var job = _jobs[i];
                if (job.Update(this)) _jobs.Remove(job);
            }
        }
    }

    public static IJobManager CreateInstance<T>(IEventAccess<T> tickEvent) where T : ITickEventArgs
    {
        var manager = new JobManager();
        tickEvent.AddListener(manager.TickHandler);
        return manager;
    }
}