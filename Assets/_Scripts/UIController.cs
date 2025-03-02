using System;
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
    [SerializeField] private Slider sliderCPU;
    [SerializeField] private TextMeshProUGUI textCPU;
    [SerializeField] private TextMeshProUGUI textMessage;
    [SerializeField] private FlyCamera flyCamera;
    [SerializeField] private Toggle toggleMultiThreaded;
    [SerializeField] private SubScene subSceneDots;

    #endregion

    #region Private Variables

    private MonoBehaviourPrefabManager _spawnerMonoBehaviour;
    private SpawnEntitiesSystem _spawnerDots;
    private int _row = 0;
    private int _col = 0;
    #endregion

    #region Unity Functions

    private void Awake()
    {
        //Set all the Slider Texts
        SetRowText();
        SetColText();
        SetZPosText();
        SetCPUText();
        DisplayMessage();
    }


    private void Update()
    {
        HandleEscapeToTurnOffFlyCamera();
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

    public void SetCPUText()
    {
        textCPU.SetText("CPU Load: " + sliderCPU.value);
        Global.IterationCount = (int)sliderCPU.value;
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
        Global.IsMultiThreaded = toggleMultiThreaded.isOn;
    }

    public void OnButtonClick_Delete()
    {
        _row = 0;
        _col = 0;
        if (Global.IsDots == false)
        {
            if (_spawnerMonoBehaviour != null)
            {
                _spawnerMonoBehaviour.DeleteAllChildren();
            }
        }
        else
        {
            if (_spawnerDots != null)
            {
                _spawnerDots.DeleteAllCubes();
            }
        }
        DisplayMessage();
    }
    
    
    public void DisplayMessage()
    {
        int count = _row * _col;

        if (count == 0)
        {
            textMessage.SetText("Please Spawn Cubes");
            return;
        }
        
        string typeSpawn = Global.IsDots ? "DOTS" : "Monobehaviour";

        string output = $"Build for {typeSpawn} \n" +
                        $"Size: ({_row} , {_col}) = {count}\n" +
                        $"Press <b>Esc</b> to return UI Mode\n" +
                        $"Mouse Left Click to Fire\n" +
                        $"Mouse Right Click to Toggle modes\n";

        
        string multiThreaded = toggleMultiThreaded.isOn ?  "Using Multi thread\n" : "Using Single thread\n";
        
        textMessage.SetText(output + multiThreaded);
    }

    #endregion
    
    #region Private Functions

    private void SpawnObjects(bool isDots)
    {
        OnButtonClick_Delete();

        Global.IsDots = isDots;
        int row = (int)sliderRow.value;
        int col = (int)sliderCol.value;
        float zPos = sliderZPos.value;

        int outCount = row * col;

        subSceneDots.enabled = Global.IsDots;

        if (Global.IsDots == false)
        {
            //using regular Monobehaviour
            
            if (_spawnerMonoBehaviour == null)
                _spawnerMonoBehaviour = FindFirstObjectByType<MonoBehaviourPrefabManager>();

            _spawnerMonoBehaviour.SpawnGroup(row, col, 2.0f, zPos);
        }
        else
        {
            if (_spawnerDots == null)
                _spawnerDots = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SpawnEntitiesSystem>();

            _spawnerDots.SpawnGroup(row, col, 2.0f, zPos);
        }

        _row = row;
        _col = col;
        
        DisplayMessage();

        SetFlyCameraActive(true);
    }

    private void HandleEscapeToTurnOffFlyCamera()
    {
        if (flyCamera.enabled == true)
        {
            //added Right mouse button. still leave in Escape in case Mac mouse is still 1 button.
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                SetFlyCameraActive(false);
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                SetFlyCameraActive(true);
            }
        }
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