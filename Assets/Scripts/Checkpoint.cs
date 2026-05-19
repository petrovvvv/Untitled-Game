using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D c)
    {
        c.gameObject.GetComponent<Player>().SetCheckpoint(transform.position);
    }
}
