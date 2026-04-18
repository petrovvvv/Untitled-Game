using UnityEngine;

public class Eye : MonoBehaviour
{
    // All objects to be made visible when eyes are acquired
  void OnTriggerEnter2D(Collider2D c)
  {
    c.gameObject.GetComponent<Player>().AddEyes();
    gameObject.SetActive(false);
  }
}