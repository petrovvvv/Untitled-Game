using UnityEngine;

public class Leg : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D c)
    {
        c.gameObject.GetComponent<Player>().AddLeg();
        gameObject.SetActive(false);
    }
}