/*
 *  For simplicity, currently both player types have the same class, TODO: probably make a class heirarchy
 */

using UnityEngine;
public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private Animator blindnessAnimator;

    public virtual void AddEyes()
    {
        blindnessAnimator.SetTrigger("Lift");
        transform.GetComponentInParent<Player>().AddEyes();
    }
    public virtual void AddLeg()
    {
        transform.GetComponentInParent<Player>().AddLeg();
    }
    public virtual void AddArm()
    {
        transform.GetComponentInParent<Player>().AddArm();
    }
}