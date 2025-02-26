using System.Collections;
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
        if (movementInput.attackTriggered)
        {
            movementInput.attackTriggered = false;
            SoulMeleeAttack();
        }
        isCompleted = true;
    }

    public void SoulMeleeAttack()
    {
        if (movementInput.isAttacking) return;
        movementInput.isAttacking = true;
        movementInput.characterHitbox.EnableHitbox(true);
        StartCoroutine(SoulAttackCooldown());
    }

    IEnumerator SoulAttackCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        movementInput.isAttacking = false;
        movementInput.characterHitbox.EnableHitbox(false);
    }

    public override void Exit()
    {

    }
}
