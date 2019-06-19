using Aiv.Fast2D;
using OpenTK;
using System;

public class AISpawnSystem : IECSSystem
{
    Type[] IECSSystem.Filters => new Type[]
    {
        typeof(TimeComponent),
        typeof(SpawnPointComponent),
    };

    int IECSSystem.ExecutionOrder => (int)SystemExecutionOrder.Update;

    void IECSSystem.Execute(ECSEngine engine, int entity)
    {
        TimeComponent tc = engine.GetComponent<TimeComponent>(entity);

        tc.CurrentTime -= Time.DeltaTime;

        if(tc.CurrentTime <= 0f)
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

            engine.AddComponent<SpriteRendererComponent>(enemy,
                new SpriteRendererComponent()
                {
                    RenderOffset = RenderOffset.Player,
                    Texture = new Texture(@"Assets\EnemyBlack2.png"),
                    Sprite = new Sprite(1f, 1f) { pivot = Vector2.One * 0.5f }
                });

            engine.AddComponent<AIComponent>(enemy, new AIComponent());
        }

        engine.SetComponent<TimeComponent>(entity, tc);
    }
}