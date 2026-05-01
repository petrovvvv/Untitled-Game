using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Leg : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D c)
    {
        c.gameObject.GetComponent<PlayerCollision>().AddLeg();
        gameObject.SetActive(false);
    }
}