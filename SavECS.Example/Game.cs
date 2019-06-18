using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aiv.Fast2D;
using OpenTK;

using ECSEntity = System.Int32;

public static class Game
{
    public static ECSEngine engine = new ECSEngine();
    public static Window window = new Window(500, 800, "Game");
    public static Time time = new Time(Game.window);
    public static Random random = new Random();

    public static void Init()
    {
        // init window
        Game.window.SetDefaultOrthographicSize(20f);

        //register components
        engine.RegisterComponent<PositionComponent>();
        engine.RegisterComponent<VelocityComponent>();
        engine.RegisterComponent<InputReceiverComponent>();
        engine.RegisterComponent<SpriteRendererComponent>();
        engine.RegisterComponent<TimeComponent>();
        engine.RegisterComponent<SpawnPointComponent>();
        engine.RegisterComponent<NameComponent>();
        engine.RegisterComponent<AIComponent>();

        //add systems
        engine.AddSystem(new InputSystem());
        engine.AddSystem(new SpawnSystem());
        engine.AddSystem(new MoveSystem());
        engine.AddSystem(new SpriteRendererSystem());

        //create entities & add components
        #region Player
        ECSEntity player = engine.CreateEntity();

        var halfOrthoSize = Game.window.CurrentOrthoGraphicSize * 0.5f;

        engine.AddComponent<NameComponent>(player, new NameComponent() { Name = "PLAYER" });

        engine.AddComponent<InputReceiverComponent>(player, new InputReceiverComponent());

        engine.AddComponent<PositionComponent>(player,
            new PositionComponent()
            {
                Position = new Vector2(halfOrthoSize * window.aspectRatio, halfOrthoSize * 1.5f)
            });

        engine.AddComponent<VelocityComponent>(player,
           new VelocityComponent()
           {
               Velocity = new Vector2(5f, 5f)
           });

        engine.AddComponent<SpriteRendererComponent>(player,
            new SpriteRendererComponent()
            {
                RenderOffset = RenderOffset.Player,
                Texture = new Texture(@"Assets\Player.png"),
                Sprite = new Sprite(1f, 1f) { pivot = Vector2.One * 0.5f }
            });
        #endregion

        #region EnemySpawner(s)
        int spawnersCount = 5;
        float step = Game.window.OrthoWidth / (float)(spawnersCount + 1);
        float offset = -1f;

        float minTime = 1f;
        float maxTime = 3f;

        for (int i = 0; i < spawnersCount; i++)
        {
            var spawner = engine.CreateEntity();

            engine.AddComponent<NameComponent>(spawner, new NameComponent() { Name = "SPAWNER " + i });

            engine.AddComponent<SpawnPointComponent>(spawner,
                new SpawnPointComponent()
                {
                    SpawnPoint = new Vector2(step * (i + 1), offset)
                });

            var time = (float)(minTime + Game.random.NextDouble() * (maxTime - minTime));

            engine.AddComponent<TimeComponent>(spawner,
               new TimeComponent()
               {
                   Time = time,
                   CurrentTime = time
               });
        }
        #endregion

        // init engine
        Game.engine.Init();
    }

    public static void Run()
    {
        while (Game.window.opened)
        {
            Input.Update(Game.window);

            // run engine
            Game.engine.Run();

            // update window
            Game.window.Update();
        }
    }
}