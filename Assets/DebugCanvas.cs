using UnityEngine;
using TMPro;

public class DebugCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;
    private float fps;

    private void Update()
    {
        // Calculate FPS
        fps = 1f / Time.deltaTime;
    
        // Set FPS text
        fpsText.text = "FPS: " + Mathf.RoundToInt(fps).ToString();
    }
    
}