using UnityEngine;

public class DamageCollectable : MonoBehaviour, ICollectable
{
    public int damageAmount = 5;

    public void Collect(GameObject player)
    {

            GameManager.Instance.TakeDamage(damageAmount);
            Destroy(gameObject);

    }
}