using System.Collections.Generic;

internal sealed class ECSSystems : List<IECSSystem>
{
    internal new void Sort()
    {
        this.Sort((x1, x2) => x1.ExecutionOrder.CompareTo(x2.ExecutionOrder));
    }
}