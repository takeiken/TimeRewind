using UnityEngine;

public class Enemy_IdleState : EnemyState
{
    public AnimationClip anim;
    public override void Enter()
    {
        if (anim == null || animator == null) return;
        animator.Play(anim.name);
    }

    public override void Do()
    {

        isCompleted = true;
    }

    public override void Exit()
    {

    }
}
