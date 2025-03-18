using UnityEngine;
using UnityEngine.U2D;

public class RewindState : PlayerState
{
    public AnimationClip anim;
    public override void Enter()
    {
        animator.Play(anim.name);
        movementInput.sprite.color = Color.white;
    }

    public override void Do()
    {
        movementInput.StartRewindActions();
        isCompleted = true;
    }

    public override void Exit()
    {

    }
}
