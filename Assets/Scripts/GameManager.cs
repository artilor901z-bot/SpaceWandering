using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int playerHealth = 100;
    public int playerScore = 0;
    public int maxAmmo = 50; // 最大弹药数
    public int currentAmmo = 20; // 初始弹药数
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI packageText; // 新增的弹药文本
    public Slider healthBar;
    public Slider packageBar; // 新增的弹药条
    public TextMeshProUGUI gameOverText;
    public AbsorbController absorbController; // 新增的 AbsorbController 引用

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
        // 设置 healthBar 和 packageBar 的最大值
        healthBar.maxValue = playerHealth;
        packageBar.maxValue = maxAmmo;

        // 初始化 UI
        UpdateUI();
        gameOverText.gameObject.SetActive(false); // 初始隐藏Game Over文本

        // 禁用 Slider 的交互性
        healthBar.interactable = false;
        packageBar.interactable = false; // 禁用弹药条的交互性

        // 设置 AbsorbController 的 GameManager
        if (absorbController != null)
        {
            absorbController.SetGameManager(this);
        }
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

    public void AddAmmo(int ammo)
    {
        currentAmmo = Mathf.Min(currentAmmo + ammo, maxAmmo); // 增加弹药并确保不超过最大值
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
        packageText.text = "Ammo: " + currentAmmo; // 更新弹药文本
        healthBar.value = playerHealth; // 更新血条值
        packageBar.value = currentAmmo; // 更新弹药条值
    }
}
