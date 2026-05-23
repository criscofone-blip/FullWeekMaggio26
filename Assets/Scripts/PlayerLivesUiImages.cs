using UnityEngine;
using UnityEngine.UI;

public class PlayerLivesUIImages : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Life Images")]
    [SerializeField] private Image imageThreeLives;
    [SerializeField] private Image imageTwoLives;
    [SerializeField] private Image imageOneLife;

    private void Start()
    {
        playerHealth.OnLivesChanged.AddListener(UpdateLivesUI);
        UpdateLivesUI(playerHealth.CurrentLives);
    }

    // Mostra una sola immagine in base alle vite rimaste.
    private void UpdateLivesUI(int currentLives)
    {
        imageThreeLives.gameObject.SetActive(currentLives == 3);
        imageTwoLives.gameObject.SetActive(currentLives == 2);
        imageOneLife.gameObject.SetActive(currentLives == 1);
    }
}
