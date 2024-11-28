using UnityEngine;

public class AbsorbController : MonoBehaviour
{
    public float absorbRadius = 5f;
    public float absorbSpeed = 10f;
    public Transform hand;
    public Transform body;

    private bool isAbsorbing = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAbsorbing = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isAbsorbing = false;
        }

        if (isAbsorbing)
        {
            AbsorbCollectables();
        }
    }

    void AbsorbCollectables()
    {
        Collider2D[] collectables = Physics2D.OverlapCircleAll(body.position, absorbRadius);
        Vector3 handDirection = (hand.position - body.position).normalized;

        foreach (Collider2D collectable in collectables)
        {
            if (collectable.CompareTag("Collectable"))
            {
                Vector3 directionToCollectable = (collectable.transform.position - body.position).normalized;
                float dotProduct = Vector3.Dot(handDirection, directionToCollectable);

                // 优先吸引hand朝向的物体
                if (dotProduct > 0.5f) // 0.5f 是一个阈值，可以根据需要调整
                {
                    MoveCollectableTowardsPlayer(collectable);
                }
            }
        }
    }

    void MoveCollectableTowardsPlayer(Collider2D collectable)
    {
        Vector3 direction = (body.position - collectable.transform.position).normalized;
        collectable.transform.position = Vector3.MoveTowards(collectable.transform.position, body.position, absorbSpeed * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(body.position, absorbRadius);
    }
}
