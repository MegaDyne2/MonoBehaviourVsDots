using System.Collections;
using TMPro;
using UnityEngine;

public class RotationCalculationFPS : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText; // Reference to a UI Text element to display the FPS

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowMessage();
        //Have it update the text every 1 seconds
        StartCoroutine(StartCoroutineUpdatePerSecond());
    }

    private IEnumerator StartCoroutineUpdatePerSecond()
    {
        for (;;)
        {
            yield return new WaitForSeconds(1.0f);
            ShowMessage();
        }
    }

    private void ShowMessage()
    {
        double elapsedMs = Global.ElapsedRotationMS;
        double fps = elapsedMs > 0.001 ? 1000f / elapsedMs : 0;
        fpsText.text = $"<u>Rotation Calculation</u>\nFrameTime: <u>{elapsedMs.ToString("0.00")}ms</u>\nFPS: <u>{(fps).ToString("0.00")}</u>";  
    }
}
