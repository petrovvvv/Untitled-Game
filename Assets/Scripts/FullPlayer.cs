using UnityEngine;
using UnityEngine.Animations;

public class FullPlayer : Player
{
  public override void AddLeg()
  {
    anim.SetTrigger("Leg2");
    transform.GetComponentInParent<PlayerController>().AddLeg2();
  }

  public override void SetAnimation(float dx, bool grounded, bool jump)
  {
    base.SetAnimation(dx, grounded, jump);
    anim.SetBool("Grounded", grounded);
    if (jump)
    {
      anim.SetTrigger("Jump");
    }
  }
}
