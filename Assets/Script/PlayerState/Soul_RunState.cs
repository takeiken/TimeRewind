using UnityEngine;

public class Soul_RunState : PlayerState
{
    public AnimationClip anim;
    public override void Enter()
    {
        animator.Play(anim.name);
        movementInput.sprite.color = Color.blue;
    }

    public override void Do()
    {
        animator.speed = Helpers.Map(movementInput.moveSpeed, 0, 1, 0, 1.6f, true);

        if (Mathf.Abs(movementInput.movement.x) > 0f)
        {
            movementInput.sprite.flipX = movementInput.movement.x < 0f;
        }
        isCompleted = true;
    }

    private void Update()
    {
        if (CharacterPuppet.Instance == null) return;

        float dis = Vector3.Distance(CharacterPuppet.Instance.transform.position, movementInput.transform.position);
        if (dis >= movementInput.maxSeperateDistance)
        {
            movementInput.ForceReturnPuupet();
        }
    }

    public override void Exit()
    {

    }
}
