using UnityEngine;

public class Enemy_DeadState : EnemyState
{
    public AnimationClip anim;
    public override void Enter()
    {
        if (anim == null || animator == null) return;
        animator.Play(anim.name);
    }

    public override void Do()
    {
        Destroy(enemyInput.gameObject);
        //isCompleted = true;   //no need this, enemy not working after death
    }

    public override void Exit()
    {

    }
}
