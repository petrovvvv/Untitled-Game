using UnityEngine;

public class Arm : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D c)
    {
        c.gameObject.GetComponent<Player>().AddArm();
        gameObject.SetActive(false);
    }
}
