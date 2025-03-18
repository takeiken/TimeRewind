using System.Collections;
using UnityEngine;

public class Soul_DashState : PlayerState
{
    public AnimationClip anim;
    public override void Enter()
    {
        animator.Play(anim.name);
        movementInput.sprite.color = Color.blue;
    }

    public override void Do()
    {
        if (movementInput.dashTriggered)
        {
            movementInput.dashTriggered = false;
            SoulStartDash();
        }
        isCompleted = true;
    }

    public void SoulStartDash()
    {
        if (movementInput.isDashing) return;
        movementInput.isDashing = true;
        movementInput.ableToDash = false;
        StartCoroutine(SoulDashCooldown());
    }

    IEnumerator SoulDashCooldown()
    {
        if (movementInput.lastMovement == Vector2.zero)
        {
            movementInput.lastMovement = Vector2.right;
        }
        Vector2 startPosition = movementInput.rb.position; // Use Rigidbody position
        Vector2 targetPosition = startPosition + movementInput.lastMovement * movementInput.dashDistance;

        float dashingTime = 0f;
        while (dashingTime < movementInput.dashDuration)
        {
            dashingTime += Time.deltaTime;
            float t = 1f - ((movementInput.dashDuration - dashingTime) / movementInput.dashDuration); // Normalize time

            Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, t); // Interpolate position

            movementInput.rb.MovePosition(newPosition); // Move the Rigidbody to the new position
            yield return null; // Wait for the next frame
        }
        movementInput.rb.MovePosition(targetPosition);
        movementInput.isDashing = false;

        //Make dash avaliable after cooldown
        Invoke(nameof(EnableSoulDash), movementInput.dashCooldown);
    }

    private void EnableSoulDash()
    {
        movementInput.ableToDash = true;
    }

    public override void Exit()
    {

    }
}
