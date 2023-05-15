using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// game over straight way
// drag too long at normal speed game end
// stop game and show result panel upon reaching level end instead of loadscene again

public class ResultsPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerText; // GAME OVER [P]!
    [SerializeField] private TextMeshProUGUI scoreText; // 0 - 300
    [SerializeField] private TextMeshProUGUI contaminationPercentText; // 0 - 100%
    [SerializeField] private TextMeshProUGUI finalScoreText; // 0 - 200
    
    private void OnEnable()
    {
        headerText.text = $"GAME OVER {GameManager.Instance.playerName}!";
        scoreText.text = $"{GameManager.Instance.score}";
        contaminationPercentText.text = $"{(int)(GameManager.Instance.contamination * 100f)}";

        float contamMultiplier = 1f - GameManager.Instance.contamination;
        finalScoreText.text = $"{(int)(GameManager.Instance.score * contamMultiplier)}";
    }
}
