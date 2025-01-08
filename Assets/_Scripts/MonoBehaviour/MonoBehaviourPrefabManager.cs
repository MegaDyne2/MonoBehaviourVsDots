using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class MonoBehaviourPrefabManager : MonoBehaviour
{
    #region Prefabs

    public GameObject prefabMono;

    
    
    #endregion

    #region Private Varibles

    //Keeping track of the saved Object and it's scripts
    private readonly List<Transform> _saveObjects = new();
    private readonly List<MonoBehaviourPrefabCubeController> _monoBehaviourControllers = new();

    private NativeArray<float3> _positions;
    private NativeArray<quaternion> _rotations;
    private NativeArray<float> _rotationSpeeds;
    
    #endregion
    
    #region Unity Functions

    private void Update()
    {
        foreach (var controller in _monoBehaviourControllers)
        {
            if (controller == null)
                continue;

            controller.transform.Rotate(Vector3.up, controller.rotationSpeed * Time.deltaTime);
        }
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

                _saveObjects.Add(go.transform);
                _monoBehaviourControllers.Add(go.GetComponent<MonoBehaviourPrefabCubeController>());
            }
        }
        
        // Resize NativeArrays after spawning
        int count = _monoBehaviourControllers.Count;
        
        if (_positions.IsCreated) _positions.Dispose();
        if (_rotations.IsCreated) _rotations.Dispose();
        if (_rotationSpeeds.IsCreated) _rotationSpeeds.Dispose();

        _positions = new NativeArray<float3>(count, Allocator.Persistent);
        _rotations = new NativeArray<quaternion>(count, Allocator.Persistent);
        _rotationSpeeds = new NativeArray<float>(count, Allocator.Persistent);
    }

    public void DeleteAllChildren()
    {
        foreach (Transform transformSavedObject in _saveObjects)
        {
            if (transformSavedObject == null)
                continue;

            Destroy(transformSavedObject.gameObject);
        }

        _saveObjects.Clear();
        _monoBehaviourControllers.Clear();
    }

    #endregion
}