using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MonoBehaviourPrefabManager : MonoBehaviour
{
    public GameObject prefabMono; // Prefab for boids

    public int rowCount = 10;
    public int colCount = 10;
    public float distanceZ = 20.0f;

    public float spacing = 1.5f;
    
    
    public int prefabCount = 50; // Number of boids to spawn

    void Start()
    {
        // for (int i = 0; i < boidCount; i++)
        // {
        //     Vector3 spawnPosition = new Vector3(
        //         Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
        //         Random.Range(-spawnArea.y / 2, spawnArea.y / 2),
        //         Random.Range(-spawnArea.z / 2, spawnArea.z / 2)
        //     );
        //
        //     //Instantiate(boidPrefab, spawnPosition, Quaternion.identity);
        // }

        SpawnGroup(rowCount, colCount, spacing, distanceZ, out var spawnCount, out var instantiateTime);

        DeleteAllChildren();
    }
    
    public void SpawnGroup(int inRow, int inCol, float inSpacing, float inZ, out int outCount, out long instantiateTime)
    {
        
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        this.transform.SetPositionAndRotation(new Vector3(0.0f, 0.0f, inZ), Quaternion.identity);

        float startWidth = - (inRow * inSpacing * 0.5f);  
        float startHeight = - (inCol * inSpacing * 0.5f);

        prefabCount = inRow * inCol;
        
        // Measure delete time
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < inRow; i++)
        {
            for (int j = 0; j < inCol; j++)
            {
                GameObject go = Instantiate(prefabMono, transform);
                go.name = i + "," + j;
                go.transform.SetLocalPositionAndRotation(
                    new Vector3(startWidth + (inSpacing * i), startHeight + (inSpacing * j), 0.0f), 
                    Quaternion.identity);
            }
        }
        
        // Calculate elapsed time in milliseconds with high precision
        double milliseconds = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000.0;

        // Log the precise time
        Debug.Log($"Initialized {prefabCount} prefabs in {milliseconds:0.000000} ms");
        
        instantiateTime = (long)milliseconds;
        outCount = prefabCount;
    }


    public float DeleteAllChildren()
    {
        // Measure delete time
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        stopwatch.Stop();
        // Calculate elapsed time in milliseconds with high precision
        double milliseconds = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000.0;

        // Log the precise time
        Debug.Log($"DeleteAllChildren {prefabCount} prefabs in {milliseconds:0.000000} ms");

        return (long)milliseconds;
    }
    
}