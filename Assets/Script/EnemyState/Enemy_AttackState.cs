using System.Collections;
using UnityEngine;

public class Enemy_AttackState : EnemyState
{
    public AnimationClip anim;
    public override void Enter()
    {
        if (anim == null || animator == null) return;
        animator.Play(anim.name);
    }

    public override void Do()
    {
        if (enemyInput.canAttack && enemyInput.canSeePlayer)
        {
            enemyInput.isAttacking = true;
            StartCoroutine(StartAttack());
        };

        isCompleted = true;
    }

    IEnumerator StartAttack()
    {
        enemyInput.TriggerAttack();
        enemyInput.playerInRange = false;
        yield return new WaitForSeconds(1f);
        enemyInput.isAttacking = false;
    }

    public override void Exit()
    {

    }
}
