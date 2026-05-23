using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Health playerHealth;

    [Header("Hearts")]
    [SerializeField] private Image[] hearts;

    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    private void Start()
    {
        UpdateHearts(playerHealth.CurrentHealth, playerHealth.MaxHealth);
        playerHealth.OnHealthChanged.AddListener(UpdateHearts);
    }

    // Aggiorna i cuoricini in base alle vite rimaste.
    private void UpdateHearts(int currentHealth, int maxHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < currentHealth ? fullHeart : emptyHeart;
            hearts[i].enabled = i < maxHealth;
        }
    }
}