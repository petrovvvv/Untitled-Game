using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Arm : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D c)
    {
        c.gameObject.GetComponentInParent<Player>().AddArm();
        gameObject.SetActive(false);
    }
}
