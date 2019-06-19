using Aiv.Fast2D;
using OpenTK;
using System;

using ECSEntity = System.Int32;

public class PlayerShootSystem : IECSSystem
{
    Type[] IECSSystem.Filters => new Type[]
    {
        typeof(InputReceiverComponent),
        typeof(WeaponComponent),
        typeof(PositionComponent)
    };

    int IECSSystem.ExecutionOrder => (int)SystemExecutionOrder.Input;

    void IECSSystem.Execute(ECSEngine engine, int entity)
    {
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

            engine.AddComponent<SpriteRendererComponent>(bullet,
                new SpriteRendererComponent()
                {
                    RenderOffset = RenderOffset.Player,
                    Texture = new Texture(@"Assets\Player.png"),
                    Sprite = new Sprite(1f, 1f) { pivot = Vector2.One * 0.5f, Rotation = (float)Math.PI / 2f }
                });

            engine.AddComponent<AIComponent>(bullet, new AIComponent());
        }
    }
}