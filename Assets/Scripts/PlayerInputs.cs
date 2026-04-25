using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PlayerInputs : MonoBehaviour
{
    public List<Button> gameButtons = new List<Button>();
    public List<TMP_Text> gameText = new List<TMP_Text>();
    //private int moveCounter = 0;
    //public int MoveCounter => moveCounter;

    private void Awake()
    {
    
        foreach (TMP_Text screenText in gameText)
        {
            screenText.text = "";
        }
    }

    private void Start()
    {
        foreach (Button btn in gameButtons)
        {
            btn.onClick.AddListener(() => InputXOnClick(btn));
        }
    }
    void InputXOnClick(Button clickedButton)
    {
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
    }

}
