using System;

using ECSEntity = System.Int32;

public class AIMoveSystem : IECSSystem
{
    Type[] IECSSystem.Filters => new Type[]
    {
        typeof(PositionComponent),
        typeof(VelocityComponent),
        typeof(AIComponent)
    };

    int IECSSystem.ExecutionOrder => (int)SystemExecutionOrder.Update;

    void IECSSystem.Execute(ECSEngine engine, ECSEntity[] entities)
    {
        for (int i = 0; i < entities.Length; i++)
        {
            ECSEntity entity = entities[i];

            PositionComponent pc = engine.GetComponent<PositionComponent>(entity);
            VelocityComponent vc = engine.GetComponent<VelocityComponent>(entity);

            pc.Position += vc.Velocity * Time.DeltaTime;

            engine.SetComponent<PositionComponent>(entity, pc);
        }
    }
}
