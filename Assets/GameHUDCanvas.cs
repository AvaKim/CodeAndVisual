using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameHUDCanvas : MonoBehaviour
{
    private float gameTime = 0f;
    public TextMeshProUGUI timerText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
        
        float minutes = Mathf.FloorToInt(gameTime / 60);  
        float seconds = Mathf.FloorToInt(gameTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
