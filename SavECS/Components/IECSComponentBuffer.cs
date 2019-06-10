using System;
using ECSEntityIndex = System.Int32;

public interface IECSComponentBuffer
{
    void RemoveComponent(ECSEntityIndex index);
}