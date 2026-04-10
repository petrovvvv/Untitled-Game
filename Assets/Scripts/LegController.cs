using UnityEngine;

public class LegController : MonoBehaviour
{
    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D c)
    {
        c.gameObject.GetComponentInParent<PlayerController>().AddLeg1();
        gameObject.SetActive(false);
    }
}
