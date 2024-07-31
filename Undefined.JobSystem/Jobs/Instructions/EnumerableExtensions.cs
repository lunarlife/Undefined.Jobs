using System.Collections.Generic;

namespace Undefined.JobSystem.Jobs.Instructions;

public static class EnumerableExtensions
{
    public static IInstruction AsJob(this IEnumerable<IInstruction?> enumerable) => new WaitNewJob(enumerable);
}