using Unity.Entities;

public struct ThreadingToggle : IComponentData
{
    public bool UseMultiThreading;
}