using UnityEngine;
using UnityEngine.Animations;

public class FullPlayer : Player
{
  private bool hasArm;

  void Start()
  {
    Startup();
    hasArm = false;
  }
  public override void AddLeg()
  {
    anim.SetTrigger("Leg2");
    transform.GetComponentInParent<PlayerController>().AddLeg2();
  }

  public override void AddArm()
  {
    if (!hasArm) {
      anim.SetTrigger("Arm1");
      transform.GetComponentInParent<PlayerController>().AddArm1();
      hasArm = true;
    } else
    {
      anim.SetTrigger("Arm2");
      transform.GetComponentInParent<PlayerController>().AddArm2();
    }
  }

  public override void SetAnimation(float dx, bool grounded, bool jump)
  {
    base.SetAnimation(dx, grounded, jump);
    anim.SetBool("Grounded", grounded);
    anim.SetBool("Left", transform.localScale.x < 0f);
    if (jump)
    {
      anim.SetTrigger("Jump");
    }
  }
}
