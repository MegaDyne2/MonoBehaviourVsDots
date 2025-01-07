using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


public partial class SpawnEntitiesSystem : SystemBase
{
    #region Private Variables

    private int _rowCount = 100;
    private int _colCount = 100;
    private float _distanceZ = 100.0f;
    private float _spacing = 2f;
    private readonly List<Entity> _spawnedCubes = new();

    #endregion

    #region SystemBase Functions

    protected override void OnCreate()
    {
        RequireForUpdate<SpawnCubesConfig>();
        this.Enabled = false; //don't want it to run right way since we have nothing spawned yet.
    }

    protected override void OnUpdate()
    {
        SpawnGroupWork(_rowCount, _colCount, _spacing, _distanceZ);
        this.Enabled = false;
    }

    #endregion

    #region Public Functions

    //The Call to do the spawn.
    public void SpawnGroup(int inRow, int inCol, float inSpacing, float inZ)
    {
        _rowCount = inRow;
        _colCount = inCol;
        _distanceZ = inZ;
        _spacing = inSpacing;

        //when we call SpawnGroupWork here it would fail because SpawnCubesConfig is not ready yet.
        this.Enabled = true;
    }


    //This is to spawn the bullet Entity.
    //Unlike the SpawnCube this is most likely ready by the time user click on the mouse again.
    public void SpawnBullet(Vector3 position, Vector3 velocity)
    {
        SpawnBulletsConfig spawnCubesConfig = SystemAPI.GetSingleton<SpawnBulletsConfig>();
        Entity spawnedEntity = EntityManager.Instantiate(spawnCubesConfig.bulletEntity);

        LocalTransform localTransform = SystemAPI.GetComponent<LocalTransform>(spawnedEntity);
        localTransform.Position = position;
        SystemAPI.SetComponent(spawnedEntity, localTransform);

        var movingComponent = SystemAPI.GetComponent<MovingComponent>(spawnedEntity);
        movingComponent.velocity = velocity;
        SystemAPI.SetComponent(spawnedEntity, movingComponent);
    }

    public void DeleteAllCubes()
    {
        foreach (var entity in _spawnedCubes)
        {
            if (EntityManager.Exists(entity))
            {
                EntityManager.DestroyEntity(entity);
            }
        }

        // Clear the list after deletion
        _spawnedCubes.Clear();
    }

    #endregion

    #region Private Functions

    //We have to call the actual spawn code in Update because it can't find SpawnCubesConfig in time.
    private void SpawnGroupWork(int inRow, int inCol, float inSpacing, float inZ)
    {
        //Get the data
        SpawnCubesConfig spawnCubesConfig = SystemAPI.GetSingleton<SpawnCubesConfig>();

        //starting positions
        float startHeight = -(inRow * inSpacing * 0.5f);
        float startWidth = -(inCol * inSpacing * 0.5f);

        for (int i = 0; i < inRow; i++)
        {
            for (int j = 0; j < inCol; j++)
            {
                //spawn it
                Entity spawnedEntity = EntityManager.Instantiate(spawnCubesConfig.cubePrefabEntity);

                //set position
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

                //add it to list so that we can delete it later
                _spawnedCubes.Add(spawnedEntity);
            }
        }
    }

    #endregion
}