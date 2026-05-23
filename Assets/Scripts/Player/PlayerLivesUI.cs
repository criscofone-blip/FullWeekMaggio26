using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLivesUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("UI")]
    [SerializeField] private RectTransform heartsContainer;
    [SerializeField] private Image heartPrefab;

    private readonly List<Image> hearts = new();

    private void Start()
    {
        CreateHearts();

        playerHealth.OnLivesChanged.AddListener(UpdateHearts);

        UpdateHearts(playerHealth.CurrentLives);
    }

    // Crea un cuore per ogni vita massima del player.
    private void CreateHearts()
    {
        for (int i = 0; i < playerHealth.MaxLives; i++)
        {
            Image heart = Instantiate(heartPrefab, heartsContainer);
            heart.gameObject.SetActive(true);
            hearts.Add(heart);
        }
    }

    // Mostra solo i cuori corrispondenti alle vite rimaste.
    private void UpdateHearts(int currentLives)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].gameObject.SetActive(i < currentLives);
        }
    }
}