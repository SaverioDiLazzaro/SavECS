using System;

using Aiv.Fast2D;
using OpenTK;

using ECSEntity = System.Int32;

public class PlayerShootSystem : IECSSystem
{
    Type[] IECSSystem.Filters => new Type[]
    {
        typeof(InputReceiverComponent),
        typeof(PositionComponent),
        typeof(WeaponComponent)
    };

    int IECSSystem.ExecutionOrder => (int)SystemExecutionOrder.Input;

    void IECSSystem.Execute(ECSEngine engine, ECSEntity[] entities)
    {
        for (int i = 0; i < entities.Length; i++)
        {
            ECSEntity entity = entities[i];

            if (Input.IsKeyDown(KeyCode.Space))
            {
                WeaponComponent wc = engine.GetComponent<WeaponComponent>(entity);
                PositionComponent pc = engine.GetComponent<PositionComponent>(entity);

                ECSEntity bullet = engine.CreateEntity();

                engine.AddComponent<NameComponent>(bullet,
                    new NameComponent()
                    {
                        Name = "BULLET"
                    });

                engine.AddComponent<PositionComponent>(bullet,
                    new PositionComponent()
                    {
                        Position = pc.Position + wc.BulletSpawnOffset
                    });

                engine.AddComponent<VelocityComponent>(bullet,
                    new VelocityComponent()
                    {
                        Velocity = new Vector2(0f, -10f)
                    });

                engine.AddComponent(bullet, new BoxColliderComponent() { Size = new Vector2(1f, 1f) });

                engine.AddComponent<SpriteRendererComponent>(bullet,
                    new SpriteRendererComponent()
                    {
                        RenderOffset = RenderOffset.Player,
                        Texture = TextureManager.GetTexture("LaserRed01"),
                        Sprite = new Sprite(0.1f, 1f) { pivot = new Vector2(0.1f, 0.5f) }
                    });

                engine.AddComponent<AIComponent>(bullet, new AIComponent());
            }
        }
    }
}