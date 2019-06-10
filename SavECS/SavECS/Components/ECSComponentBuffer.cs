using System;
using System.Collections.Generic;

using ECSEntityIndex = System.Int32;

internal class ECSComponentBuffer<T> : IECSComponentBuffer
    where T : struct, IECSComponent
{
    private readonly List<T> components = new List<T>();

    internal ECSEntityIndex AddComponent(T component)
    {
        this.components.Add(component);
        ECSEntityIndex index = this.components.Count - 1;
        return index;
    }
    void IECSComponentBuffer.RemoveComponent(int index)
    {
        this.components.RemoveAt(index);
    }
    internal T GetComponent(ECSEntityIndex index)
    {
        return this.components[index];
    }
    internal void SetComponent(int index, T component)
    {
        this.components[index] = component;
    }
}