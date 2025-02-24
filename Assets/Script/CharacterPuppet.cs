using UnityEngine;

public class CharacterPuppet : MonoBehaviour
{
    public static CharacterPuppet Instance;
    public SpriteRenderer sprite;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
