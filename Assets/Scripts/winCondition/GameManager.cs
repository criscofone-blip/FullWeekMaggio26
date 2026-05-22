using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Win Condition")]
    [SerializeField] private int piecesToWin = 3;

    [Header("UI Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    private int collectedPieces;
    private bool gameEnded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    // Aggiunge un pezzo raccolto e controlla se abbiamo vinto.
    public void AddPiece(int amount)
    {
        if (gameEnded)
            return;

        collectedPieces += amount;

        if (collectedPieces >= piecesToWin)
        {
            WinGame();
        }
    }

    // Mostra il panel di vittoria.
    private void WinGame()
    {
        gameEnded = true;

        winPanel.SetActive(true);
        losePanel.SetActive(false);

        UnlockCursor();

        Time.timeScale = 0f;
    }

    // Mostra il panel di sconfitta.
    public void LoseGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        losePanel.SetActive(true);
        winPanel.SetActive(false);

        UnlockCursor();

        Time.timeScale = 0f;
    }

    // Ricarica la scena corrente.
    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Sblocca il cursore per usare i bottoni UI.
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}