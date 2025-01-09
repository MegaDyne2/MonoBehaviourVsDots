using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

public class PrefabSpawnerAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject prefabCubesDots;
    [SerializeField] private GameObject prefabBulletDots;
    
    public class Baker : Baker<PrefabSpawnerAuthoring>
    {
        public override void Bake(PrefabSpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            
            AddComponent(entity, new SpawnCubesConfig
            {
                cubePrefabEntity = GetEntity(authoring.prefabCubesDots, TransformUsageFlags.Dynamic)
            });

            AddComponent(entity, new SpawnBulletsConfig
            {
                bulletEntity = GetEntity(authoring.prefabBulletDots, TransformUsageFlags.Dynamic),
            });
        }
    }
}


public struct SpawnCubesConfig : IComponentData
{
    public Entity cubePrefabEntity;
}

public struct SpawnBulletsConfig : IComponentData
{
    public Entity bulletEntity;
}