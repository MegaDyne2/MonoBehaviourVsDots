using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public class RotatingCubeAuthoring : MonoBehaviour
{

    public class Baker : Baker<RotatingCubeAuthoring>
    {
        public override void Bake(RotatingCubeAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent(entity, new RotatingCube());
            
            
            //Add collision
            // Add a PhysicsCollider (e.g., box collider)
            // var boxCollider = Unity.Physics.BoxCollider.Create(new BoxGeometry
            // {
            //     Center = float3.zero,
            //     Size = new float3(1, 1, 1)
            // });
            //AddComponent(entity, new PhysicsCollider { Value = boxCollider });
            
        }
    }
}

public struct RotatingCube : IComponentData
{
    
}