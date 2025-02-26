using UnityEngine;

public class CharacterHitbox : MonoBehaviour
{
    private CharacterMovement movementInput;
    private Collider2D hitboxCollider;

    private void Start()
    {
        hitboxCollider = GetComponent<Collider2D>();
        hitboxCollider.enabled = false;
    }

    public void SetupHitbox(CharacterMovement input)
    {
        movementInput = input;
    }

    public void EnableHitbox(bool enable)
    {
        hitboxCollider.enabled = enable;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            print("enemy found");
            EnemyMovement enemy = collision.gameObject.GetComponent<EnemyMovement>();
            if (enemy != null)
            {
                print("enemy script found");
                if (movementInput.currentBodyType == enemy.bodyType)
                {
                    //kill enemy
                    Destroy(enemy.gameObject);
                }
            }
        }
    }
}
