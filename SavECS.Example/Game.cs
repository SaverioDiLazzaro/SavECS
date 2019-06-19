using System;

using Aiv.Fast2D;
using OpenTK;

using ECSEntity = System.Int32;

public static class Game
{
    private static ECSEngine engine = new ECSEngine();
    public static Window Window = new Window(500, 800, "Game");
    public static Random Random = new Random();

    public static void Init()
    {
        // init window
        Game.Window.SetDefaultOrthographicSize(20f);

        //add systems
        engine.AddSystem(new PlayerMoveSystem());
        engine.AddSystem(new PlayerShootSystem());
        engine.AddSystem(new AISpawnSystem());
        engine.AddSystem(new AIMoveSystem());
        engine.AddSystem(new FollowEntityPositionSystem());
        engine.AddSystem(new SpriteRendererSystem());

        //create entities & add components
        #region Player
        ECSEntity player = engine.CreateEntity();

        var halfOrthoSize = Game.Window.CurrentOrthoGraphicSize * 0.5f;

        engine.AddComponent(player, new NameComponent() { Name = "PLAYER" });

        engine.AddComponent(player, new InputReceiverComponent());

        engine.AddComponent(player, new PositionComponent() { Position = new Vector2(halfOrthoSize * Window.aspectRatio, halfOrthoSize * 1.5f) });

        engine.AddComponent(player, new VelocityComponent() { Velocity = new Vector2(5f, 5f) });

        engine.AddComponent(player,
            new SpriteRendererComponent()
            {
                RenderOffset = RenderOffset.Player,
                Texture = new Texture(@"Assets\Player.png"),
                Sprite = new Sprite(1f, 1f) { pivot = Vector2.One * 0.5f }
            });
        #endregion

        #region Weapon (Player)
        ECSEntity playerWeapon = engine.CreateEntity();
        engine.AddComponent(playerWeapon, new InputReceiverComponent());
        engine.AddComponent(playerWeapon, new PositionComponent());
        engine.AddComponent(playerWeapon, new WeaponComponent() { BulletSpawnOffset = new Vector2(0f, -1f) });
        engine.AddComponent(playerWeapon, new EntityHolderComponent() { Entity = player });
        #endregion

        #region EnemySpawner(s)
        int spawnersCount = 5;
        float step = Game.Window.OrthoWidth / (float)(spawnersCount + 1);
        float offset = -1f;

        float minTime = 1f;
        float maxTime = 3f;

        for (int i = 0; i < spawnersCount; i++)
        {
            var spawner = engine.CreateEntity();

            engine.AddComponent(spawner, new NameComponent() { Name = "SPAWNER " + i });

            engine.AddComponent(spawner,
                new SpawnPointComponent()
                {
                    SpawnPoint = new Vector2(step * (i + 1), offset)
                });

            var time = (float)(minTime + Game.Random.NextDouble() * (maxTime - minTime));

            engine.AddComponent(spawner,
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
        while (Game.Window.opened)
        {
            Input.Update(Game.Window);

            // run engine
            Game.engine.Run();

            // update window
            Game.Window.Update();
        }
    }
}