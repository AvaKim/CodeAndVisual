using UnityEngine;
using TMPro;

public class DebugCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;
    private float fps = 60f;

    void Start()
    {
        InvokeRepeating("GetFPS", 1, 1);
    }
    private void GetFPS()
    {
        // Calculate FPS
        fps = (int)1.0f / Time.unscaledDeltaTime;
        
        fpsText.text = "FPS: " + fps;
        
    }
    
}