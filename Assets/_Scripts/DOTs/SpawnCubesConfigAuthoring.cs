using Unity.Entities;
using UnityEngine;

public class SpawnCubesConfigAuthoring : MonoBehaviour
{

    public GameObject cubePrefab;
    public bool useMultithreading = false;
    
    public int amountToSpawn;
    
    
    
    public class Baker : Baker<SpawnCubesConfigAuthoring>
    {
        public override void Bake(SpawnCubesConfigAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            
            AddComponent(entity, new SpawnCubesConfig
            {
                cubePrefabEntity = GetEntity(authoring.cubePrefab,TransformUsageFlags.Dynamic),
                useMultiThreading = authoring.useMultithreading // Set the field

            });
        }
    }
    
}


public struct SpawnCubesConfig : IComponentData
{
    public Entity cubePrefabEntity;
    public bool useMultiThreading;
}