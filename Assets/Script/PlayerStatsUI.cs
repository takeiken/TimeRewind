using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    private CharacterMovement character;
    public TextMeshProUGUI lifeText;

    private void Start()
    {
        character = CharacterMovement.Instance;
        UpdatePlayerLife();
    }

    public void UpdatePlayerLife()
    {
        lifeText.text = "Life: " + character.lifeCount.ToString();
    }
}
