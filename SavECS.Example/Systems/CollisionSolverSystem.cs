using System;

using ECSEntity = System.Int32;

public class CollisionSolverSystem : IECSSystem
{
    Type[] IECSSystem.Filters => new Type[]
    {
        typeof(CollisionDataComponent),
    };

    int IECSSystem.ExecutionOrder => (int)SystemExecutionOrder.Physics;

    void IECSSystem.Execute(ECSEngine engine, ECSEntity[] entities)
    {
        for (int i = 0; i < entities.Length; i++)
        {
            ECSEntity entity = entities[i];

            engine.DestroyEntity(entity);
        }
    }
}