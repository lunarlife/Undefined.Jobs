using System.Collections.Generic;

namespace Undefined.JobSystem.Jobs.Instructions;

public class WaitNewJob : InstructionBase
{
    private readonly IEnumerable<IInstruction?> _instructions;
    private IEnumerator<IInstruction?>? _enumerator;

    public WaitNewJob(IEnumerable<IInstruction?> instructions)
    {
        _instructions = instructions;
    }

    protected override bool IsReady(IJobManager jobManager)
    {
        if (_enumerator == null)
        {
            _enumerator = _instructions.GetEnumerator();
            if (!_enumerator.MoveNext()) return true;
        }

        var current = _enumerator.Current;
        if (current is null)
            return !_enumerator.MoveNext();
        if (current.Update(jobManager)) return !_enumerator.MoveNext();
        return false;
    }

    protected override void OnCancel()
    {
        _enumerator?.Current?.Cancel();
    }

    protected override void OnReset() => _enumerator = _instructions.GetEnumerator();
}