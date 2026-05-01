using UnityEngine;

public class Hazard : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D c)
    {
        c.gameObject.GetComponentInParent<Player>().TakeDamage(1);
    }
}