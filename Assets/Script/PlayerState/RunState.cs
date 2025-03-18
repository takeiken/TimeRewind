using UnityEngine;
using UnityEngine.U2D;

public class RunState : PlayerState
{
    public AnimationClip anim;
    public override void Enter()
    {
        animator.Play(anim.name);
        movementInput.sprite.color = Color.white;
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
