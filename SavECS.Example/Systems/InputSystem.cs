using Aiv.Fast2D;
using System;

public class InputSystem : IECSSystem
{
    Type[] IECSSystem.Filters => new Type[]
    {
        typeof(InputReceiverComponent),
        typeof(PositionComponent),
        typeof(VelocityComponent)
    };

    int IECSSystem.ExecutionOrder => (int)SystemExecutionOrder.Input;

    void IECSSystem.Execute(ECSEngine engine, int entity)
    {
        PositionComponent pc = engine.GetComponent<PositionComponent>(entity);
        VelocityComponent vc = engine.GetComponent<VelocityComponent>(entity);

        if (Input.IsKeyPressed(KeyCode.W))
        {
            pc.Position.Y -= vc.Velocity.Y * Time.DeltaTime;
        }

        if (Input.IsKeyPressed(KeyCode.S))
        {
            pc.Position.Y += vc.Velocity.Y * Time.DeltaTime;
        }

        if (Input.IsKeyPressed(KeyCode.A))
        {
            pc.Position.X -= vc.Velocity.X * Time.DeltaTime;
        }

        if (Input.IsKeyPressed(KeyCode.D))
        {
            pc.Position.X += vc.Velocity.X * Time.DeltaTime;
        }

        engine.SetComponent<PositionComponent>(entity, pc);
    }
}