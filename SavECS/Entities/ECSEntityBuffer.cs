using System;
using ECSEntity = System.Int32;

internal class ECSEntityBuffer
{
    private ECSEntity[] buffer = new ECSEntity[0];

    internal void Add(ECSEntity entity, int newSize)
    {
        Array.Resize(ref buffer, newSize);
        buffer[newSize - 1] = entity;
    }
}