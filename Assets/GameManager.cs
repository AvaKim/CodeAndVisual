using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public int maxScore = 300;
    public int gameDifficulty = 1;
    public int contamination = 0;
    public string playerName = "undefined";

    public Image preGamePauseLayer;
    public TextMeshProUGUI scoreTextUGUI;
    public GameObject startGameButton;
    public Animator screenFadeOut;
    public void OnClickGoToMenuScene()
    {
        StartCoroutine(GoToMenuScene());
    }

    
    IEnumerator GoToMenuScene()
    {
        preGamePauseLayer.enabled = true;
        screenFadeOut.Play("TransitionOut");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(0);
    }

    public void OnClickStartGame()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        Debug.Log("Starting the game!");
        Time.timeScale = gameDifficulty switch
        {
            0 => 0.5f,
            1 => 1f,
            2 => 1.5f,
            _ => Time.timeScale
        };
        preGamePauseLayer.enabled = false;
        startGameButton.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(0.3f);
        Destroy(startGameButton);
    }
    // Start is called before the first frame update
    void Start()
    {
        playerName = GlobalGameManager.Instance == null ? "undefined" : GlobalGameManager.Instance.playerNickname;
        gameDifficulty = GlobalGameManager.Instance == null ? 1 : GlobalGameManager.Instance.gameDifficulty;
        Time.timeScale = 0; // Pause game until player starts
    }

    void OnDestroy()
    {
        
    }

    void CollectGarbage()
    {
        
    }

    void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUGUI();
    }

    void SubtractScore(int amount)
    {
        score -= amount;
        UpdateScoreUGUI();
    }

    void UpdateScoreUGUI()
    {
        scoreTextUGUI.text = score + " / " + maxScore;   
    }
    

    void GameOver()
    {
        
    }
    void Update()
    {
    }
}
