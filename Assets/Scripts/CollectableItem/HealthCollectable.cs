using UnityEngine;

public class HealthCollectable : MonoBehaviour, ICollectable
{
    public int healthAmount = 10;

    public void Collect(GameObject player)
    {
        // 增加玩家的生命值
        GameManager.Instance.AddHealth(healthAmount);
        Destroy(gameObject);
    }
}