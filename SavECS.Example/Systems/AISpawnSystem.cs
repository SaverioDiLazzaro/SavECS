using System;

using Aiv.Fast2D;
using OpenTK;

using ECSEntity = System.Int32;

public class AISpawnSystem : IECSSystem
{
    Type[] IECSSystem.Filters => new Type[]
    {
        typeof(TimeComponent),
        typeof(SpawnPointComponent),
    };

    int IECSSystem.ExecutionOrder => (int)SystemExecutionOrder.Update;

    void IECSSystem.Execute(ECSEngine engine, ECSEntity[] entities)
    {
        for (int i = 0; i < entities.Length; i++)
        {
            ECSEntity entity = entities[i];

            TimeComponent tc = engine.GetComponent<TimeComponent>(entity);

            tc.CurrentTime -= Time.DeltaTime;

            if (tc.CurrentTime <= 0f)
            {
                tc.CurrentTime += tc.Time;

                SpawnPointComponent pc = engine.GetComponent<SpawnPointComponent>(entity);

                var enemy = engine.CreateEntity();

                engine.AddComponent<NameComponent>(enemy,
                    new NameComponent()
                    {
                        Name = "ENEMY"
                    });

                engine.AddComponent<PositionComponent>(enemy,
                    new PositionComponent()
                    {
                        Position = pc.SpawnPoint
                    });

                engine.AddComponent<VelocityComponent>(enemy,
                    new VelocityComponent()
                    {
                        Velocity = new Vector2(0f, 5f)
                    });

                engine.AddComponent(enemy, new BoxColliderComponent() { Size = new Vector2(1f, 1f) });

                engine.AddComponent<SpriteRendererComponent>(enemy,
                    new SpriteRendererComponent()
                    {
                        RenderOffset = RenderOffset.Player,
                        Texture = TextureManager.GetTexture("EnemyBlack2"),
                        Sprite = new Sprite(1f, 1f) { pivot = Vector2.One * 0.5f }
                    });

                engine.AddComponent<AIComponent>(enemy, new AIComponent());
            }

            engine.SetComponent<TimeComponent>(entity, tc);
        }
    }
}