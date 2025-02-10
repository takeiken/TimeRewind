using UnityEngine;
using UnityEngine.U2D;

public class RunState : State
{
    public AnimationClip anim;
    public override void Enter()
    {
        animator.Play(anim.name);
    }

    public override void Do()
    {
        animator.speed = Helpers.Map(movementInput.moveSpeed, 0, 1, 0, 1.6f, true);

        if (Mathf.Abs(movementInput.movement.x) > 0f)
        {
            movementInput.sprite.flipX = movementInput.movement.x < 0f;
        }
        isCompleted = true;
        /*if ((Mathf.Abs(movementInput.movement.x) + Mathf.Abs(movementInput.movement.y)) <= 0f)
        {
            isCompleted = true;
        }*/
    }

    public override void Exit() 
    { 

    }
}
