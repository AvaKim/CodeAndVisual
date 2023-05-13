using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGameButton : MonoBehaviour
{
    public TMP_InputField nickNameInputField;
    public TMP_Dropdown ageDropDown;
    public TextMeshProUGUI fillPleaseTextUI;
    public Animator sceneTransitioner;
        
    public void TryPlayGame()
    {
        if (nickNameInputField.text.Length < 1 || string.IsNullOrEmpty(ageDropDown.captionText.text))
        {
            StartCoroutine(AlertPlayerToFill());
        }
        else
        {
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        sceneTransitioner.Play("TransitionOut");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }

    IEnumerator AlertPlayerToFill()
    {
        //TODO: Reject sound and animation
        fillPleaseTextUI.enabled = true;
        yield return new WaitForSeconds(1.8f);
        fillPleaseTextUI.enabled = false;
    }

    public void OnAgeDropdownUpdated(Int32 index)
    {
        GlobalGameManager.Instance.gameDifficulty = index;
    }

    public void OnNicknameFieldUpdated(string input)
    {
        GlobalGameManager.Instance.playerNickname = input;
    }
}
