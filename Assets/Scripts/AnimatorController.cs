using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public Controller player;

    //animator variables
    private int animGrounded;
    private int animYVelocity;
    private int animXVelocity;
    private int animJump;
    private int animCharge;
    private int animExit;

    private bool exited=false;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        animGrounded = Animator.StringToHash("isGrounded");
        animYVelocity = Animator.StringToHash("YVelocity");
        animXVelocity = Animator.StringToHash("XVelocity");
        animJump = Animator.StringToHash("Jump");
        animCharge = Animator.StringToHash("Charge");
        animExit = Animator.StringToHash("Exit");
    }

    private void Update()
    {
        if (player.exit && !exited){
            anim.SetTrigger(animExit);
            exited = true;
        }
        if (!player.simulate) return;
        anim.SetBool(animGrounded, player.isGrounded);
        anim.SetFloat(animYVelocity, player.rg.velocity.y);
        anim.SetFloat(animXVelocity, player.rg.velocity.x);
        anim.SetBool(animJump, player.jump);
        anim.SetBool(animCharge, player.charging);
    }

    public void JumpTrigger()
    { player.Jump(); }

    public void NexLevelTrigger()
    { Scene_Manager.instance.LoadScene(Scene_Manager.instance.GetActiveIndex() + 1);}

    public void LandedTrigger()
    { player.landed = true; }
    
}
