using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class AttackState : PlayerState
{
    public AnimationClip anim;

    public override void Enter()
    {
        animator.Play(anim.name);
        movementInput.sprite.color = Color.white;
    }

    public override void Do()
    {
        if (movementInput.attackTriggered)
        {
            movementInput.attackTriggered = false;
            //Shoot();
            MeleeAttack();
        }
        isCompleted = true;
    }

    public void MeleeAttack()
    {
        if (movementInput.isAttacking) return;
        movementInput.isAttacking = true;
        movementInput.ableToAttack = false;
        movementInput.characterHitbox.EnableHitbox(true);
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        movementInput.isAttacking = false;
        movementInput.characterHitbox.EnableHitbox(false);

        Invoke(nameof(EnableAttack), movementInput.attackCooldown);
    }

    private void EnableAttack()
    {
        movementInput.ableToAttack = true;
    }

    public void Shoot()
    {
        if (movementInput.currentProjectile != null) return;

        movementInput.isAttacking = true;

        // Create a projectile instance
        movementInput.currentProjectile = Instantiate(movementInput.projectilePrefab, transform.position, Quaternion.identity);

        // Calculate direction to the cursor
        Vector2 direction = (movementInput.GetCursorPosition() - body.position).normalized;

        // Set the projectile's rotation to face the cursor
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        movementInput.currentProjectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        movementInput.currentProjectile.GetComponent<SimpleProjectile>().SetCharacter(movementInput);

        StartCoroutine(ShootCooldown());
    }

    IEnumerator ShootCooldown()
    {
        yield return new WaitUntil(() => movementInput.currentProjectile == null);
        movementInput.isAttacking = false;
    }

    public override void Exit()
    {

    }
}
