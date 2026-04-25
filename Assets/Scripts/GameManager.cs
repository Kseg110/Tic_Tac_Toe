using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private PlayerInputs playerInputs;

    private int[] board = new int[9];

    private void Awake()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0f;
        // Initialize game board
        for (int i = 0; i < board.Length; i++)
            board[i] = 0;
    }

    void PlayClicked()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void QuitClicked()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    void Start()
    {
        startButton.onClick.AddListener(PlayClicked);
        resetButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        quitButton.onClick.AddListener(QuitClicked);
    }

    void MakeAIMove()
    {
        int aiMove = MinMaxAI.ComputerMove(board);
        if (aiMove != -1)
        {
            board[aiMove] = 1;
            // Update board
            Button aiButton = playerInputs.gameButtons[aiMove];
            TMP_Text tmpText = aiButton.GetComponentInChildren<TMP_Text>();
            if (tmpText != null)
            {
                tmpText.text = "O";
            }
            else
            {
                Text uiText = aiButton.GetComponentInChildren<Text>();
                if (uiText != null)
                {
                    uiText.text = "O";
                }
            }
            aiButton.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
