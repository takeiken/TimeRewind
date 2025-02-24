using UnityEngine;
using UnityEngine.Events;

public class EnemyHitBox : MonoBehaviour
{
    private EnemyMovement enemyParent;
    private AttackType.EnemyAttackType attackType;

    public void Setup(EnemyMovement em)
    {
        enemyParent = em;
        switch (LayerMask.LayerToName(gameObject.layer))
        {
            case "EnemyPhysicalAttack":
                attackType = AttackType.EnemyAttackType.Physical;
                break;

            case "EnemySoulAttack":
                attackType = AttackType.EnemyAttackType.Soul;
                break;

            case "EnemyMixedAttack":
                attackType = AttackType.EnemyAttackType.Both;
                break;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if ((CharacterMovement.Instance.currentBodyType == CharacterBodyType.BodyType.Soul && (attackType == AttackType.EnemyAttackType.Soul || attackType == AttackType.EnemyAttackType.Both)) ||
        (CharacterMovement.Instance.currentBodyType == CharacterBodyType.BodyType.Physical && (attackType == AttackType.EnemyAttackType.Physical || attackType == AttackType.EnemyAttackType.Both)))
        {
            print("Enemey attack type: " + attackType);
            CharacterMovement.Instance.onPlayerDamaged?.Invoke();
        }
    }
}
