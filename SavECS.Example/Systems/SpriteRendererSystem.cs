using System;

using ECSEntity = System.Int32;

public class SpriteRendererSystem : IECSSystem
{
    Type[] IECSSystem.Filters => new Type[]
    {
        typeof(PositionComponent),
        typeof(SpriteRendererComponent)
    };

    int IECSSystem.ExecutionOrder => (int)SystemExecutionOrder.Render;

    void IECSSystem.Execute(ECSEngine engine, ECSEntity[] entities)
    {
        for (int i = 0; i < entities.Length; i++)
        {
            ECSEntity entity = entities[i];

            PositionComponent pc = engine.GetComponent<PositionComponent>(entity);
            SpriteRendererComponent src = engine.GetComponent<SpriteRendererComponent>(entity);

            src.Sprite.position = pc.Position;
            engine.SetComponent<SpriteRendererComponent>(entity, src);

            src.Sprite.DrawTexture(src.Texture);
        }
    }
}