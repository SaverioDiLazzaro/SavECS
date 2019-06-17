using System;

// entities are essentially ids!
using ECSEntity = System.Int32;

namespace SavECS
{
    static class Time
    {
        public const float DeltaTime = 1f/60f;
    }

    // simple vector struct
    struct Vector3
    {
        public float x;
        public float y;
        public float z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3 operator *(Vector3 v, float f)
        {
            return new Vector3(v.x * f, v.y * f, v.z * f);
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public override string ToString()
        {
            return "x: " + this.x.ToString("0.000") + " y: " + this.y.ToString("0.000") + " z: " + this.z.ToString("0.000");
        }
    }

    // components are structs which inherit from 'IECSComponent' interface
    struct PositionComponent : IECSComponent
    {
        public Vector3 Position;
    }

    struct VelocityComponent : IECSComponent
    {
        public Vector3 Velocity;
    }

    // systems inherit from 'IECSSystem' interface
    // systems manipulate a group of entities based on component filters
    // systems are behaviours
    class MoveSystem : IECSSystem
    {
        // filters are type of components the system needs
        private readonly Type[] filters = new Type[]
        {
            typeof(PositionComponent),
            typeof(VelocityComponent)
        };

        Type[] IECSSystem.Filters { get { return this.filters; } }

        // specifies the order of execution of the system compared to the others
        int IECSSystem.ExecutionOrder { get { return 0; } }

        // system execution
        void IECSSystem.Execute(ECSEngine engine, ECSEntity entity)
        {
            // get data
            PositionComponent pc = engine.GetComponent<PositionComponent>(entity);
            VelocityComponent vc = engine.GetComponent<VelocityComponent>(entity);

            // manipulate data
            pc.Position += vc.Velocity * Time.DeltaTime;

            // set data
            engine.SetComponent(entity, pc);

            Debug.Log("Entity: " + entity + " - Pos: " + pc.Position, ConsoleColor.Green, entity);

            // other operations
            if (pc.Position.y < -10)
            {
                Console.Clear();
                Debug.Log("Destroyed: " + entity, ConsoleColor.Red, entity);

                // destroy entities
                engine.DestroyEntity(entity);
            }
        }
    }

    class Debug
    {
        internal static void Log(string message, ConsoleColor color = ConsoleColor.White, int location = -1)
        {
            if(location >= 0)
            {
                Console.SetCursorPosition(0, location);
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // create engine
            ECSEngine engine = new ECSEngine();

            // register components
            engine.RegisterComponent<PositionComponent>();
            engine.RegisterComponent<VelocityComponent>();

            // add systems
            MoveSystem moveSystem = new MoveSystem();
            engine.AddSystem(moveSystem);

            for (int i = 0; i < 5; i++)
            {
                // create entities
                ECSEntity entity = engine.CreateEntity();

                // add components
                engine.AddComponent(entity, new PositionComponent());
                engine.AddComponent(entity, new VelocityComponent() { Velocity = new Vector3(0f, -1f * (i + 1), 0f) });
            }

            // init engine
            engine.Init();

            while (true)
            {
                // run engine
                engine.Run();
                System.Threading.Thread.Sleep((int)(Time.DeltaTime * 1000));
            }
        }
    }
}
