using System;
using System.Collections.Generic;
using ECSEntity = System.Int32;

public class CollisionSolverSystem : IECSSystem
{
    Type[] IECSSystem.Filters => new Type[]
    {
        typeof(CollisionDataComponent),
    };

    int IECSSystem.ExecutionOrder => (int)SystemExecutionOrder.Physics;

    List<ECSEntity> entitiesToDestroy = new List<ECSEntity>();

    void IECSSystem.Execute(ECSEngine engine, ECSEntity[] entities)
    {
        for (int i = 0; i < entities.Length; i++)
        {
            ECSEntity entity = entities[i];

            CollisionDataComponent cdc = engine.GetComponent<CollisionDataComponent>(entity);

            ECSEntity entity1 = cdc.Entity1;
            ECSEntity entity2 = cdc.Entity2;

            engine.DestroyEntity(entity);

            if (!this.entitiesToDestroy.Contains(entity1))
            {
                this.entitiesToDestroy.Add(entity1);
            }

            if (!this.entitiesToDestroy.Contains(entity2))
            {
                this.entitiesToDestroy.Add(entity2);
            }
        }

        this.entitiesToDestroy.Sort((x1, x2) => -x1.CompareTo(x2));

        for (int i = 0; i < this.entitiesToDestroy.Count; i++)
        {
            ECSEntity entity = this.entitiesToDestroy[i];
            engine.DestroyEntity(entity);
        }

        this.entitiesToDestroy.Clear();
    }
}