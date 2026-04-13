using UnityEngine;

public class StartPlayer : Player
{
   
    void Start()
    {
        Startup();
    }
    public override void AddEyes()
    {
        anim.SetTrigger("Eyes");
        transform.Find("BlindCurtain").gameObject.GetComponent<Animator>().SetTrigger("Eyes");
    }

    public override void AddLeg()
    {
        transform.GetComponentInParent<PlayerController>().AddLeg1();
    }
}
