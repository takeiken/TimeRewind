using UnityEngine;
using UnityEngine.U2D;

public class IdleState : State
{
    public AnimationClip anim;
    public override void Enter()
    {
        animator.Play(anim.name);
        movementInput.sprite.color = Color.white;
    }

    public override void Do()
    {
        if (movementInput.switchBodyTriggered)
        {
            movementInput.switchBodyTriggered = false;
            movementInput.currentBodyType = CharacterBodyType.BodyType.Physical;
            movementInput.SwitchPlayerTag();
        }

        if (CharacterPuppet.Instance != null)
        {
            //teleport player back to puppet & destroy puppet clone
            movementInput.transform.position = CharacterPuppet.Instance.transform.position;
            Destroy(CharacterPuppet.Instance.gameObject);
        }
        isCompleted = true;
    }

    public override void Exit()
    {

    }
}
