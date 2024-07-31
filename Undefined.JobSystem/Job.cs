using System.Collections.Generic;
using Undefined.Events;
using Undefined.JobSystem.Events;
using Undefined.JobSystem.Jobs;
using Undefined.JobSystem.Jobs.Instructions;

namespace Undefined.JobSystem;

public class Job : InstructionBase, IJob
{
    private readonly IEnumerable<IInstruction?> _job;
    private readonly object _lockObj = new();
    private readonly Event<JobStatusChangedEventArgs> _onStatusChanged = new();
    private IEnumerator<IInstruction?>? _enumerator;

    public IEventAccess<JobStatusChangedEventArgs> OnStatusChanged => _onStatusChanged.Access;

    public Job(IEnumerable<IInstruction?> job)
    {
        _job = job;
    }

    protected override bool IsReady(IJobManager jobManager)
    {
        lock (_lockObj)
        {
            if (_enumerator == null)
            {
                _enumerator = _job.GetEnumerator();
                if (!_enumerator.MoveNext()) return true;
            }

            var current = _enumerator.Current;
            if (current is null)
                return !_enumerator.MoveNext();
            if (current.Update(jobManager)) return !_enumerator.MoveNext();
            return false;
        }
    }

    protected override void OnCancel()
    {
        if (_enumerator?.Current is { Status: InstructionStatus.Executing } instruction)
            instruction.Cancel();
    }

    protected override void OnReset() => _enumerator = null;
}