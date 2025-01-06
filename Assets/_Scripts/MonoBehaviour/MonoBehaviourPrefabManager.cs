using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MonoBehaviourPrefabManager : MonoBehaviour
{
    public GameObject prefabMono; // Prefab for boids

    public int rowCount = 100;
    public int colCount = 100;
    public float distanceZ = 100.0f;
    
    private float _spacing = 2f;


    
    public int prefabCount = 50; // Number of boids to spawn

    private List<Transform> saveObjects = new ();
    private List<MonoBehaviourPrefabController> monoBehaviourControllers = new ();



    private void Update()
    {
        foreach (var controller in monoBehaviourControllers)
        {
            //Debug.unityLogger.Log($"Normal: {controller.speed} | {Time.deltaTime} | {controller.speed * Time.deltaTime}"); 
            if (controller == null)
                continue;
            
            controller.transform.Rotate(Vector3.up, controller.speed * Time.deltaTime);
        }
    }

    public void SpawnGroup(int inRow, int inCol, float inSpacing, float inZ, out int outCount, out long instantiateTime)
    {

        
        this.transform.SetPositionAndRotation(new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);

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
                GameObject go = Instantiate(prefabMono, null);
                go.name = i + "," + j;
                go.transform.SetLocalPositionAndRotation(
                    new Vector3(startWidth + (inSpacing * j), startHeight + (inSpacing * i), inZ), 
                    Quaternion.identity);
                
                saveObjects.Add(go.transform);
                monoBehaviourControllers.Add(go.GetComponent<MonoBehaviourPrefabController>());
            }
        }
        
        // Calculate elapsed time in milliseconds with high precision
        double milliseconds = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000.0;

        // Log the precise time
        Debug.Log($"Initialized {prefabCount} prefabs in {milliseconds:0.000000} ms");
        
        instantiateTime = (long)milliseconds;
        outCount = prefabCount;
    }


    public void DeleteAllChildren()
    {
        foreach(Transform child in saveObjects)
        {
            if (child == null)
                continue;
            
            Destroy(child.gameObject);
        }
        saveObjects.Clear();
        monoBehaviourControllers.Clear();
    }
    
}