using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUi : MonoBehaviour
{
    [Header("Health UI")]
    [SerializeField]
    private Image healthBarFilled;
    [SerializeField]
    private TextMeshProUGUI healthText;

    [Header("Shield UI")]
    [SerializeField]
    private Image shieldImg;
    [SerializeField]
    private TextMeshProUGUI shieldText;

    [Header("Player Reference")]
    [SerializeField]
    private Player player; // Reference to the Enemy script


    private void OnEnable()
    {
        player.OnHealthChanged += UpdateHealthUI; // Subscribe to the health change event
        player.OnShield += UpdateShieldUI; // Subscribe to the shield change event
    }
    private void OnDisable()
    {
        player.OnHealthChanged -= UpdateHealthUI; // Unsubscribe to prevent memory leaks
        player.OnShield -= UpdateShieldUI; // Unsubscribe to prevent memory leaks
    }

    private void UpdateHealthUI(int currentHp, int maxHp)
    {
        healthBarFilled.fillAmount = (float)currentHp / maxHp;

        healthText.text = $"{currentHp}/{maxHp}";
    }

    private void UpdateShieldUI(int num)
    {
        if (num > 0)
        {
            shieldImg.gameObject.SetActive(true);
            shieldText.gameObject.SetActive(true);
            shieldText.text = num.ToString();

        }
        else
        {
            shieldImg.gameObject.SetActive(false);
            shieldText.gameObject.SetActive(false);
        }
    }
}
