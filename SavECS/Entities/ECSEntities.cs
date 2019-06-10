using System;
using System.Collections.Generic;

using ECSEntity = System.Int32;
using ECSEntityIndex = System.Int32;

internal sealed class ECSEntities : List<ECSEntityData>
{
    #region Static
    static ECSEntities()
    {
        Pool<ECSEntityData>.Register(() => new ECSEntityData(), 256);
    }
    #endregion

    #region Entities
    internal ECSEntity CreateEntity()
    {
        ECSEntityData data = Pool<ECSEntityData>.GetInstance();

        this.Add(data);
        ECSEntity entity = this.Count - 1;
        return entity;
    }
    internal void DestroyEntity(ECSEntity entity)
    {
        ECSEntityData data = this[entity];
        this.Remove(data);

        Pool<ECSEntityData>.RecycleInstance(data, x => x.Clear());
    }
    internal ECSEntityData GetEntityData(ECSEntity entity)
    {
        ECSEntityData data = this[entity];
        return data;
    }
    #endregion

    #region Components
    internal void AddComponent<T>(ECSEntity entity, T component, ECSEntityIndex index) where T : struct, IECSComponent
    {
        ECSEntityData data = this[entity];
        Type type = typeof(T);
        data.Add(type, index);
    }
    internal bool HasComponents(ECSEntity entity, Type[] types)
    {
        for (int i = 0; i < types.Length; i++)
        {
            Type type = types[i];

            if (!this.HasComponent(entity, type))
            {
                return false;
            }
        }

        return true;
    }
    internal bool HasComponent(ECSEntity entity, Type type)
    {
        ECSEntityData data = this[entity];

        if (data.ContainsKey(type))
        {
            return true;
        }

        return false;
    }
    #endregion
}