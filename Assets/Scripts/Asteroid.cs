using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float shrinkAmount = 0.5f; // 缩小量

    public void TakeDamage()
    {
        transform.localScale -= new Vector3(shrinkAmount, shrinkAmount, 0);
        if (transform.localScale.x <= 0 || transform.localScale.y <= 0)
        {
            Destroy(gameObject); // 销毁陨石
        }
    }
}