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
    public float contamination = 0;
    public string playerName = "undefined";

    public Slider contaminationUISlider;
    public Image preGamePauseLayer;
    public TextMeshProUGUI scoreTextUGUI;
    public GameObject startGameButton;
    public Animator screenFadeOut;

    public static GameManager Instance;
    
    public void OnClickGoToMenuScene()
    {
        StartCoroutine(LoadScene(0));
    }

    
    IEnumerator LoadScene(int sceneIndex)
    {
        Debug.Log("Moving to scene " + sceneIndex + "...");
        preGamePauseLayer.enabled = true;
        float preFadeoutDelayAmount = 0f;
        switch (gameDifficulty)
        {
            case 0:
                preFadeoutDelayAmount = 2f;
                break;
            case 1:
                preFadeoutDelayAmount = 1.5f;
                break;
            case 2:
                preFadeoutDelayAmount = 1.5f;
                break;
        }

        yield return new WaitForSeconds(preFadeoutDelayAmount);
        screenFadeOut.Play("TransitionOut");
        yield return new WaitForSeconds(1.5F); // fadeout animation length

        if (GlobalGameManager.Instance == null)
        {
            Debug.Log("No GlobalManager found. Reloading without saving data");
        }
        else
        {
            GlobalGameManager.Instance.playerScore = score;
            GlobalGameManager.Instance.contamination = contamination;   
        }

        SceneManager.LoadScene(sceneIndex);
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
        if(Instance != null && Instance != this) 
        {
            Debug.LogWarning("Another GameManager instance detected");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        
        playerName = GlobalGameManager.Instance == null ? "undefined" : GlobalGameManager.Instance.playerNickname;
        gameDifficulty = GlobalGameManager.Instance == null ? 1 : GlobalGameManager.Instance.gameDifficulty;
        contamination = GlobalGameManager.Instance == null ? 0f : GlobalGameManager.Instance.contamination;
        score = GlobalGameManager.Instance == null ? 0 : GlobalGameManager.Instance.playerScore;

        UpdatePlayerHUD();
        PauseGame(); // Pause game until player starts.

    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ContaminateByAmount(float amount)
    {
        contamination += amount;

        if (contamination > 1f) contamination = 1f;
        contaminationUISlider.value = contamination;

        if (contamination >= 1f)
        {
            GameOver();
        }
    }


    public void AddScore(int amount)
    {
        score += amount;

        if (score >= maxScore)
        {
            score = maxScore;
            Debug.Log("Added score "+ amount + ". Reached max score!");
        }
        Debug.Log("Added score "+ amount + ". Current: " + score);
        UpdateScoreUGUI();
    }

    public void SubtractScore(int amount)
    {
        score -= amount;
        if (score < 0) score = 0;

        Debug.Log("Lost score "+ amount + ". Current: " + score);

        UpdateScoreUGUI();
        
    }

    public void ReloadScene()
    {
        StartCoroutine(LoadScene(1));
    }

    void UpdatePlayerHUD()
    {
        UpdateScoreUGUI();
        UpdateContaminationSliderUGUI();
    }
    void UpdateScoreUGUI()
    {
        scoreTextUGUI.text = score + " / " + maxScore;
    }

    void UpdateContaminationSliderUGUI()
    {
        contaminationUISlider.value = contamination;
    }
    

    void GameOver()
    {
        Debug.Log("Gameover");
    }
    void Update()
    {
    }
}
