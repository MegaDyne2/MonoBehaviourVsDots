using Unity.Entities;
using UnityEngine;

public class SpawnCubesConfigAuthoring : MonoBehaviour
{

    public GameObject cubePrefab;
    public bool useMultithreading = false;
    
    public int amountToSpawn;

    public GameObject dotsBullets;
    
    
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
            
            AddComponent(entity, new SpawnBulletsConfig
            {
                bulletEntity = GetEntity(authoring.dotsBullets,TransformUsageFlags.Dynamic),
            });
        }
    }
    
}


public struct SpawnCubesConfig : IComponentData
{
    public Entity cubePrefabEntity;
    public bool useMultiThreading;
}

public struct SpawnBulletsConfig : IComponentData
{
    public Entity bulletEntity;
}