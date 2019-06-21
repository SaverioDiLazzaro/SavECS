using OpenTK;
using System;

using ECSEntity = System.Int32;

public class CollisionDetectionSystem : IECSSystem
{
    Type[] IECSSystem.Filters => new Type[]
    {
        typeof(PositionComponent),
        typeof(BoxColliderComponent)
    };

    int IECSSystem.ExecutionOrder => (int)SystemExecutionOrder.Physics;

    void IECSSystem.Execute(ECSEngine engine, ECSEntity[] entities)
    {
        for (int i = 0; i < entities.Length - 1; i++)
        {
            ECSEntity entity1 = entities[i];

            for (int j = i + 1; j < entities.Length; j++)
            {
                ECSEntity entity2 = entities[j];

                if(entity1 != entity2)
                {
                    PositionComponent pc1 = engine.GetComponent<PositionComponent>(entity1);
                    BoxColliderComponent box1 = engine.GetComponent<BoxColliderComponent>(entity1);

                    Vector2 box1ExtentMin = this.GetExtentMin(pc1, box1);
                    Vector2 box1ExtentMax = this.GetExtentMax(pc1, box1);

                    PositionComponent pc2 = engine.GetComponent<PositionComponent>(entity2);
                    BoxColliderComponent box2 = engine.GetComponent<BoxColliderComponent>(entity2);

                    Vector2 box2ExtentMin = this.GetExtentMin(pc2, box2);
                    Vector2 box2ExtentMax = this.GetExtentMax(pc2, box2);

                    if(this.Intersect(box1ExtentMin, box1ExtentMax, box2ExtentMin, box2ExtentMax))
                    {
                        ECSEntity collisionDataEntity = engine.CreateEntity();

                        engine.AddComponent(collisionDataEntity, new CollisionDataComponent() { Entity1 = entity1, Entity2 = entity2 });
                    }
                }
            }
        }
    }

    private Vector2 GetExtentMin(PositionComponent pc, BoxColliderComponent box)
    {
        return new Vector2(pc.Position.X - (box.Size.X * 0.5f), pc.Position.Y - (box.Size.Y * 0.5f));
    }

    private Vector2 GetExtentMax(PositionComponent pc, BoxColliderComponent box)
    {
        return new Vector2(pc.Position.X + (box.Size.X * 0.5f), pc.Position.Y + (box.Size.Y * 0.5f));
    }

    private bool Intersect(Vector2 box1ExtentMin, Vector2 box1ExtentMax, Vector2 box2ExtentMin, Vector2 box2ExtentMax)
    {
        return (box1ExtentMin.X < box2ExtentMax.X &&
                box1ExtentMax.X > box2ExtentMin.X &&
                box1ExtentMin.Y < box2ExtentMax.Y &&
                box1ExtentMax.Y > box2ExtentMin.Y);
    }
}