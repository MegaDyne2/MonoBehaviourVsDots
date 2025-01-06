using Unity.Entities;

public partial struct LifetimeSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

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