using UnityEngine;

public class Soul_IdleState : State
{
    public AnimationClip anim;
    public override void Enter()
    {
        animator.Play(anim.name);
        movementInput.sprite.color = Color.blue;
    }

    public override void Do()
    {
        if (movementInput.switchBodyTriggered)
        {
            movementInput.switchBodyTriggered = false;
            movementInput.currentBodyType = CharacterBodyType.BodyType.Soul;
            movementInput.SwitchPlayerTag();
        }

        if(CharacterPuppet.Instance == null)
        {
            GameObject puppet = Instantiate(movementInput.puppetPrefab, movementInput.transform.position, Quaternion.identity);
            puppet.GetComponent<CharacterPuppet>().sprite.flipX = movementInput.sprite.flipX;
        }
        isCompleted = true;
    }

    public override void Exit()
    {

    }
}
