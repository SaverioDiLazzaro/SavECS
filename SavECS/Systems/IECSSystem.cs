using System;

using ECSEntity = System.Int32;

public interface IECSSystem
{
    Type[] Filters { get; }
    int ExecutionOrder { get; }
    void Execute(ECSEngine engine, ECSEntity[] entities);
}