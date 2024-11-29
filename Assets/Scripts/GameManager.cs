using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int playerHealth = 100;
    public int playerScore = 0;
    public int maxAmmo = 50;
    public int currentAmmo = 20;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI packageText;
    public Slider healthBar;
    public Slider packageBar;
    public TextMeshProUGUI gameOverText;
    public Button retryButton;
    public AbsorbController absorbController;

    private bool isInvincible = false;
    private float invincibilityEndTime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("GameManager instance initialized.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        healthBar.maxValue = playerHealth;
        packageBar.maxValue = maxAmmo;

        UpdateUI();
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        healthBar.interactable = false;
        packageBar.interactable = false;

        if (absorbController != null)
        {
            absorbController.SetGameManager(this);
        }

        retryButton.onClick.AddListener(RetryGame);
    }

    void Update()
    {
        if (isInvincible && Time.time > invincibilityEndTime)
        {
            isInvincible = false;
        }
    }

    public void TakeDamage(int damage)
    {

        playerHealth -= damage;
        Debug.Log($"Player took {damage} damage, current health: {playerHealth}");
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            Debug.Log("Player Died");
            gameOverText.text = $"Game Over\nFinal Score: {playerScore}";
            gameOverText.gameObject.SetActive(true);
            retryButton.gameObject.SetActive(true);
        }
        UpdateUI();
    }

    public void AddHealth(int amount)
    {
        playerHealth = Mathf.Min(playerHealth + amount, 100);
        Debug.Log("Health added. Current health: " + playerHealth);
        UpdateUI();
    }

    public void AddScore(int score)
    {
        playerScore += score;
        UpdateUI();
    }

    public void AddAmmo(int ammo)
    {
        currentAmmo = Mathf.Min(currentAmmo + ammo, maxAmmo);
        UpdateUI();
    }

    public bool UseAmmo(int amount)
    {
        if (currentAmmo >= amount)
        {
            currentAmmo -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }


    void UpdateUI()
    {
        healthText.text = "Health: " + playerHealth;
        scoreText.text = "Score: " + playerScore;
        packageText.text = "Ammo: " + currentAmmo;
        healthBar.value = playerHealth;
        packageBar.value = currentAmmo;
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

