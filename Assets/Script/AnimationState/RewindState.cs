using UnityEngine;
using UnityEngine.U2D;

public class RewindState : State
{
    public AnimationClip anim;
    public override void Enter()
    {
        animator.Play(anim.name);
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
