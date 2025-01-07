using System.Collections;
using TMPro;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This allows actions for the UI menu
/// </summary>
public class UIController : MonoBehaviour
{
    #region Links

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

    #endregion

    #region Private Variables

    private MonoBehaviourPrefabManager _spawnerMonobehaviour;
    private SpawnEntitiesSystem _spawnerDOTS;
    private bool _isDOTS = false;

    #endregion

    #region Unity Functions

    private void Awake()
    {
        //Set all the Slider Texts
        SetRowText();
        SetColText();
        SetZPosText();
    }

    #endregion
    
    #region Accessors

    public bool IsDots()
    {
        return _isDOTS;
    }

    #endregion
    
    #region Callbacks

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
        if (_isDOTS == false)
        {
            if (_spawnerMonobehaviour != null)
            {
                _spawnerMonobehaviour.DeleteAllChildren();
            }
        }
        else
        {
            if (_spawnerDOTS != null)
            {
                _spawnerDOTS.DeleteAllCubes();
            }
        }
    }

    #endregion
    
    #region Private Functions

    private void SpawnObjects(bool isDots)
    {
        OnButtonClick_Delete();

        _isDOTS = isDots;
        int row = (int)sliderRow.value;
        int col = (int)sliderCol.value;
        float zPos = sliderZPos.value;

        int outCount = row * col;

        subSceneDots.enabled = _isDOTS;

        if (_isDOTS == false)
        {
            if (_spawnerMonobehaviour == null)
                _spawnerMonobehaviour = FindFirstObjectByType<MonoBehaviourPrefabManager>();

            _spawnerMonobehaviour.SpawnGroup(row, col, 2.0f, zPos);
        }
        else
        {
            StartCoroutine(WaitForSubScene(row, col, zPos));
        }


        string typeSpawn = _isDOTS ? "DOTs" : "Monobehaviour";

        string output = $"Build for {typeSpawn} \n" +
                        $"Size: ({row} , {col}) = {outCount}\n" +
                        $"Mouse Click to Fire\n" +
                        $"Press <b>Esc</b> to return mouse";

        textMessage.SetText(output);

        SetFlyCameraActive(true);
    }

    private IEnumerator WaitForSubScene(int row, int col, float zPos)
    {
        while (!subSceneDots.IsLoaded)
        {
            yield return null; // Wait for the next frame
        }
        
        if (_spawnerDOTS == null)
            _spawnerDOTS = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SpawnEntitiesSystem>();

        _spawnerDOTS.SpawnGroup(row, col, 2.0f, zPos);
    }

    #endregion

    #region Public Functions

    public void SetFlyCameraActive(bool active)
    {
        flyCamera.enabled = active;
        canvasGroup.enabled = !active;

        Cursor.lockState = active ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !active;
    }

    #endregion
}