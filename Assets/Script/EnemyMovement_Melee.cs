using System.Collections;
using UnityEngine;

public class EnemyMovement_Melee : EnemyMovement
{
    private Collider2D attackCollider;

    public override void Start()
    {
        base.Start();
        if(attackCollider == null)
        {
            attackCollider = FindChildWithTag(gameObject, "EnemyAttackHitbox").GetComponent<Collider2D>();
            attackCollider.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (canAttack && canSeePlayer)
        {
            StartCoroutine(MeleeAttackRoutine());
        }
    }

    private IEnumerator MeleeAttackRoutine()
    {
        canAttack = false;
        if (attackCollider != null)
        {
            attackCollider.enabled = true;
            yield return new WaitForSeconds(0.1f);
            attackCollider.enabled = false;
        }

        yield return new WaitForSeconds(attackFrequency);
        canAttack = true;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D (collision);
        if (collision.CompareTag("Player"))
        {
            CharacterMovement.Instance.onPlayerDamaged?.Invoke();
        }
    }

    GameObject FindChildWithTag(GameObject parent, string tag)  
    {
        GameObject child = null;

        foreach (Transform transform in parent.transform)
        {
            if (transform.CompareTag(tag))
            {
                child = transform.gameObject;
                break;
            }
        }

        return child;
    }
}
