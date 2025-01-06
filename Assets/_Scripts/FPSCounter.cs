using System.Collections;
using TMPro;
using UnityEngine;


/// <summary>
/// Show's the FPS on screen
/// </summary>
public class FPSCounter : MonoBehaviour
{
    #region Links and Private Variable

    [SerializeField] private TextMeshProUGUI fpsText; // Reference to a UI Text element to display the FPS
    private float deltaTime = 0.0f;
    private float fps = 0.0f;

    #endregion

    #region Unity Functions

    void Start()
    {
        //Have it update the text every 1 seconds
        StartCoroutine(StartCoroutineUpdatePerSecond());
    }

    void Update()
    {
        // Calculate frame time
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Calculate FPS
        fps = 1.0f / deltaTime;
    }

    #endregion

    #region Private Functions

    private IEnumerator StartCoroutineUpdatePerSecond()
    {
        for (;;)
        {
            yield return new WaitForSeconds(1.0f);
            fpsText.text = $"FPS: {Mathf.Ceil(fps)}";
        }
    }

    #endregion
}