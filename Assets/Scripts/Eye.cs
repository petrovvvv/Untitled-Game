using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Eye : MonoBehaviour
{
  [SerializeField] private Animator blindnessAnimator;
  
  // All objects to be made visible when eyes are acquired
  void OnTriggerEnter2D(Collider2D c)
  {
    blindnessAnimator.SetTrigger("Lift");
    c.gameObject.GetComponentInParent<Player>().AddEyes();
    gameObject.SetActive(false);
  }
}