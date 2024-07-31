using System.Diagnostics;
using System.Threading;
using Undefined.Events;
using Undefined.JobSystem.Events;
using Undefined.JobSystem.Exceptions;
using Undefined.Verify;

namespace Undefined.JobSystem;

public class TickLoop
{
    private readonly Event<ITickEventArgs> _onTick = new();
    private int _targetTickRate = 100;

    public bool IsActive { get; private set; }

    public double DeltaTime { get; private set; }

    public double TotalTime { get; private set; }

    public IEventAccess<ITickEventArgs> OnTick => _onTick.Access;


    public int TargetTickRate
    {
        get => _targetTickRate;
        set
        {
            if (_targetTickRate < 1 && _targetTickRate != -1)
                throw new LoopException($"{nameof(TargetTickRate)} must be more than 1 or equals -1.");
            _targetTickRate = value;
        }
    }

    public void Start()
    {
        Verifying.Argument(!IsActive, "The tick loop is already active.");
        IsActive = true;
        new Thread(() =>
        {
            var frameTime = 0f;
            var sw = new Stopwatch();
            while (IsActive)
            {
                if (TargetTickRate != -1)
                {
                    var target = 1000f / TargetTickRate;
                    var passed = target - frameTime;
                    var sleep = (int)passed;
                    if (sleep > 0) Thread.Sleep(sleep);
                    frameTime += sleep;
                }

                sw.Restart();

                var frameSec = frameTime / 1000f;
                DeltaTime = frameSec;
                TotalTime += frameSec;
                _onTick.Raise(new TickEventArgs(frameSec, TotalTime));

                sw.Stop();
                frameTime = sw.ElapsedTicks / 10000f;
            }
        })
        {
            Name = "Tick Loop"
        }.Start();
    }

    public void Stop()
    {
        Verifying.Argument(IsActive, "The tick loop is already active.");
        IsActive = false;
    }

    private class TickEventArgs : ITickEventArgs
    {
        public double DeltaTime { get; }
        public double TotalTime { get; }

        public TickEventArgs(double deltaTime, double totalTime)
        {
            DeltaTime = deltaTime;
            TotalTime = totalTime;
        }
    }
}