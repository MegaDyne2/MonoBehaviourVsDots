using System.Collections.Generic;
using System.Diagnostics;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Debug = UnityEngine.Debug;


public partial class SpawnCubesSystem : SystemBase
{
    public int rowCount = 100;
    public int colCount = 100;
    public float distanceZ = 100.0f;
    
    private float spacing = 2f;
    public int prefabCount = 50; // Number of boids to spawn

    private List<Entity> entities = new List<Entity>();
    
    protected override void OnCreate()
    {
        RequireForUpdate<SpawnCubesConfig>();
    }

    protected override void OnUpdate()
    {
        this.Enabled = false;

        //SpawnGroup(rowCount, colCount, spacing, distanceZ, out var spawnCount, out var instantiateTime);
        
    }

    public void SpawnGroup(int inRow, int inCol, float inSpacing, float inZ, out int outCount, out long instantiateTime)
    {
        SpawnCubesConfig spawnCubesConfig = SystemAPI.GetSingleton<SpawnCubesConfig>();

        float startHeight = - (inRow * inSpacing * 0.5f);  
        float startWidth = - (inCol * inSpacing * 0.5f);

        prefabCount = inRow * inCol;
        
        // Measure delete time
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < inRow; i++)
        {
            
            for (int j = 0; j < inCol; j++)
            {
                Entity spawnedEntity = EntityManager.Instantiate(spawnCubesConfig.cubePrefabEntity);
                //EntityManager.SetComponentData(spawnedEntity, new LocalTransform
                SystemAPI.SetComponent(spawnedEntity, new LocalTransform
                {
                    Position = new float3(
                        startWidth + (inSpacing * j), 
                        startHeight + (inSpacing * i), 
                        inZ
                    ),
                    //these needs to be set or it will be at zero
                    Rotation = quaternion.identity,
                    Scale = 1f
                });

                entities.Add(spawnedEntity);
            }
        }
        
        // Calculate elapsed time in milliseconds with high precision
        double milliseconds = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000.0;

        // Log the precise time
        Debug.Log($"Initialized {prefabCount} prefabs in {milliseconds:0.000000} ms");
        
        instantiateTime = (long)milliseconds;
        outCount = prefabCount;
        

        outCount = 0;
        instantiateTime = 0;
    }
    
    
    public void DeleteAllEntities()
    {
        foreach (var entity in entities)
        {
            if (EntityManager.Exists(entity))
            {
                EntityManager.DestroyEntity(entity);
            }
        }
        
        // Clear the list after deletion
        entities.Clear();
    }
}
