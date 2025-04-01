using Unity.VisualScripting;
using UnityEngine;

public class Enemy_FollowState : EnemyState
{
    public AnimationClip anim;
    public override void Enter()
    {
        if (anim == null || animator == null) return;
        animator.Play(anim.name);
    }

    public override void Do()
    {
        if (CharacterMovement.Instance.puppetLifeCount <= 0)
        {
            enemyInput.seenPlayer = false;
        }

        if(Vector2.Distance(CharacterMovement.Instance.gameObject.transform.position,enemyInput.transform.position) <= enemyInput.attackRange) 
        {
            enemyInput.playerInRange = true;
        }
        else
        {
            enemyInput.playerInRange = false;
        }
        isCompleted = true;
    }

    public override void Exit()
    {

    }
}
