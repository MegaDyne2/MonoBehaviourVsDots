using System;
using System.Collections;
using TMPro;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UIController : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    
    [SerializeField] private Slider sliderRow;
    [SerializeField] private TextMeshProUGUI textRow;

    [SerializeField] private Slider sliderCol;
    [SerializeField] private TextMeshProUGUI textCol;

    [SerializeField] private Slider sliderZPos;
    [SerializeField] private TextMeshProUGUI textZPos;


    [SerializeField] private TextMeshProUGUI textMessage;

    [SerializeField] private FlyCamera flyCamera;

    [SerializeField] private Toggle toggleMultiThreaded;

    [SerializeField] private SubScene subSceneDots;
    
    private MonoBehaviourPrefabManager _spawnerMonobehaviour;
    private SpawnCubesSystem _spawnerDOTs;
 
    
    private bool _isDots = false;

    public bool IsDots()
    {
        return _isDots;
    }
    private void Awake()
    {
        SetRowText();
        SetColText();
        SetZPosText();
    }

    // Row and Column need to be swap
    public void SetRowText()
    {
        textRow.SetText("Row: " + sliderRow.value);
    }

    public void SetColText()
    {
        textCol.SetText("Col: " + sliderCol.value);
    }

    public void SetZPosText()
    {
        textZPos.SetText("Z pos: " + sliderZPos.value);
    }
    

    public void OnButtonClick_MonoBehaviour_Spawn()
    {
        SpawnObjects(false);
    }

    public void OnButtonClick_Dots_Spawn()
    {
        SpawnObjects(true);
    }

    public void OnToggle_DOTs_Multithread()
    {
        Debug.Log("OnToggle_DOTs_Multithread: " + toggleMultiThreaded.isOn);
        var world = World.DefaultGameObjectInjectionWorld;
        var entityManager = world.EntityManager;

        // Query the singleton entity manually
        EntityQuery singletonQuery = entityManager.CreateEntityQuery(ComponentType.ReadWrite<SpawnCubesConfig>());

        if (singletonQuery.CalculateEntityCount() > 0)
        {
            // Get the singleton entity
            Entity singletonEntity = singletonQuery.GetSingletonEntity();

            // Modify the useMultiThreading field
            var spawnConfig = entityManager.GetComponentData<SpawnCubesConfig>(singletonEntity);
            spawnConfig.useMultiThreading = toggleMultiThreaded.isOn; // Use the bool from the UI
            entityManager.SetComponentData(singletonEntity, spawnConfig);

            Debug.Log($"UseMultiThreading: {spawnConfig.useMultiThreading}");
        }
        else
        {
            Debug.LogError("SpawnCubesConfig singleton entity not found!");
        }

        // Clean up the query
        singletonQuery.Dispose();
        
    }
    public void OnButtonClick_Delete()
    {
        if (_isDots == false)
        {
            if (_spawnerMonobehaviour != null)
            {
                _spawnerMonobehaviour.DeleteAllChildren();
            }
        }
        else
        {
            if (_spawnerDOTs != null)
            {
                _spawnerDOTs.DeleteAllEntities();
            }
        }
    }
    
    private void SpawnObjects(bool isDots)
    {
        OnButtonClick_Delete();
        
        _isDots = isDots;
        int row = (int)sliderRow.value;
        int col = (int)sliderCol.value;
        float zPos = sliderZPos.value;

        int outCount = 0;
        long outTime = 0;

        subSceneDots.enabled = _isDots;
        
        if (_isDots == false)
        {
            
            if(_spawnerMonobehaviour == null)
                _spawnerMonobehaviour = Object.FindFirstObjectByType<MonoBehaviourPrefabManager>();

            _spawnerMonobehaviour.SpawnGroup(row, col, 2.0f, zPos, out outCount, out outTime);
        }
        else
        {

            StartCoroutine(WaitForSubScene(row, col, zPos));


        }
        
        outCount = row * col;

        string typeSpawn = _isDots ? "DOTs" : "Monobehaviour";
        
        string output = $"Build for {typeSpawn} \n" +
                        $"Size: ({row} , {col}) = {outCount}\n" +
                        $"Time: {outTime}ms\n" +
                        $"Press Esc to return mouse";

        textMessage.SetText(output);

        SetFlyCameraActive(true);
    }
    

    public void SetFlyCameraActive(bool active)
    {
        flyCamera.enabled = active;
        canvasGroup.enabled = !active;
        
        Cursor.lockState = active?CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !active;
    }
    //public void OnButtonClick_MonoBehaviour_Destroy()
    IEnumerator WaitForSubScene(int row, int col, float zPos)
    {
        
        while (!subSceneDots.IsLoaded)
        {
            yield return null; // Wait for the next frame
        }
        

        if(_spawnerDOTs == null)
            _spawnerDOTs = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SpawnCubesSystem>();
            
        Debug.Log(_spawnerDOTs);
        _spawnerDOTs.SpawnGroup(row, col, 2.0f, zPos, out int outCount, out long outTime);
    }

    public void FireBullet(Vector3 position, Vector3 velocity)
    {
        if (subSceneDots.IsLoaded == false)
        {
            return;
        }
        
        if(_spawnerDOTs == null)
            _spawnerDOTs = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SpawnCubesSystem>();
            
        Debug.Log(_spawnerDOTs);
        _spawnerDOTs.SpawnBullet(position, velocity);
    }

}
