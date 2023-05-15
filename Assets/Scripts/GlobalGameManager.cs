using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    public string playerNickname = "undefined";
    public int gameDifficulty = 1; // 0 = slow, 1 = medium, 2 = fast
    public int playerScore = 0;
    public float contamination = 0;

    public static GlobalGameManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance != null & Instance != this)
        {
            Debug.Log("Only one GlobalManager should exist at a time. Destroying this instance.");
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
