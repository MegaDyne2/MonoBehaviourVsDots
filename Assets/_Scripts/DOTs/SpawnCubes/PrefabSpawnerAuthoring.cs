using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

public class PrefabSpawnerAuthoring : MonoBehaviour
{
    [FormerlySerializedAs("cubePrefab")] public GameObject prefabMonoBehaviour;
    [FormerlySerializedAs("dotsBullets")] public GameObject prefabBulletDOTS;

    public bool useMultithreading = false;


    public class Baker : Baker<PrefabSpawnerAuthoring>
    {
        public override void Bake(PrefabSpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new SpawnCubesConfig
            {
                cubePrefabEntity = GetEntity(authoring.prefabMonoBehaviour, TransformUsageFlags.Dynamic),
                useMultiThreading = authoring.useMultithreading // Set the field
            });

            AddComponent(entity, new SpawnBulletsConfig
            {
                bulletEntity = GetEntity(authoring.prefabBulletDOTS, TransformUsageFlags.Dynamic),
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