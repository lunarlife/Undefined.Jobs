namespace Undefined.JobSystem.Jobs.Instructions;

public interface IInstruction
{
    public InstructionStatus Status { get; }
    public bool Update(IJobManager jobManager);
    public void Reset();
    public void Cancel();
}