using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EnemyUi : MonoBehaviour
{
    [SerializeField]
    private Image healthBarFilled;
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField] 
    private TextMeshProUGUI damageText;

    [SerializeField] 
    private Image intentIcon; // Icon to represent the enemy's current intent

    [SerializeField]
    private Enemy enemy; // Reference to the Enemy script

    private PhasePayload enemyIntent; // Store the current intent for UI updates



    private void OnEnable()
    {
        enemy.OnHealthChanged += UpdateHealthUI; // Subscribe to the health change event
        enemy.OnIntentChanged += UpdateDamageUi; // Subscribe to the intent change event
    }
    private void OnDisable()
    {
        enemy.OnHealthChanged -= UpdateHealthUI; // Unsubscribe to prevent memory leaks
        enemy.OnIntentChanged -= UpdateDamageUi; // Subscribe to the intent change event
    }

    private void UpdateHealthUI(int currentHp, int maxHp)
    {
        healthBarFilled.fillAmount = (float)currentHp / maxHp;

        healthText.text = $"{currentHp}/{maxHp}";
    }

    private void UpdateDamageUi()
    {
        enemyIntent = enemy.CurrentIntent; // Get the current intent from the Enemy script

        if (enemyIntent.phases != null)
        {
            damageText.text = enemyIntent.amount.ToString();// Update the damage text based on the intent

            intentIcon.sprite = enemyIntent.phases.IntentIcon; // Update the intent icon based on the intent

        }
    }
}
