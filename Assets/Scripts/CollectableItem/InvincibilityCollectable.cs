using UnityEngine;

public class InvincibilityCollectable : MonoBehaviour, ICollectable
{
    public float invincibilityDuration = 5f;

    public void Collect(GameObject player)
    {
        // 使玩家无敌
        //GameManager.Instance.ActivateInvincibility(invincibilityDuration);
        Destroy(gameObject);
    }
}