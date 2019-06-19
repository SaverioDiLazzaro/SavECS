using System;

public class FollowEntityPositionSystem : IECSSystem
{
    Type[] IECSSystem.Filters => new Type[] 
    {
        typeof(PositionComponent),
        typeof(EntityHolderComponent)
    };

    int IECSSystem.ExecutionOrder => (int)SystemExecutionOrder.LateUpdate;

    void IECSSystem.Execute(ECSEngine engine, int entity)
    {
        PositionComponent pc = engine.GetComponent<PositionComponent>(entity);
        EntityHolderComponent ehc = engine.GetComponent<EntityHolderComponent>(entity);
        
        PositionComponent pcOther = engine.GetComponent<PositionComponent>(ehc.Entity);
        pc.Position = pcOther.Position;

        engine.SetComponent<PositionComponent>(entity, pc);
    }
}