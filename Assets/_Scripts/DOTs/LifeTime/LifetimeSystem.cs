using Unity.Entities;

/// <summary>
/// System for Lifetime. when Entity with the component Lifetime reach Zero delete it.
/// </summary>
public partial struct LifetimeSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        //cannot delete this with SystemAPI
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);
        
        foreach (var (lifetime, entity) in SystemAPI.Query<RefRW<Lifetime>>().WithEntityAccess())
        {
            // Decrease the lifetime
            lifetime.ValueRW.TimeRemaining -= deltaTime;

            // Destroy the entity if lifetime is up
            if (lifetime.ValueRW.TimeRemaining <= 0f)
            {
                ecb.DestroyEntity(entity); // Queue destruction
            }
        }
        
        ecb.Playback(state.EntityManager);
    }
}