using System;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, ICombatTarget //this holds both ItakeDamage and IBuffable
{
    //[SerializeField] private int currentHealth = 100;
    //[SerializeField] private int maxHealth = 100;

    [SerializeField] private CurrentRunState runState; // Inject the Single Source of Truth

    [SerializeField] private int shield = 0;


    public int CurrentStrength { get; private set; }

    [SerializeField] private TurnManager turnManager;
    //[Header("Inventory")]
    //[SerializeField] private useables[] useables
    //[SerializeField] private totems[] totems

    public static event Action<int, int> OnPlayerTookDamage; // for BattleAnalyticsManager to track dmg and shield

    public UnityAction<int, int> OnHealthChanged; //we don'r remake this to event bc im lazy :)
    public UnityAction<int> OnShield; //but I do like event action better so we dont need to link up in inspector like here.
    public static event Action OnDeath;
    private void Start()
    {

        if (runState.currentMaxHealth == 0)
        {
            Debug.LogWarning("RunState health is 0! Auto-resetting for testing purposes.");
            runState.ResetRun();
        }

        OnHealthChanged?.Invoke(runState.currentHealth, runState.currentMaxHealth);
    }

    private void OnEnable()
    {
        turnManager.OnPlayerTurnStart += ResetShield;
    }


    //Interfaces
    public void TakeDamage(int dmg)
    {

        int shieldAbsorbed = 0;
        int damageHit = 0;
        // 1. Let the VOLATILE shield absorb damage first
        if (shield > 0)
        {
            if (shield >= dmg)
            {
                shield -= dmg; // Shield absorbs everything
                dmg = 0;       // No damage left to hit health
            }
            else
            {
                shieldAbsorbed = dmg;
                dmg -= shield; // Shield absorbs what it can, remainder goes to health
                shield = 0;    // Shield is destroyed
            }

            // Tell the UI the shield took a hit
            OnShield?.Invoke(shield);
        }

        // 2. Apply remaining damage to the PERSISTENT Backpack Health
        if (dmg > 0)
        {
            damageHit = dmg; // For analytics tracking
            runState.currentHealth -= dmg;
            runState.currentHealth = Mathf.Max(runState.currentHealth, 0); // Prevent negative HP

            // Update the Health Bar UI
            OnHealthChanged?.Invoke(runState.currentHealth, runState.currentMaxHealth);

            // Check for death
            if (runState.currentHealth <= 0)
            {
                OnDeath?.Invoke();
            }
        }

        OnPlayerTookDamage?.Invoke(damageHit, shieldAbsorbed);
    }

    void Strengthen(int amount) // we dotn use this
    {
        CurrentStrength += amount;
    }

    public void Heal(int amount)
    {
        runState.currentHealth += amount;

        // Clamp to max health
        runState.currentHealth = Mathf.Min(runState.currentHealth, runState.currentMaxHealth);

        OnHealthChanged?.Invoke(runState.currentHealth, runState.currentMaxHealth);
    }

    public void Shield(int amount) 
    { 
        shield += amount;
        OnShield?.Invoke(shield); //for updating shield UI

        // implement so the shield goes away after enemy round is over
    }

    private void ResetShield()
    {
        if (shield > 0)
        {
            shield = 0;
            Debug.Log("Shield reset at end of enemy turn.");
        }
        OnShield?.Invoke(shield); //update shield UI
    }

}