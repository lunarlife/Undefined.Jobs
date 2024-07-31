using System;

namespace Undefined.JobSystem.Jobs.Instructions;

public class WaitBool : InstructionBase
{
    private readonly Func<bool> _func;

    public WaitBool(Func<bool> func)
    {
        _func = func;
    }

    protected override bool IsReady(IJobManager jobManager) => _func();

    protected override void OnCancel()
    {
    }

    protected override void OnReset()
    {
    }
}