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

    [Header("Shield UI")]
    [SerializeField]
    private Image shieldImg;
    [SerializeField]
    private TextMeshProUGUI shieldText;

    [SerializeField]
    private Enemy enemy; // Reference to the Enemy script

    [SerializeField]
    private StatusManager statusManager;

    private PhasePayload enemyIntent; // Store the current intent for UI updates



    private void OnEnable()
    {
        if (enemy != null)
        {
            enemy.OnHealthChanged += UpdateHealthUI; // Subscribe to the health change event
            enemy.OnIntentChanged += UpdateDamageUi; // Subscribe to the intent change event
            enemy.OnShieldChanged += UpdateShieldUI; // Subscribe to the shield change event
        }

        if (statusManager == null && enemy != null)
        {
            statusManager = enemy.GetComponent<StatusManager>();
        }

        if (statusManager != null)
        {
            statusManager.OnStatusesChanged += UpdateDamageUi;
        }

        UpdateDamageUi();
    }
    private void OnDisable()
    {
        if (enemy != null)
        {
            enemy.OnHealthChanged -= UpdateHealthUI; // Unsubscribe to prevent memory leaks
            enemy.OnIntentChanged -= UpdateDamageUi; // Subscribe to the intent change event
            enemy.OnShieldChanged -= UpdateShieldUI; // Subscribe to the shield change event
        }

        if (statusManager != null)
        {
            statusManager.OnStatusesChanged -= UpdateDamageUi;
        }
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

    private void UpdateDamageUi()
    {
        if (enemy == null)
        {
            return;
        }

        enemyIntent = enemy.CurrentIntent; // Get the current intent from the Enemy script

        if (enemyIntent.phases != null)
        {
            int displayAmount = enemyIntent.amount;

            if (enemyIntent.phases.phaseType == PhaseType.Attack)
            {
                IStatusReciver sourceStatuses = enemy.GetComponent<IStatusReciver>();
                if (sourceStatuses != null)
                {
                    displayAmount += sourceStatuses.GetTotalStatusAmount(StatusType.Strength);
                }
            }

            if (damageText != null)
            {
                damageText.text = displayAmount.ToString(); // Update the damage text based on the intent
            }

            if (intentIcon != null)
            {
                intentIcon.sprite = enemyIntent.phases.IntentIcon; // Update the intent icon based on the intent
            }

        }
    }
}
