using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxLives = 3;

    private int currentLives;

    public int CurrentLives => currentLives;
    public int MaxLives => maxLives;

    public UnityEvent<int> OnLivesChanged;
    public UnityEvent OnPlayerDeath;

    private void Awake()
    {
        currentLives = maxLives;
    }

    private void Start()
    {
        OnLivesChanged?.Invoke(currentLives);
    }

    // Toglie una o più vite al player.
    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);

        OnLivesChanged?.Invoke(currentLives);

        if (currentLives <= 0)
        {
            Die();
        }
    }

    // Gestisce la morte del player.
    private void Die()
    {
        OnPlayerDeath?.Invoke();

        Debug.Log("PLAYER DEAD");

        // Per ora disattiviamo il player.
        gameObject.SetActive(false);
    }
}