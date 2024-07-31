using System;

namespace Undefined.JobSystem.Exceptions;

public class LoopException : Exception
{
    public LoopException(string? message) : base(message)
    {
    }
}