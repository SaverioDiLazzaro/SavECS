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

            CollisionDataComponent cdc = engine.GetComponent<CollisionDataComponent>(entity);

            ECSEntity entity1 = cdc.Entity1;
            ECSEntity entity2 = cdc.Entity2;

            engine.DestroyEntity(entity);

            engine.DestroyEntity(entity1);
            engine.DestroyEntity(entity2);
        }
    }
}