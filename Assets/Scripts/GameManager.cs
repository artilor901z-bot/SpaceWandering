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
    public string gameOverSceneName = "GameOverScene"; // 添加结束场景的名称

    public AudioClip damageAudioClip; // 玩家受伤时的音频
    public AudioClip scoreAudioClip; // 玩家加分时的音频
    public AudioClip collectAudioClip; // 玩家收集物品时的音频
    public AudioSource audioSource;

    private bool isInvincible = false;
    private float invincibilityEndTime;
    private float displayedHealth;

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
        audioSource = GetComponent<AudioSource>();

        healthBar.maxValue = playerHealth;
        packageBar.maxValue = maxAmmo;

        displayedHealth = playerHealth;

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

        displayedHealth = Mathf.Lerp(displayedHealth, playerHealth, Time.deltaTime * 5);
        healthBar.value = displayedHealth;
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        Debug.Log($"Player took {damage} damage, current health: {playerHealth}");
        if (damageAudioClip != null)
        {
            audioSource.PlayOneShot(damageAudioClip);
        }
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            Debug.Log("Player Died");
            StartGameOver();
        }
        UpdateUI();
    }

    // 添加新方法处理游戏结束
    private void StartGameOver()
    {
        // 保存最终分数，以便在结束场景显示
        PlayerPrefs.SetInt("FinalScore", playerScore);
        PlayerPrefs.Save();

        // 使用协程来添加短暂延迟，让玩家看到死亡瞬间
        StartCoroutine(GameOverSequence());
    }

    private System.Collections.IEnumerator GameOverSequence()
    {
        // 等待短暂时间（例如1秒）让玩家看到死亡画面
        yield return new WaitForSeconds(1f);

        // 加载结束场景
        SceneManager.LoadScene(gameOverSceneName);
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
        if (scoreAudioClip != null)
        {
            audioSource.PlayOneShot(scoreAudioClip);
        }
        UpdateUI();
    }

    public void AddAmmo(int ammo)
    {
        currentAmmo = Mathf.Min(currentAmmo + ammo, maxAmmo);
        if (collectAudioClip != null)
        {
            audioSource.PlayOneShot(collectAudioClip);
        }
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
