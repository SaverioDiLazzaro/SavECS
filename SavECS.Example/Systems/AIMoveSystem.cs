using System;

public class AIMoveSystem : IECSSystem
{
    Type[] IECSSystem.Filters => new Type[]
    {
        typeof(PositionComponent),
        typeof(VelocityComponent),
        typeof(AIComponent)
    };

    int IECSSystem.ExecutionOrder => (int)SystemExecutionOrder.LateUpdate;

    void IECSSystem.Execute(ECSEngine engine, int entity)
    {
        PositionComponent pc = engine.GetComponent<PositionComponent>(entity);
        VelocityComponent vc = engine.GetComponent<VelocityComponent>(entity);

        pc.Position += vc.Velocity * Time.DeltaTime;

        engine.SetComponent<PositionComponent>(entity, pc);
    }
}
