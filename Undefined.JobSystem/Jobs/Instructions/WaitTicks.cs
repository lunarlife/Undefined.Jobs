using Undefined.Verify;

namespace Undefined.JobSystem.Jobs.Instructions;

public class WaitTicks : InstructionBase
{
    private readonly int _ticksCount;
    private int _remain;

    public WaitTicks(int count = 1)
    {
            Verifying.Argument(count > 0, "Count of frames must be greater than 0.");
            _ticksCount = count;
        }

    protected override bool IsReady(IJobManager jobManager)
    {
            _remain++;
            if (_remain < _ticksCount) return false;
            Reset();
            return true;
        }

    protected override void OnCancel()
    {
        }

    protected override void OnReset()
    {
            _remain = 0;
        }
}