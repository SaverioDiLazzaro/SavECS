using System;

using OpenTK;

public class DestroyEntityWhenOutOfScreenSystem : IECSSystem
{
    private float extraBounds = 3f;

    Type[] IECSSystem.Filters => new Type[]
    {
        typeof(PositionComponent),
        typeof(AIComponent)
    };

    int IECSSystem.ExecutionOrder => (int)SystemExecutionOrder.LateUpdate;

    void IECSSystem.Execute(ECSEngine engine, int entity)
    {
        PositionComponent pc = engine.GetComponent<PositionComponent>(entity);

        if (this.IsOutOfScreen(pc.Position))
        {
            engine.DestroyEntity(entity);
        }
    }

    private bool IsOutOfScreen(Vector2 position)
    {
        float width =  Game.Window.OrthoWidth;
        float height = Game.Window.OrthoHeight;
        return position.X < -extraBounds || position.X > width + extraBounds || position.Y < -extraBounds || position.Y > height + extraBounds;
    }
}