using UnityEngine;

public class Soul_AttackState : State
{
    public AnimationClip anim;
    public override void Enter()
    {
        animator.Play(anim.name);
        movementInput.sprite.color = Color.blue;
    }

    public override void Do()
    {

        isCompleted = true;
    }

    public override void Exit()
    {

    }
}
