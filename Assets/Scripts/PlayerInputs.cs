using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class PlayerInputs : MonoBehaviour
{
    public List<Button> gameButtons;
    public List<TMP_Text> gameText;

    public event Action<int> OnPlayerMove;

    private void Awake()
    {
    
        foreach (TMP_Text screenText in gameText)
        {
            screenText.text = "";
        }
    }

    private void Start()
    {
        for (int i = 0; i < gameButtons.Count; i++)
        {
            int index = i;
            gameButtons[i].onClick.AddListener(() => InputXOnClick(index));
        }
    }
    public void InputXOnClick(int index)
    {
        Button clickedButton = gameButtons[index];
        TMP_Text tmpText = clickedButton.GetComponentInChildren<TMP_Text>();
        if (tmpText != null)
        {
            tmpText.text = "X";
        }
        else
        {
            Text uiText = clickedButton.GetComponentInChildren<Text>();
            if (uiText != null)
            {
                uiText.text = "X";
            }
        }
        clickedButton.interactable = false; // disables button after interacted 
        
        Debug.Log("Player Placed an X");
        OnPlayerMove?.Invoke(index);
    }

}
