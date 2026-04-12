using UnityEngine;

public class Leg : MonoBehaviour
{
    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D c)
    {
        c.gameObject.GetComponent<Player>().AddLeg();
        gameObject.SetActive(false);
    }
}
