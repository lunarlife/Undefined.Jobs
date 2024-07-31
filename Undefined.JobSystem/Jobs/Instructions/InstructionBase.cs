using Undefined.JobSystem.Exceptions;

namespace Undefined.JobSystem.Jobs.Instructions;

public abstract class InstructionBase : IInstruction
{
    public InstructionStatus Status { get; private set; } = InstructionStatus.Executing;

    public bool Update(IJobManager jobManager)
    {
        if (Status == InstructionStatus.NoStatus) Status = InstructionStatus.Executing;
        if (Status != InstructionStatus.Executing) return true;
        if (!IsReady(jobManager)) return false;
        if (Status == InstructionStatus.Executing)
            Status = InstructionStatus.Completed;
        return true;
    }


    public void Reset()
    {
        Status = InstructionStatus.NoStatus;
        OnReset();
    }

    public void Cancel()
    {
        if (Status != InstructionStatus.Executing) throw new JobException("Job already completed.");
        Status = InstructionStatus.Cancelled;
        OnCancel();
    }

    protected abstract void OnCancel();
    protected abstract void OnReset();
    protected abstract bool IsReady(IJobManager jobManager);
}