using UnityEngine;

public class TripleShotCollectable : MonoBehaviour, ICollectable
{
    public void Collect(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.EnableTripleShot();
        }
        Destroy(gameObject);
    }
}