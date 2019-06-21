using ECSEntity = System.Int32;

public struct CollisionDataComponent : IECSComponent
{
    public ECSEntity Entity1;
    public ECSEntity Entity2;
}