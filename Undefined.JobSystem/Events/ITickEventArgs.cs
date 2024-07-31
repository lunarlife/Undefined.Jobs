using Undefined.Events;

namespace Undefined.JobSystem.Events;

public interface ITickEventArgs : IEventArgs
{
    public double DeltaTime { get; }
    public double TotalTime { get; }
}