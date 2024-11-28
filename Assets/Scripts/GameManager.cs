using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int playerHealth = 100;
    public int playerScore = 0;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;
    public Slider healthBar;
    public TextMeshProUGUI gameOverText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
        gameOverText.gameObject.SetActive(false); // 初始隐藏Game Over文本
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            // 处理玩家死亡
            Debug.Log("Player Died");
            gameOverText.gameObject.SetActive(true); // 显示Game Over文本
        }
        UpdateUI();
    }

    public void AddScore(int score)
    {
        playerScore += score;
        UpdateUI();
    }

    void UpdateUI()
    {
        healthText.text = "Health: " + playerHealth;
        scoreText.text = "Score: " + playerScore;
        healthBar.value = playerHealth; // 更新血条值
    }
}