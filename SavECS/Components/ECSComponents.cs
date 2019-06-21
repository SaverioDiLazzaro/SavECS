using System;
using System.Collections.Generic;

using ECSEntity = System.Int32;
using ECSEntityIndex = System.Int32;

internal sealed class ECSComponents : Dictionary<Type, IECSComponentBuffer>
{
    internal void RegisterComponent<T>() where T : struct, IECSComponent
    {
        Type type = typeof(T);

        if (!this.ContainsKey(type))
        {
            IECSComponentBuffer buffer = new ECSComponentBuffer<T>();
            this.Add(type, buffer);
        }
    }

    internal ECSEntityIndex AddComponent<T>(ECSEntity entity, T component) where T : struct, IECSComponent
    {
        Type type = typeof(T);
        ECSComponentBuffer<T> buffer = this[type] as ECSComponentBuffer<T>;
        ECSEntityIndex index = buffer.AddComponent(component);
        return index;
    }

    internal T GetComponent<T>(ECSEntityIndex index) where T : struct, IECSComponent
    {
        Type type = typeof(T);
        ECSComponentBuffer<T> buffer = this[type] as ECSComponentBuffer<T>;
        T component = buffer.GetComponent(index);
        return component;
    }

    internal void SetComponent<T>(ECSEntityIndex index, T component) where T : struct, IECSComponent
    {
        Type type = typeof(T);
        ECSComponentBuffer<T> buffer = this[type] as ECSComponentBuffer<T>;
        buffer.SetComponent(index, component);
    }

    internal void RemoveComponent(Type type, ECSEntityIndex index)
    {
        IECSComponentBuffer buffer = this[type];
        buffer.RemoveComponent(index);
    }
}