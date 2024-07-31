using System;

namespace Undefined.JobSystem.Exceptions;

public class JobException : Exception
{
    public JobException(string? message) : base(message)
    {
    }
}