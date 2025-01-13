using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MonoBehaviourPrefabManager : MonoBehaviour
{
    
    #region Prefabs

    public GameObject prefabMono;
    
    #endregion
    
    #region Links

    [SerializeField] Toggle toggleMultithreaded;

    #endregion
    
    #region Private Varibles

    //Keeping track of the saved Object and it's scripts
    private readonly List<MonoBehaviourPrefabCubeController> _monoBehaviourControllers = new();
    
    private NativeArray<quaternion> _rotations;
    private NativeArray<float> _rotationSpeeds;
    
    #endregion
    
    #region Unity Functions

    private void OnDestroy()
    {
        Jobs_DeleteNativeArray();
    }

    private void Update()
    {
        if (Global.IsDots == true)
        {
            return;
        }

        if (toggleMultithreaded.isOn)
        {
            Jobs_Update();
            return;
        }

        SingleThread_Update();
    }

    private void SingleThread_Update()
    {
        var startTime = System.Diagnostics.Stopwatch.StartNew();

        foreach (var currentCube in _monoBehaviourControllers)
        {
            if (currentCube == null)
                continue;
            
            currentCube.transform.rotation  = Global.CalculateNewRotation(currentCube.rotationSpeed, currentCube.transform.rotation,Time.deltaTime, Global.IterationCount);
        }
        startTime.Stop();
        Global.ElapsedRotationMS = startTime.Elapsed.TotalMilliseconds;
    }

    #endregion

    #region Public Functions

    //Do the Spawn
    public void SpawnGroup(int inRow, int inCol, float inSpacing, float inZ)
    {
        this.transform.SetPositionAndRotation(new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);

        float startHeight = -(inRow * inSpacing * 0.5f);
        float startWidth = -(inCol * inSpacing * 0.5f);

        for (int i = 0; i < inRow; i++)
        {
            for (int j = 0; j < inCol; j++)
            {
                GameObject go = Instantiate(prefabMono, null);

                go.transform.SetLocalPositionAndRotation(
                    new Vector3(startWidth + (inSpacing * j), startHeight + (inSpacing * i), inZ),
                    Quaternion.identity);
                
                _monoBehaviourControllers.Add(go.GetComponent<MonoBehaviourPrefabCubeController>());
            }
        }
        
        // Resize NativeArrays after spawning
        int count = _monoBehaviourControllers.Count;
        
 
        Jobs_CreateNativeArray(count);
    }

    public void DeleteAllChildren()
    {
        foreach (var transformSavedObject in _monoBehaviourControllers)
        {
            if (transformSavedObject == null)
                continue;

            Destroy(transformSavedObject.gameObject);
        }
        
        _monoBehaviourControllers.Clear();

        //delete all bullets as well
        MonoBehaviourBullet.DeleteAllBullets();
        
        Jobs_DeleteNativeArray();
    }

    #endregion
    
    
    #region Jobs
    private void Jobs_Update()
    {
        var startTime = System.Diagnostics.Stopwatch.StartNew();

        // Step 1: Populate NativeArrays
        for (int i = 0; i < _monoBehaviourControllers.Count; i++)
        {
            if (_monoBehaviourControllers[i] == null)
                continue;

            _rotations[i] = _monoBehaviourControllers[i].transform.rotation;
            _rotationSpeeds[i] = _monoBehaviourControllers[i].rotationSpeed;
        }
        
        
        // Step 2: Schedule the rotation job
        var rotationJob = new RotateCubesJob
        {
            IterationCount = Global.IterationCount,
            DeltaTime = Time.deltaTime,
            Rotations = _rotations,
            RotationSpeeds = _rotationSpeeds
        };

        JobHandle jobHandle = rotationJob.Schedule(_monoBehaviourControllers.Count, 64);
        jobHandle.Complete();
        
        // Step 3: Apply results back to MonoBehaviour objects
        for (int i = 0; i < _monoBehaviourControllers.Count; i++)
        {
            if (_monoBehaviourControllers[i] == null)
                continue;

            _monoBehaviourControllers[i].transform.rotation = _rotations[i];
        }
        
        startTime.Stop();
        Global.ElapsedRotationMS = startTime.Elapsed.TotalMilliseconds;
    }


    
    [BurstCompile]
    private struct RotateCubesJob : IJobParallelFor
    {
        public int IterationCount;
        public float DeltaTime;
        public NativeArray<quaternion> Rotations;
        [ReadOnly] public NativeArray<float> RotationSpeeds;

        public void Execute(int index)
        {
            // Rotate around the Y-axis
            Rotations[index] = Global.CalculateNewRotation(RotationSpeeds[index], Rotations[index], DeltaTime, IterationCount);
        }

      
    }

    
    private void Jobs_CreateNativeArray(int count)
    {
        _rotations = new NativeArray<quaternion>(count, Allocator.Persistent);
        _rotationSpeeds = new NativeArray<float>(count, Allocator.Persistent);
    }

    private void Jobs_DeleteNativeArray()
    {
        if (_rotations.IsCreated)
        {
            _rotations.Dispose();
        }

        if (_rotationSpeeds.IsCreated)
        {
            _rotationSpeeds.Dispose();
        }
    }

    #endregion
}