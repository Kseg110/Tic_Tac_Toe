using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    [Header("Panels + Buttons")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button[] quitButtons;
    [SerializeField] private Button replayButton;

    [Header("UI Text Elements")]
    [SerializeField] private TMP_Text winsText;
    [SerializeField] private TMP_Text lossText;
    [SerializeField] private TMP_Text drawText;


    [Header("Game Ref")]
    [SerializeField] private PlayerInputs playerInputs;

    

    private readonly int[] board = new int[9];
    private bool isPlayersTurn = true;
    private bool gameActive = true;

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
        isPlayersTurn = true;
        gameActive = true;
        EnableAllButtons(true);
    }

    void ReplayClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        menuPanel.SetActive(false);
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
        foreach (var btn in quitButtons)
        {
            btn.onClick.AddListener(QuitClicked);
        }
        replayButton.onClick.AddListener(ReplayClicked);
        playerInputs.OnPlayerMove += OnPlayerMove;

        winsText.text = $"Player: {ScoreKeeper.WinCount}";
        lossText.text = $"AI: {ScoreKeeper.LossCount}";
        drawText.text = $"Draw: {ScoreKeeper.DrawCount}";
    }

    private void OnDestroy()
    {
        playerInputs.OnPlayerMove -= OnPlayerMove;
    }

    private void OnPlayerMove(int index)
    {
        if (!gameActive || !isPlayersTurn || board[index] != 0)
            return;
        board[index] = -1; // Player
        CheckGameState();

        if (gameActive)
        {
            isPlayersTurn = false;
            StartCoroutine(AIMoveWithDelay());
        }
    }

    private IEnumerator AIMoveWithDelay()
    {
        EnableAllButtons(false);
        yield return new WaitForSeconds(1f);
        MakeAIMove();
        if(gameActive)
        {
            isPlayersTurn = true;
            EnableAllButtons(true);
        }
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
        CheckGameState();
    }

    void CheckGameState()
    {
        int winner = MinMaxAI.Win(board);
        if (winner == -1)
        {
            gameActive = false;
            EnableAllButtons(false);
            ScoreKeeper.WinCount++;
            Debug.Log("Player Wins");
            endPanel.SetActive(true);
            winsText.text = $"Player: {ScoreKeeper.WinCount}";
            Transform child = endPanel.transform.Find("Titles/WinTitle");
            if (child != null)
                child.gameObject.SetActive(true);

        }
        else if (winner == 1)
        {
            gameActive = false;
            EnableAllButtons(false);
            ScoreKeeper.LossCount++;
            Debug.Log("AI Wins / Player Loss");
            endPanel.SetActive(true);
            lossText.text = $"AI: {ScoreKeeper.LossCount}";
            Transform child = endPanel.transform.Find("Titles/LossTitle");
            if (child != null)
                child.gameObject.SetActive(true);
        }
        else if (IsBoardFull())
        {
            gameActive = false;
            EnableAllButtons(false);
            ScoreKeeper.DrawCount++;
            Debug.Log("Draw game");
            endPanel.SetActive(true);
            drawText.text = $"Draws: {ScoreKeeper.DrawCount}";
            Transform child = endPanel.transform.Find("Titles/DrawTitle");
            if (child != null)
                child.gameObject.SetActive(true);
        }
    }

    bool IsBoardFull()
    {
        foreach (int cell in board)
            if (cell == 0) return false;
        return true;
    }

    void EnableAllButtons(bool enable)
    {
        foreach (Button btn in playerInputs.gameButtons)
        {
            if (btn != null && btn.interactable != enable && btn.GetComponentInChildren<TMP_Text>().text == "")
                btn.interactable = enable;
        }
    }

}
