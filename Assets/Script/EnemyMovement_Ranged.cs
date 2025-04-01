using System.Collections;
using UnityEngine;

public class EnemyMovement_Ranged : EnemyMovement
{
    public override void TriggerAttack()
    {
        base.TriggerAttack();
        StartCoroutine(ShootingRoutine());

        /*if (canAttack && canSeePlayer)
        {
            StartCoroutine(ShootingRoutine());
        }*/
    }

    private IEnumerator ShootingRoutine()
    {
        canAttack = false;
        ShootPlayer();
        yield return new WaitForSeconds(attackFrequency);

        canAttack = true;
    }

    public void ShootPlayer()
    {
        // Create a projectile instance
        GameObject currentProjectile = Instantiate(enemyProjectile, transform.position, Quaternion.identity);

        // Calculate direction to the player
        Vector2 direction = ((Vector2)CharacterMovement.Instance.transform.position - rb.position).normalized;

        // Set the projectile's rotation to face the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        currentProjectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
