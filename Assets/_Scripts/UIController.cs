using System;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider sliderRow;
    [SerializeField] private TextMeshProUGUI textRow;

    [SerializeField] private Slider sliderCol;
    [SerializeField] private TextMeshProUGUI textCol;

    [SerializeField] private Slider sliderZPos;
    [SerializeField] private TextMeshProUGUI textZPos;


    [SerializeField] private TextMeshProUGUI textMessage;

    
    private MonoBehaviourPrefabManager _spawnerMonobehaviour;
    private SpawnCubesSystem _spawnerDOTs;
    
    private bool _isDots = false;

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
        
        if (_isDots == false)
        {
            if(_spawnerMonobehaviour == null)
                _spawnerMonobehaviour = Object.FindFirstObjectByType<MonoBehaviourPrefabManager>();

            _spawnerMonobehaviour.SpawnGroup(row, col, 2.0f, zPos, out outCount, out outTime);
        }
        else
        {
            if(_spawnerDOTs == null)
                _spawnerDOTs = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SpawnCubesSystem>();
            
            _spawnerDOTs.SpawnGroup(row, col, 2.0f, zPos, out outCount, out outTime);

        }
        
        outCount = row * col;

        string typeSpawn = _isDots ? "DOTs" : "Monobehaviour";
        
        string output = $"Build for {typeSpawn} \n" +
                        $"Size: ({row} , {col}) = {outCount}\n" +
                        $"Time: {outTime}ms\n" +
                        $"Press Esc to return mouse";

        textMessage.SetText(output);        
    }


    //public void OnButtonClick_MonoBehaviour_Destroy()
    
}
