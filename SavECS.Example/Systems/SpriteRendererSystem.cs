using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SpriteRendererSystem : IECSSystem
{
    Type[] IECSSystem.Filters => new Type[]
    {
        typeof(PositionComponent),
        typeof(SpriteRendererComponent)
    };

    int IECSSystem.ExecutionOrder => (int)SystemExecutionOrder.Render;

    void IECSSystem.Execute(ECSEngine engine, int entity)
    {
        PositionComponent pc = engine.GetComponent<PositionComponent>(entity);
        SpriteRendererComponent src = engine.GetComponent<SpriteRendererComponent>(entity);

        src.Sprite.position = pc.Position;
        engine.SetComponent<SpriteRendererComponent>(entity, src);

        src.Sprite.DrawTexture(src.Texture);
    }
}