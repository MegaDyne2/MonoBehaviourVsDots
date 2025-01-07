using Unity.Entities;
using UnityEngine;

/// <summary>
/// After lifetime runs out it will delete the entity
/// </summary>
public class LifetimeAuthoring : MonoBehaviour
{
    public float lifetime = 5.0f;
    
    private class Baker : Baker<LifetimeAuthoring>
    {
        public override void Bake(LifetimeAuthoring authoring)
        {
            Entity lifetimeAuthoring = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(lifetimeAuthoring, new Lifetime
            {
                TimeRemaining = authoring.lifetime
            });
        }
    }
}

public struct Lifetime : IComponentData
{
    public float TimeRemaining;
}