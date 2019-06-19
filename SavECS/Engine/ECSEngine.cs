using System;

using ECSEntity = System.Int32;
using ECSEntityIndex = System.Int32;

public sealed class ECSEngine
{
    private readonly ECSComponents components = new ECSComponents();
    private readonly ECSEntities entities = new ECSEntities();
    private readonly ECSSystems systems = new ECSSystems();

    #region Engine
    public void Init()
    {
        this.systems.Sort();
    }
    public void Run()
    {
        for (int i = 0; i < this.systems.Count; i++)
        {
            IECSSystem system = this.systems[i];
            ECSEntity[] entities = this.GetAllEntitiesWithComponents(system.Filters);

            for (int id = entities.Length - 1; id >= 0; id--)
            {
                ECSEntity entity = entities[id];
                system.Execute(this, entity);
            }
        }
    }
    #endregion

    #region Systems
    public void AddSystem(IECSSystem system)
    {
        this.systems.Add(system);
    }
    public bool RemoveSystem(IECSSystem system)
    {
        return this.systems.Remove(system);
    }
    public void AddSystems(params IECSSystem[] systems)
    {
        for (int i = 0; i < systems.Length; i++)
        {
            this.systems.Add(systems[i]);
        }
    }
    public void RemoveSystems(params IECSSystem[] systems)
    {
        for (int i = 0; i < systems.Length; i++)
        {
            this.systems.Remove(systems[i]);
        }
    }
    #endregion

    #region Entities
    public ECSEntity CreateEntity()
    {
        return this.entities.CreateEntity();
    }
    public void DestroyEntity(ECSEntity entity)
    {
        ECSEntityData data = this.entities.GetEntityData(entity);

        var keys = data.Keys;
        Type[] types = new Type[keys.Count];
        int count = 0;

        //TODO: improve with Type[] buffer in ECSEntityData
        foreach (Type type in keys)
        {
            types[count++] = type;
        }

        for (int i = 0; i < types.Length; i++)
        {
            Type type = types[i];

            ECSEntityIndex index = data[type];
            data.Remove(type);

            this.components.RemoveComponent(type, index);

            this.UpdateAllEntitiesWithComponent(entity, type);
        }

        this.entities.DestroyEntity(entity);
    }
    private void UpdateAllEntitiesWithComponent(ECSEntity entity, Type type)
    {
        if(entity == this.entities.Count - 1)
        {
            return;
        }

        for (int i = entity; i < this.entities.Count; i++)
        {
            if (this.entities.HasComponent(i, type))
            {
                ECSEntityData data = this.entities[i];
                ECSEntityIndex index = data[type];
                index--;
                data[type] = index;
            }
        }
    }
    internal ECSEntity[] GetAllEntitiesWithComponents(Type[] types)
    {
        ECSEntity[] array = new ECSEntity[0];
        int count = 0;

        for (int entity = 0; entity < this.entities.Count; entity++)
        {
            if (this.entities.HasComponents(entity, types))
            {
                Array.Resize(ref array, count + 1);
                array[count] = entity;
                count++;
            }
        }

        return array;
    }
    #endregion

    #region Components
    private void RegisterComponent<T>() where T : struct, IECSComponent
    {
        this.components.RegisterComponent<T>();
    }
    public void AddComponent<T>(ECSEntity entity, T component) where T : struct, IECSComponent
    {
        this.RegisterComponent<T>();

        ECSEntityIndex index = this.components.AddComponent(entity, component);
        entities.AddComponent(entity, component, index);
    }
    public T GetComponent<T>(ECSEntity entity) where T : struct, IECSComponent
    {
        int index = this.GetIndexByEntity<T>(entity);

        T component = this.components.GetComponent<T>(index);
        return component;
    }
    public void SetComponent<T>(ECSEntity entity, T component) where T : struct, IECSComponent
    {
        int index = this.GetIndexByEntity<T>(entity);

        this.components.SetComponent(index, component);
    }
    private ECSEntityIndex GetIndexByEntity<T>(int entity) where T : struct, IECSComponent
    {
        ECSEntityData data = this.entities[entity];

        Type type = typeof(T);
        ECSEntityIndex index = data[type];

        return index;
    }
    #endregion
}