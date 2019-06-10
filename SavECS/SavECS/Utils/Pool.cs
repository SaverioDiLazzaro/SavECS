using System;
using System.Collections.Generic;

public static class Pool<T> where T : class
{
    public static bool IsRegistered { get { return Pool<T>.instances != null; } }

    private static Queue<T> instances;
    private static Func<T> allocator;

    public static void Register(Func<T> allocator, int capacity = 0)
    {
        if (Pool<T>.IsRegistered)
            return;

        if (allocator == null)
            throw new NullReferenceException("Func<T> allocator can't be null");

        Pool<T>.instances = new Queue<T>(capacity);
        Pool<T>.allocator = allocator;
    }
    public static void UnRegister(Action<T> onDeRegister = null)
    {
        if (!Pool<T>.IsRegistered)
            return;

        if (onDeRegister != null)
        {
            int count = Pool<T>.instances.Count;
            for (int i = 0; i < count; i++)
            {
                onDeRegister.Invoke(Pool<T>.instances.Dequeue());
            }
        }

        Pool<T>.instances.Clear();
        Pool<T>.instances = null;
        Pool<T>.allocator = null;
    }
    public static T GetInstance(Action<T> onGet = null)
    {
        if (!Pool<T>.IsRegistered)
            throw new Exception("On GetInstance :: Pool is not registered");

        T toReturn = Pool<T>.instances.Count == 0 ? Pool<T>.allocator.Invoke() : Pool<T>.instances.Dequeue();
        if (onGet != null)
        {
            onGet.Invoke(toReturn);
        }
        return toReturn;
    }
    public static void RecycleInstance(T toRecycle, Action<T> onRecycle = null)
    {
        if (!Pool<T>.IsRegistered)
            throw new Exception("On RecycleInstance :: Pool is not registered");

        if (toRecycle == null)
        {
            return;
        }

        if (onRecycle != null)
        {
            onRecycle.Invoke(toRecycle);
        }

        Pool<T>.instances.Enqueue(toRecycle);
    }
}