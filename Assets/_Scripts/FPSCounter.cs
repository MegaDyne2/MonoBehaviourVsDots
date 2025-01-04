using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText; // Reference to a UI Text element to display the FPS
    private float deltaTime = 0.0f;
    private float fps = 0.0f;

    void Start()
    {
        StartCoroutine(StartCoroutineUpdatePerSecond());
    }

    private IEnumerator StartCoroutineUpdatePerSecond()
    {
        for (;;)
        {
            yield return new WaitForSeconds(1.0f);
            fpsText.text = $"FPS: {Mathf.Ceil(fps)}";

        }
    }

    void Update()
    {
        // Calculate frame time
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Calculate FPS
        fps = 1.0f / deltaTime;
        
    }
}