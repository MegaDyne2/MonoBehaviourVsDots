using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Physics;
using Unity.Transforms;

public partial struct TriggerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsWorldSingleton>();
    }

    public void OnUpdate(ref SystemState state)
    {
        //get the EntityManager
        EntityManager entityManager = state.EntityManager;

        //Get all Entitites
        NativeArray<Entity> entities = entityManager.GetAllEntities(Allocator.Temp);

        //go though Each ones
        foreach (Entity entity in entities)
        {
            
            //look though Sphere Trigger
            if (entityManager.HasComponent<SphereTriggerComponent>(entity))
            {
                //get said items
                RefRW<LocalToWorld> triggerTransform = SystemAPI.GetComponentRW<LocalToWorld>(entity);
                RefRO<SphereTriggerComponent> triggerComponent = SystemAPI.GetComponentRO<SphereTriggerComponent>(entity);

                //set the size of the sphere
                float size = triggerComponent.ValueRO.RadianSize;
                triggerTransform.ValueRW.Value.c0 = new float4(size, 0, 0, 0);
                triggerTransform.ValueRW.Value.c1 = new float4(0, size, 0, 0);
                triggerTransform.ValueRW.Value.c2 = new float4(0, 0, size, 0);

                PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

                //do the casting
                NativeList<ColliderCastHit> hits = new NativeList<ColliderCastHit>(Allocator.Temp);

                physicsWorld.SphereCastAll(triggerTransform.ValueRO.Position, triggerComponent.ValueRO.RadianSize / 2,
                    float3.zero, 1, ref hits, new CollisionFilter 
                    { 
                        BelongsTo = (uint)CollisionLayer.Player, 
                        CollidesWith = (uint)CollisionLayer.Collectible 
                    });
                
                //what do we got?
                foreach (ColliderCastHit hit in hits)
                {
                    entityManager.DestroyEntity(hit.Entity);
                }
                
                hits.Dispose();
            }
        }

        entities.Dispose();
    }
}

public enum CollisionLayer
{
    Player = 1 << 6,
    Collectible = 1 << 7
}